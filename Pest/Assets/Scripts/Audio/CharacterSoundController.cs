using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CharacterSoundController : MonoBehaviour
{
	AudioSource audioSource = null;

	// Start is called before the first frame update
	void Start()
	{
		audioSource = GetComponent<AudioSource>();
	}

	void PlaySoundEffect(string soundEffectName)
	{
		audioSource.clip = null;
		audioSource.clip = AudioManager.Instance.GetAudioClip(soundEffectName);

		if(audioSource.clip != null)
		{
			audioSource.PlayOneShot(audioSource.clip);
		}
	}
}
