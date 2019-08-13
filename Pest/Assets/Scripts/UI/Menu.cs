using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
	public MainMenuController mainMenu = null;
	public GameObject DefaultButton = null;
	public Menu parentMenu = null;

	public void OpenChildMenu(Menu child)
	{
		mainMenu.CurrentMenu = child;
		child.gameObject.SetActive(true);
		gameObject.SetActive(false);
	}

	public void OpenParentMenu()
	{
		if(parentMenu != null)
		{
			mainMenu.CurrentMenu = parentMenu;
			parentMenu.gameObject.SetActive(true);
			gameObject.SetActive(false);
		}
	}
}
