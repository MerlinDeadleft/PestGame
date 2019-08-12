using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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
