using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Cinemachine;

public class VignetteController : MonoBehaviour
{
	PostProcessVolume postProcessVolume = null;
	Vignette vignette = null;
	[SerializeField] Transform playerModel = null;
	[SerializeField] Transform VignetteCenter = null;
	[SerializeField] PlayerController playerController = null;
	[SerializeField] float maxDistanceToLight = 10.0f;
	[SerializeField, Range(0.0f, 1.0f)] float maxVignetteIntensity = 10.0f;
	[SerializeField] float cameraMinDistance = 4.0f;
	float defaultCameraDistance = 0.0f;
	bool zoom = false;
	CinemachineFramingTransposer framingTransposer = null;
	[SerializeField] float zoomSpeed = 1.0f;
	[SerializeField] float reverseZoomSpeed = 1.0f;
	[SerializeField] float waitForZoomTime = 1.0f;
	float waitForZoomTimer = 0.0f;

	List<Light> lights = new List<Light>();
	[SerializeField] List<Light> contributingLight = new List<Light>();

	// Start is called before the first frame update
	void Start()
	{
		postProcessVolume = GetComponent<PostProcessVolume>();
		postProcessVolume.profile.TryGetSettings(out vignette);

		Light[] allLights = FindObjectsOfType<Light>();

		foreach(Light light in allLights)
		{
			if(light.type != LightType.Directional)
			{
				lights.Add(light);
			}
		}

		StartCoroutine(Initialize());
	}

	IEnumerator Initialize()
	{
		yield return new WaitForEndOfFrame(); // need to wait for a frame for Cinemachine to init

		framingTransposer = Camera.main.GetComponent<CinemachineBrain>().
			ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>().
			GetCinemachineComponent<CinemachineFramingTransposer>();

		defaultCameraDistance = framingTransposer.m_CameraDistance;
	}

	// Update is called once per frame
	void Update()
	{
		if(framingTransposer == null)
		{
			return; //only execute update after initialize finished
		}

		vignette.center.value = Camera.main.WorldToViewportPoint(VignetteCenter.position);

		float vignetteIntensity = CalculateVignetteIntensity();
		vignette.intensity.value = Mathf.Min(vignetteIntensity, maxVignetteIntensity);
		playerController.UpdateLightEffect(vignetteIntensity);

		if(zoom)
		{
			framingTransposer.m_CameraDistance = Mathf.Lerp(framingTransposer.m_CameraDistance, cameraMinDistance, Time.deltaTime * zoomSpeed);
		}
		else
		{
			framingTransposer.m_CameraDistance = Mathf.Lerp(framingTransposer.m_CameraDistance, defaultCameraDistance, Time.deltaTime * reverseZoomSpeed);
		}
	}

	float CalculateVignetteIntensity()
	{
		contributingLight.Clear();
		float intensity = 0.0f;
		int contributingLights = 0;

		foreach(Light light in lights)
		{
			if(light != null && light.enabled)
			{
				float distanceToPlayer = Vector3.Magnitude(light.transform.position - playerModel.position);

				if(distanceToPlayer <= maxDistanceToLight)
				{
					RaycastHit hit;
					Ray ray = new Ray(light.transform.position, (playerModel.transform.position + Vector3.up) - light.transform.position);

					if(Physics.Raycast(ray, out hit, maxDistanceToLight, Physics.AllLayers, QueryTriggerInteraction.Ignore))
					{
						if(hit.transform.tag == "Player")
						{
							contributingLight.Add(light);
							contributingLights++;

							intensity += Mathf.Clamp01(1.0f / distanceToPlayer);
						}
					}
				}
			}
		}

		if(contributingLights > 0)
		{
			if(!zoom)
			{
				waitForZoomTimer += Time.deltaTime;

				if(waitForZoomTimer >= waitForZoomTime)
				{
					zoom = true;
				}
			}

			return intensity / contributingLights;
		}
		else
		{
			if(zoom)
			{
				zoom = false;
				waitForZoomTimer = 0.0f;
			}

			return 0;
		}
	}
}