using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioClipDatabase", menuName = "AudioClipDatabase")]
public class AudioClipDatabase : ScriptableObject
{
	[System.Serializable]
	public struct AudioClipData
	{
		public string Name;
		public AudioClip AudioClip;
	}

	public string Name = "";
	public AudioClipData[] AudioClips;

	public AudioClip GetAudioClip(string name)
	{
		foreach(AudioClipData clipData in AudioClips)
		{
			if(clipData.Name == name)
			{
				return clipData.AudioClip;
			}
		}

		Debug.Log("No Audio Clip with AudioClipData name: \"" + name + "\" found.");
		return null;
	}
}
