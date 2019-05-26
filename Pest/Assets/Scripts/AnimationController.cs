﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
	[SerializeField] Animator animator = null;
	public float Velocity { get; set; } = 0.0f;
	public bool IsSneaking { get; set; } = false;
	public bool IsGrounded { get; set; } = true;
	public bool Jump { get; set; } = false;
	public bool IsClimbing { get; set; } = false;
	public float ClimbingSpeed { get; set; } = 0.0f;

	[SerializeField] float longIdleTime = 0.0f;
	float idleTimer = 0.0f;


	/********************animation hashes**********************/
	int velocityHash = Animator.StringToHash("Velocity");
	int longIdleHash = Animator.StringToHash("IsIdlingLong");
	int isSneakingHash = Animator.StringToHash("IsSneaking");
	int jumpHash = Animator.StringToHash("Jump");
	int isGroundedHash = Animator.StringToHash("IsGrounded");
	int isClimbingHash = Animator.StringToHash("IsClimbing");
	int climbingSpeedHash = Animator.StringToHash("ClimbingSpeed");

	// Start is called before the first frame update
	void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
		animator.SetBool(isClimbingHash, IsClimbing);
		animator.SetFloat(climbingSpeedHash, ClimbingSpeed);
		animator.SetBool(isGroundedHash, IsGrounded);
		animator.SetBool(isSneakingHash, IsSneaking);
		animator.SetFloat(velocityHash, Velocity);

		if(Jump)
		{
			animator.SetTrigger(jumpHash);
			Jump = false;
			idleTimer = 0.0f;
		}

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
}
