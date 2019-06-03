using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
	Animator animator = null;
	public float Velocity { get; set; } = 0.0f;
	public bool IsSneaking { get; set; } = false;
	public bool IsGrounded { get; set; } = true;
	public bool IsClimbing { get; set; } = false;
	public float ClimbingSpeed { get; set; } = 0.0f;

	[SerializeField] float longIdleTime = 0.0f;
	float idleTimer = 0.0f;

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

	// Start is called before the first frame update
	void Start()
    {
		animator = GetComponent<Animator>();
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

		if(Velocity < 0.1f)
		{
			idleTimer += Time.deltaTime;

			if(idleTimer >= longIdleTime)
			{
				animator.SetBool(longIdleHash, true);
			}
		}
		else
		{
			animator.SetBool(longIdleHash, false);
			idleTimer = 0.0f;
		}
    }

	public void Jump()
	{
		animator.SetTrigger(jumpHash);
		idleTimer = 0.0f;
	}

	public void HideBegin()
	{
		animator.SetTrigger(hideBeginHash);
	}

	public void HideEnd()
	{
		animator.SetTrigger(hideEndHash);
	}

	public void TakeDown()
	{
		animator.SetTrigger(takeDownHash);
	}
}
