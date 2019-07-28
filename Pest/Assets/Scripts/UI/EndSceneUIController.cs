using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSceneUIController : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}

	// Update is called once per frame
	void Update()
    {
        
    }

	public void PlayAgain()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("Blocking_Kanalisation");
	}

	public void Leave()
	{
		Application.Quit();
	}
}
