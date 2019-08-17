using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Rewired;

public class MainMenuController : MonoBehaviour
{
	public enum InputDevices { None, Mouse, Keyboard, Controller }

	[SerializeField] bool isPauseMenu = false;
	[SerializeField] EventSystem eventSystem = null;
	[SerializeField] Menu PS4Menu = null;
	[SerializeField] Menu Menu = null;
	public Menu CurrentMenu { get; set; } = null;
	Menu lastMenu = null;

	Player player;
	[SerializeField] InputDevices currentDevice = InputDevices.None;
	[SerializeField] InputDevices lastDevice = InputDevices.None;

	// Start is called before the first frame update
	void Start()
	{
		if(eventSystem == null)
		{
			eventSystem = GameObject.Find("Rewired Event System").GetComponent<EventSystem>();
		}

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

	private void OnEnable()
	{
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}

	void Update()
	{
		if(isPauseMenu)
		{
			if(player.GetButtonDown(RewiredConsts.Action.UIControl.ResumeGame))
			{
				ResumeGame();
				return;
			}
		}

		if(player.GetButtonDown(RewiredConsts.Action.UIControl.Decline))
		{
			CurrentMenu.OpenParentMenu();
		}

		if(player.IsCurrentInputSource(RewiredConsts.Action.UIControl.Any, ControllerType.Mouse))
		{
			currentDevice = InputDevices.Mouse;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
		else if(player.IsCurrentInputSource(RewiredConsts.Action.UIControl.Any, ControllerType.Keyboard))
		{
			currentDevice = InputDevices.Keyboard;
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
		else if(player.IsCurrentInputSource(RewiredConsts.Action.UIControl.Any, ControllerType.Joystick))
		{
			currentDevice = InputDevices.Controller;
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
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

	public void LoadMainMenu()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("Mainmenue");
	}

	public void QuitGame()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

	public void ResumeGame()
	{
		if(currentDevice == InputDevices.Controller)
		{
			bool isNotTopMenu = CurrentMenu.OpenParentMenuWithReturn();

			while(isNotTopMenu)
			{
				isNotTopMenu = CurrentMenu.OpenParentMenuWithReturn();
			}
		}
		else
		{
			if(CurrentMenu.OpenParentMenuWithReturn())
			{
				return;
			}
		}

		Time.timeScale = 1.0f;

		gameObject.SetActive(false);

		player.controllers.maps.SetAllMapsEnabled(false);
		player.controllers.maps.SetMapsEnabled(true, RewiredConsts.Category.CharacterControl);

		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}
}
