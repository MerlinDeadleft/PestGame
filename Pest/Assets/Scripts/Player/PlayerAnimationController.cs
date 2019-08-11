using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
	Animator animator = null;
	AnimatorOverrideController animatorOverrideController = null;

	public float Velocity { get; set; } = 0.0f;
	public bool IsSneaking { get; set; } = false;
	public bool IsGrounded { get; set; } = true;
	public bool IsClimbing { get; set; } = false;
	public float ClimbingSpeed { get; set; } = 0.0f;
	public bool IsBlinded { get; set; } = false;
	public bool CanMove { get { return animator.GetBool(canMoveHash); } set { animator.SetBool(canMoveHash, value); } }

	[SerializeField] float longIdleTime = 0.0f;
	[SerializeField] float idleTimer = 0.0f;
	[SerializeField] AnimationClip longIdleClip1 = null;
	[SerializeField] AnimationClip longIdleClip2 = null;
	
	[Header("Sound Stuff")]
	[SerializeField] PlayerSoundController soundController = null;

	/********************animation hashes**********************/
	int velocityHash = Animator.StringToHash("Velocity");
	int longIdleHash = Animator.StringToHash("IsIdlingLong");
	int isSneakingHash = Animator.StringToHash("IsSneaking");
	int jumpHash = Animator.StringToHash("Jump");
	int isGroundedHash = Animator.StringToHash("IsGrounded");
	int isClimbingHash = Animator.StringToHash("IsClimbing");
	int climbingSpeedHash = Animator.StringToHash("ClimbingSpeed");
	int hideBeginHash = Animator.StringToHash("HideBegin");
	int hideEndHash = Animator.StringToHash("HideEnd");
	int takeDownHash = Animator.StringToHash("Takedown");
	int isBlindedHash = Animator.StringToHash("IsBlinded");
	int dieHash = Animator.StringToHash("Die");
	int canMoveHash = Animator.StringToHash("CanMove");
	int hideAnimhash = Animator.StringToHash("HideAnimation");
	int pickUpHash = Animator.StringToHash("PickUp");
	int castMagicHash = Animator.StringToHash("CastMagic");

	// Start is called before the first frame update
	void Start()
    {
		animator = GetComponent<Animator>();
		animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
		animator.runtimeAnimatorController = animatorOverrideController;
		animatorOverrideController["Rat_Idle2"] = longIdleClip2;

		soundController = GetComponent<PlayerSoundController>();
    }

    // Update is called once per frame
    void Update()
    {
		animator.SetBool(isClimbingHash, IsClimbing);
		animator.SetFloat(climbingSpeedHash, ClimbingSpeed);
		animator.SetBool(isGroundedHash, IsGrounded);
		animator.SetBool(isSneakingHash, IsSneaking);
		animator.SetFloat(velocityHash, Velocity);
		animator.SetBool(isBlindedHash, IsBlinded);
		//CanMove = animator.GetBool(canMoveHash);

		if(Velocity < 0.1f)
		{
			idleTimer += Time.deltaTime;

			if(idleTimer >= longIdleTime)
			{
				if(!animator.GetBool(longIdleHash))
				{
					if(animatorOverrideController["Rat_Idle2"] == longIdleClip1)
					{
						animatorOverrideController["Rat_Idle2"] = longIdleClip2;
					}
					else
					{
						animatorOverrideController["Rat_Idle2"] = longIdleClip1;
					}
					animator.SetBool(longIdleHash, true);
					idleTimer = 0.0f;
				}
				else
				{
					animator.SetBool(longIdleHash, false);
					idleTimer = 0.0f;
				}
			}
		}
		else
		{
			animator.SetBool(longIdleHash, true);
			idleTimer = 0.0f;
		}
    }

	public void Jump()
	{
		animator.SetTrigger(jumpHash);
		idleTimer = 0.0f;
	}

	public void HideBegin(int hideAnimation)
	{
		animator.SetTrigger(hideBeginHash);
		animator.SetFloat(hideAnimhash, hideAnimation);
	}

	public void HideEnd()
	{
		animator.SetTrigger(hideEndHash);
	}

	public void TakeDown()
	{
		animator.SetTrigger(takeDownHash);
	}

	public void Die()
	{
		animator.SetTrigger(dieHash);
		animator.SetBool(canMoveHash, false);
	}

	public void Attack()
	{

	}

	public void PickUp()
	{
		animator.SetTrigger(pickUpHash);
	}

	public void CastMagic()
	{
		animator.SetTrigger(castMagicHash);
	}
}
