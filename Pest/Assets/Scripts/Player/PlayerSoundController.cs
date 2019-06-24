using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
	[SerializeField] GameObject playerGameObject = null;
	PlayerAnimationController animationController = null;

	[Header("Footstep Delays")]
	[SerializeField] float walkDelay = 0.0f;
	[SerializeField] float sneakDelay = 0.0f;
	[SerializeField] float runDelay = 0.0f;
	float timer = 0.0f;

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
