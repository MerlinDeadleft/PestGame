using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Rewired;

public class MainMenuController : MonoBehaviour
{
	public enum InputDevices { None, Mouse, Keyboard, Controller }

	[SerializeField] EventSystem eventSystem = null;
	[SerializeField] Menu PS4Menu = null;
	[SerializeField] Menu Menu = null;
	public Menu CurrentMenu { get; set; } = null;
	Menu lastMenu = null;

	Player player;
	InputDevices currentDevice = InputDevices.None;
	InputDevices lastDevice = InputDevices.None;

    // Start is called before the first frame update
    void Start()
    {
		player = ReInput.players.GetPlayer(0);
		player.controllers.maps.SetAllMapsEnabled(false);
		player.controllers.maps.SetMapsEnabled(true, RewiredConsts.Category.UIControl);

#if UNITY_PS4
		eventSystem.SetSelectedGameObject(PS4Menu.DefaultButton);
		currentDevice = InputDevices.Controller;
		currentMenu = PS4Menu;
#else
		eventSystem.SetSelectedGameObject(Menu.DefaultButton);
		currentDevice = InputDevices.Mouse;
		CurrentMenu = Menu;
#endif
	}

	void Update()
	{
		if(player.GetButton(RewiredConsts.Action.UIControl.Decline))
		{
			CurrentMenu.OpenParentMenu();
		}

		if(player.IsCurrentInputSource(RewiredConsts.Action.UIControl.Any, ControllerType.Mouse))
		{
			currentDevice = InputDevices.Mouse;
		}
		else if(player.IsCurrentInputSource(RewiredConsts.Action.UIControl.Any, ControllerType.Keyboard))
		{
			currentDevice = InputDevices.Keyboard;
		}
		else if(player.IsCurrentInputSource(RewiredConsts.Action.UIControl.Any, ControllerType.Joystick))
		{
			currentDevice = InputDevices.Controller;
		}

		if(currentDevice != lastDevice)
		{
			if(currentDevice == InputDevices.Controller)
			{
				eventSystem.SetSelectedGameObject(CurrentMenu.DefaultButton);
			}
			else if(currentDevice == InputDevices.Keyboard)
			{
				eventSystem.SetSelectedGameObject(CurrentMenu.DefaultButton);
			}
			else if(currentDevice == InputDevices.Mouse)
			{
				eventSystem.SetSelectedGameObject(null);
			}
		}

		if(CurrentMenu != lastMenu)
		{
			if(currentDevice == InputDevices.Controller || currentDevice == InputDevices.Keyboard)
			{
				eventSystem.SetSelectedGameObject(CurrentMenu.DefaultButton);
			}
		}

		lastDevice = currentDevice;
		lastMenu = CurrentMenu;
	}

	public void LoadNextLevel(string levelToLoad)
	{
		CutsceneController.sceneToLoad = levelToLoad;
		CutsceneController.cutsceneToShow = 0;

		UnityEngine.SceneManagement.SceneManager.LoadScene("Loading Screen");
	}

	public void QuitGame()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}
}
