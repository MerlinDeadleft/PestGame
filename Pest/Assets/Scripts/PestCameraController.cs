using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Rewired;

public class PestCameraController : MonoBehaviour
{
	CinemachineFramingTransposer framingTransposer = null;
	Player player = null;
	float xOffset = 0.0f;
	float yOffset = 0.0f;
	[SerializeField] float lookSensitivity = 1.0f;
	float mouseLookSensitivity = 0.005f;
	float controllerLookSensitivity = 0.5f;
	bool lastInputFromController = false;

	List<Light> lights = new List<Light>();
	public Light SelectedLight { get; set; } = null;

	// Start is called before the first frame update
	void Start()
    {
		player = ReInput.players.GetPlayer(RewiredConsts.Player.Player0);
		StartCoroutine(Initialize());

		Light[] allLights = FindObjectsOfType<Light>();

		foreach(Light light in allLights)
		{
			if(light.type != LightType.Directional)
			{
				lights.Add(light);
			}
		}
	}

	IEnumerator Initialize()
	{
		yield return new WaitForEndOfFrame(); // need to wait for a frame for Cinemachine to init

		framingTransposer = Camera.main.GetComponent<CinemachineBrain>().
			ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>().
			GetCinemachineComponent<CinemachineFramingTransposer>();
	}

	// Update is called once per frame
	void Update()
    {
		if(framingTransposer == null)
		{
			return; //only execute update after initialize finished
		}

		if(player.IsCurrentInputSource(RewiredConsts.Action.CharacterControl.LookHorizontal, ControllerType.Joystick) || player.IsCurrentInputSource(RewiredConsts.Action.CharacterControl.LookHorizontal, ControllerType.Keyboard))
		{
			lastInputFromController = true;
		}
		else
		{
			lastInputFromController = false;
		}

		if(lastInputFromController)
		{
			xOffset = -Mathf.Clamp(player.GetAxis(RewiredConsts.Action.CharacterControl.LookHorizontal) * controllerLookSensitivity * lookSensitivity, -0.5f, 0.5f);
			yOffset = Mathf.Clamp(player.GetAxis(RewiredConsts.Action.CharacterControl.LookVertical) * controllerLookSensitivity * lookSensitivity, -0.35f, 0.35f);
		}
		else
		{
			xOffset = Mathf.Clamp(xOffset - player.GetAxis(RewiredConsts.Action.CharacterControl.LookHorizontal) * mouseLookSensitivity * lookSensitivity, -0.5f, 0.5f);
			yOffset = Mathf.Clamp(yOffset + player.GetAxis(RewiredConsts.Action.CharacterControl.LookVertical) * mouseLookSensitivity * lookSensitivity, -0.35f, 0.35f);
		}

		if(player.GetButtonDown(RewiredConsts.Action.CharacterControl.ResetLook))
		{
			xOffset = 0.0f;
			yOffset = 0.0f;
		}

		framingTransposer.m_ScreenX = 0.5f + xOffset;
		framingTransposer.m_ScreenY = 0.5f + yOffset;

		DetermineClosestLight();
	}

	void DetermineClosestLight()
	{
		List<GameObject> closeLights = new List<GameObject>();

		foreach(Light light in lights)
		{
			if(light.enabled)
			{
				GameObject lightGO = light.gameObject;
				Vector3 viewPortPosition = Camera.main.WorldToViewportPoint(lightGO.transform.position);
				if(viewPortPosition.x > 0.35f && viewPortPosition.x < 0.65f)
				{
					if(viewPortPosition.y > 0.25f && viewPortPosition.y < 0.75f)
					{
						closeLights.Add(lightGO);
					}
				}
			}
		}

		GameObject closestLight = null;
		Vector3 closestViewPortPos = Vector3.zero;

		if(closeLights.Count > 0)
		{
			foreach(GameObject lightGO in closeLights)
			{
				if(closestLight == null)
				{
					closestLight = closeLights[0];
					closestViewPortPos = Camera.main.WorldToViewportPoint(lightGO.transform.position);
					continue;
				}

				Vector3 viewPortPosition = Camera.main.WorldToViewportPoint(lightGO.transform.position);
				if(viewPortPosition.z < closestViewPortPos.z)
				{
					closestLight = lightGO;
					closestViewPortPos = viewPortPosition;
				}
			}

			if(!(closestViewPortPos.z > 0.0f && closestViewPortPos.z < 15.0f)) //if z is outside 0-15 range
			{
				closestLight = null;
			}
		}

		if(closestLight != null)
		{
			SelectedLight = closestLight.GetComponent<Light>();
		}
		else
		{
			SelectedLight = null;
		}
	}
}
