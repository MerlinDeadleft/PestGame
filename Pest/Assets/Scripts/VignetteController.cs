using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class VignetteController : MonoBehaviour
{
	PostProcessVolume postProcessVolume = null;
	Vignette vignette = null;
	[SerializeField] Transform playerModel = null;
	[SerializeField] PlayerController playerController = null;
	[SerializeField] float maxDistanceToLight = 10.0f;

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
	}

	// Update is called once per frame
	void Update()
	{
		vignette.center.value = Camera.main.WorldToViewportPoint(playerModel.position);

		float vignetteIntensity = CalculateVignetteIntensity();
		vignette.intensity.value = vignetteIntensity;
		playerController.UpdateLightEffect(vignetteIntensity);
	}

	float CalculateVignetteIntensity()
	{
		contributingLight.Clear();
		float intensity = 0.0f;
		int contributingLights = 0;

		foreach(Light light in lights)
		{
			if(light != null)
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
			return intensity / contributingLights;
		}
		else
		{
			return 0;
		}
	}
}