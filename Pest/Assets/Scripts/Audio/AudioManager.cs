using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public static AudioManager Instance { get; set; } = null;

	[SerializeField] AudioClipDatabase[] audioClipDatabases;

	private void Awake()
	{
		//if(Instance != null && Instance != this)
		//{
		//	Destroy(this.gameObject);
		//}
		//else
		//{
		//	Instance = this;
		//}

		//DontDestroyOnLoad(gameObject);

		if(Instance != this)
		{
			Instance = this;
		}
	}

	public AudioClip GetAudioClip(string name)
	{
		string[] splitName = name.Split(' '); //splitName [0] is databaseName splitname[1] is clip name

		if(splitName.Length != 2)
		{
			Debug.Log("Parameter format is not following Audio Clip naming rules. Can not continue searching for audio clip\n" +
				"Parameter format looks like this: [Database Name] [Space] [Audio Clip Name]");
			return null;
		}

		foreach(AudioClipDatabase database in audioClipDatabases)
		{
			if(database.Name == splitName[0])
			{
				return database.GetAudioClip(splitName[1]);
			}
		}

		Debug.LogWarning("Could not find Audio Clip Database with name: \"" + splitName[0] + "\" returning null");
		return null;
	}
}
