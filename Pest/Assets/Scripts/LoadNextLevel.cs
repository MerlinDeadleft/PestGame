using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextLevel : MonoBehaviour
{
	[SerializeField] string nextSceneName = "";

	private void OnTriggerEnter(Collider other)
	{
        int toLoadIdx = SceneManager.GetSceneByName(nextSceneName).buildIndex;
        int activeIdx = SceneManager.GetActiveScene().buildIndex;
        if (toLoadIdx != activeIdx)
        {
            var player = GameObject.FindWithTag("Player");
            if (player)
            {
                player.GetComponent<PlayerController>().CheckpointReached = false;
            }
        }

		CutsceneController.cutsceneToShow = 1;
		CutsceneController.sceneToLoad = nextSceneName;
        SceneManager.LoadScene("Loading Screen");
	}
}
