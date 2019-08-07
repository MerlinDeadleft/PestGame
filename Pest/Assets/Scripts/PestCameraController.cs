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

	[SerializeField] List<Light> lights        = new List<Light>();
	[SerializeField] List<Light> lightsInView  = new List<Light>();
                     Light       selectedLight = null;
    [SerializeField] int         lightFocus    = -1;
    [SerializeField] GameObject  crosshair     = null;

    public GameObject Crosshair { get { return crosshair; } set { crosshair = value; } }

	public bool IsBlinded { get; set; } = false;
	float maxLookDistanceMidifier = 1.0f;
	float lookSensitivityModifier = 1.0f;


	// Start is called before the first frame update
	void Start()
    {
        player = ReInput.players.GetPlayer(RewiredConsts.Player.Player0);

        crosshair.SetActive(false);

		StartCoroutine(Initialize());

		RefreshLightsList();
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
        if (crosshair)
        {
            crosshair.transform.rotation = Camera.main.transform.rotation;
        }

        if (framingTransposer == null)
		{
			return; //only execute update after initialize finished
		}

		if(IsBlinded)
		{
			maxLookDistanceMidifier = 0.5f;
			lookSensitivityModifier = 0.5f;
		}
		else
		{
			maxLookDistanceMidifier = 1.0f;
			lookSensitivityModifier = 1.0f;
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
			xOffset = -Mathf.Clamp(player.GetAxis(RewiredConsts.Action.CharacterControl.LookHorizontal) * controllerLookSensitivity * lookSensitivity * lookSensitivityModifier, -0.5f, 0.5f);
			yOffset = Mathf.Clamp(player.GetAxis(RewiredConsts.Action.CharacterControl.LookVertical) * controllerLookSensitivity * lookSensitivity * lookSensitivityModifier, -0.35f, 0.35f);
		}
		else
		{
			xOffset = Mathf.Clamp(xOffset - player.GetAxis(RewiredConsts.Action.CharacterControl.LookHorizontal) * mouseLookSensitivity * lookSensitivity * lookSensitivityModifier, -0.5f, 0.5f);
			yOffset = Mathf.Clamp(yOffset + player.GetAxis(RewiredConsts.Action.CharacterControl.LookVertical) * mouseLookSensitivity * lookSensitivity * lookSensitivityModifier, -0.35f, 0.35f);
		}

		if(player.GetButtonDown(RewiredConsts.Action.CharacterControl.ResetLook))
		{
			xOffset = 0.0f;
			yOffset = 0.0f;
		}

		framingTransposer.m_ScreenX = 0.5f + (xOffset * maxLookDistanceMidifier);
		framingTransposer.m_ScreenY = 0.5f + (yOffset * maxLookDistanceMidifier);

		GetLightsInView();
	}

	void RefreshLightsList()
	{
		lights.Clear();

		Light[] allLights = FindObjectsOfType<Light>();

		foreach(Light light in allLights)
		{
			if(light.type != LightType.Directional)
			{
				lights.Add(light);

			}
		}
	}

    // Talis' Code

    public void GetLightsInView()
    {
        RefreshLightsList();

        foreach (Light light in lights)
        {

            Vector3 viewPos = Camera.main.WorldToViewportPoint(light.transform.position);
            if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0)
            {
                if (!lightsInView.Contains(light) && light.enabled && light)
                {
                    lightsInView.Add(light);
                }
            }
            else
            {
                if (lightsInView.Contains(light) || !light)
                {
                    lightsInView.Remove(light);
                }

                if (light == null)
                {
                    RefreshLightsList();
                    return;
                }
            }
        }
    }

    public void FocusNextLight()
    {
        GetLightsInView();

        if(lightsInView.Count > 0)
        {
            lightFocus++;
        }

        if (lightFocus > lightsInView.Count - 1)
        {
            lightFocus = -1;
            crosshair.transform.parent = null;
            crosshair.SetActive(false);
            selectedLight = null;
        }

        if(lightFocus >= 0)
        {
            if (!lightsInView[lightFocus])
            {
                lightsInView.Remove(lightsInView[lightFocus]);
            }
            else
            {
                selectedLight = lightsInView[lightFocus];

                crosshair.SetActive(true);
                crosshair.transform.position = lightsInView[lightFocus].transform.position;
                crosshair.transform.parent = lightsInView[lightFocus].transform;
            }
        }
    }

    public void TurnOffSelectedLight()
    {
        if(lightFocus >= 0)
        {
            lightsInView.RemoveAt(lightFocus);
            lightFocus = -1;
            if (selectedLight)
            {
                selectedLight.enabled = false;
				if(selectedLight.transform.parent.GetComponentInChildren<ParticleSystem>() != null)
				{
					selectedLight.transform.parent.GetComponentInChildren<ParticleSystem>().Stop(true);
				}
            }
            crosshair.transform.parent = null;
            crosshair.SetActive(false);
            GetLightsInView();
        }
    }

    // Talis' Code ende
}
