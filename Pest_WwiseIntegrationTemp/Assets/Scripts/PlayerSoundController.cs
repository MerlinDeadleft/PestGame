using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
	[SerializeField] PlayerAnimationController animationController = null;

	[Header("Footstep Delays")]
	[SerializeField] float walkDelay = 0.0f;
	[SerializeField] float sneakDelay = 0.0f;
	[SerializeField] float runDelay = 0.0f;
	float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		timer += Time.deltaTime;

		if(animationController.Velocity >= 0.1f && animationController.IsGrounded)
		{
			float delay = walkDelay;
			if(animationController.IsSneaking)
			{
				//sneakcycle
				delay = sneakDelay;
			}
			else if(animationController.Velocity >= 5.1f)
			{
				//runcycle
				delay = runDelay;
			}
			if(timer >= delay)
			{
				//walkcycle
				AkSoundEngine.PostEvent("Footstep_MC_Rat", gameObject);
				timer = 0.0f;
			}


			//if(timer >= delay)
			//{
			//	timer = 0.0f;
			//	if(animationController.IsSneaking)
			//	{
			//		//sneakcycle
			//		AkSoundEngine.PostEvent("Sneak_MC_Rat", gameObject);
			//	}
			//	else if(animationController.Velocity >= 5.1f)
			//	{
			//		//runcycle
			//		AkSoundEngine.PostEvent("Run_MC_Rat", gameObject);
			//	}
			//	else
			//	{
			//		AkSoundEngine.PostEvent("Footstep_MC_Rat", gameObject);
			//	}
			//}
		}
    }

	public void Jump()
	{
		AkSoundEngine.PostEvent("Jump_MC_Rat", gameObject);
	}
}
