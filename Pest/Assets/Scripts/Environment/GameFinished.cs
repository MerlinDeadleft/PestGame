using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFinished : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			if(other.GetComponent<PlayerController>().HasPotion)
			{
				CutsceneController.cutsceneToShow = 2;
				CutsceneController.sceneToLoad = "Mainmenue";
				SceneManager.LoadScene("Loading Screen");
			}
		}
	}
}
