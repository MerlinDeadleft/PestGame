using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
	[SerializeField] GameObject playerGameObject = null;
	PlayerAnimationController animationController = null;

    // Start is called before the first frame update
    void Start()
    {
		animationController = GetComponent<PlayerAnimationController>();
    }

	void PostSoundEvent(string eventName)
	{
		AkSoundEngine.PostEvent(eventName, playerGameObject);
	}
}
