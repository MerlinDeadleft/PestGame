using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Rewired;

public class CameraControlling : MonoBehaviour
{
	CinemachineFramingTransposer framingTransposer = null;
	Player player = null;
	float xOffset = 0.0f;
	[SerializeField] float lookSensitivity = 1.0f;
	float mouseLookSensitivity = 0.005f;
	float controllerLookSensitivity = 0.5f;
	bool lastInputFromController = false;

	// Start is called before the first frame update
	void Start()
    {
		player = ReInput.players.GetPlayer(RewiredConsts.Player.Player0);
		StartCoroutine(Initialize());
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

		if(player.IsCurrentInputSource(RewiredConsts.Action.CharacterControl.LookHorizontal, ControllerType.Joystick))
		{
			lastInputFromController = true;
		}
		else
		{
			lastInputFromController = false;
		}

		if(lastInputFromController)
		{
			xOffset = Mathf.Clamp(player.GetAxis(RewiredConsts.Action.CharacterControl.LookHorizontal) * controllerLookSensitivity * lookSensitivity, -0.5f, 0.5f);
		}
		else
		{
			xOffset = Mathf.Clamp(xOffset + player.GetAxis(RewiredConsts.Action.CharacterControl.LookHorizontal) * mouseLookSensitivity * lookSensitivity, -0.5f, 0.5f);
		}

		if(player.GetButtonDown(RewiredConsts.Action.CharacterControl.ResetLook))
		{
			xOffset = 0.0f;
		}

		framingTransposer.m_ScreenX = 0.5f + xOffset;
	}
}
