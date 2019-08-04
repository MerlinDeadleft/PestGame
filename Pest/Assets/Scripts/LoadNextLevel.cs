using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextLevel : MonoBehaviour
{
	[SerializeField] string nextSceneName = "";

	private void OnTriggerEnter(Collider other)
	{
		SceneManager.LoadScene(nextSceneName);
	}
}
