﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using RewiredConsts;

public class PlayerController : MonoBehaviour
{
	/*********************************************************/
	/* * * * * * * * * * * * * * * * * * * * * * * * * * * * */
	/*						Variables						 */
	/* * * * * * * * * * * * * * * * * * * * * * * * * * * * */
	/*********************************************************/

	/********************General variables********************/
	CharacterController charController = null;
	Rewired.Player player = null;

	/******************health system variables*****************/
	[Header("Health System")]
	[SerializeField] int maxHitPoints = 3;
	/// <summary>
	/// current hit points of the chracter
	/// </summary>
	public int HitPoints { get; set; }

	/********************movement variables********************/
	[Header("Movement")]
	[Header("On Ground")]
	[SerializeField] float walkSpeed = 5.0f;
	[SerializeField] float sneakSpeed = 2.5f;
	[SerializeField] float runSpeed = 7.5f;
	[SerializeField] float turnSpeed = 10.0f;
	[SerializeField] float jumpStrenght = 11.0f;
	bool canJump = true;
	bool hasJumped = false;
	bool isSneaking = false;

	/********************climbing variables********************/
	[Header("Climbing")]
	[SerializeField] float climbingSpeed = 3.0f;
	bool isClimbing = false;
	ClimbableObjectController climbableObject = null;
	bool rotateTowardsClimbable = false;
	bool moveTowardsClimbable = false;

	Vector3 moveDirection = Vector3.zero;

	/********************physics variables*********************/
	[Header("Physics")]
	[SerializeField] float gravityMultiplier = 4.0f;

	/********************hiding variables**********************/
	[Header("Hiding")]
	bool isHiding = false;
	bool lastIsHiding = false;
	bool startedHide = false;
	public HidingObjectController HidingObject { get; set; } = null;

	/*******************animation variables********************/
	[Header("Animation")]
	[SerializeField] PlayerAnimationController animationController = null;


	/*********************************************************/
	/* * * * * * * * * * * * * * * * * * * * * * * * * * * * */
	/*						Methods							 */
	/* * * * * * * * * * * * * * * * * * * * * * * * * * * * */
	/*********************************************************/

	/**************************Start***************************/
	// Use this for initialization
	void Start ()
	{
		charController = GetComponent<CharacterController>();
		player = ReInput.players.GetPlayer(RewiredConsts.Player.Player0);

		HitPoints = maxHitPoints;
	}

	/*************************Update***************************/
	// Update is called once per frame
	void Update ()
	{
		if(isHiding)
		{
			HandleHiding();
		}
		else
		{
			HandleMovement();
		}

		if(player.GetButtonDown(Action.CharacterControl.Interact))
		{
			HandleInteraction();
		}
		else if(player.GetButtonDown(Action.CharacterControl.Block))
		{
			HandleBlocking();
		}
		else if(player.GetButtonDown(Action.CharacterControl.Attack))
		{
			HandleAttacking();
		}


		lastIsHiding = isHiding;

		animationController.Velocity = charController.velocity.magnitude;
		animationController.IsSneaking = isSneaking;
		animationController.IsGrounded = charController.isGrounded;
		animationController.IsClimbing = isClimbing;
	}

	#region movement methods

	/*********************HandleMovement***********************/
	void HandleMovement()
	{
		if(isClimbing)
		{
			HandleClimbing();
		}
		else if(!isHiding)
		{
			HandleMoving();
		}
	}

	/**********************HandleMoving************************/
	void HandleMoving()
	{
		if(charController.isGrounded) //Character is grounded calculate movement from input
		{
			//get forward and right vector, project onto horizontal plane to cancel out camera tilt
			//normalize to use as vectors for x and z movement direction vectors
			//Up input will move character away from camera, down towards, etc...
			Vector3 forwardVector = Vector3.Normalize(Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up));
			Vector3 rightVector = Vector3.Normalize(Vector3.ProjectOnPlane(Camera.main.transform.right, Vector3.up));

			//multiply vectors with input to get weight of direction in total movement direction
			rightVector *= player.GetAxis(Action.CharacterControl.MoveHorizontal);
			forwardVector *= player.GetAxis(Action.CharacterControl.MoveVertical);

			//add forward and right vector to get movement direction
			moveDirection = Vector3.Normalize(forwardVector + rightVector);

			float moveSpeed = 0.0f;

			if(player.GetButton(Action.CharacterControl.Sneak))
			{
				moveSpeed = sneakSpeed;
				canJump = false;
				isSneaking = true;
			}
			else if(player.GetButton(Action.CharacterControl.Run))
			{
				moveSpeed = runSpeed;
				canJump = true;
				isSneaking = false;
			}
			else
			{
				moveSpeed = walkSpeed;
				canJump = true;
				isSneaking = false;
			}

			moveDirection *= moveSpeed;

			if(hasJumped)
			{
				hasJumped = false;
			}

			if(player.GetButton(Action.CharacterControl.Jump) && canJump)
			{
				canJump = false;
				moveDirection.y = jumpStrenght;
				hasJumped = true;

				animationController.Jump();
			}

			if(climbableObject != null)
			{
				climbableObject = null;
			}
		}

		if(moveDirection.x != 0.0f || moveDirection.z != 0.0f)
		{
			Quaternion newRotation = Quaternion.identity;
			newRotation.SetLookRotation(new Vector3(moveDirection.x, 0.0f, moveDirection.z));
			newRotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * turnSpeed);
			transform.rotation = newRotation;
		}

		if(player.GetButtonUp(Action.CharacterControl.Jump))
		{
			canJump = true;
		}

		// Apply gravity. Gravity is multiplied by deltaTime twice.
		// This is because gravity should be applied as an acceleration (ms^-2)
		moveDirection.y += Physics.gravity.y * gravityMultiplier * Time.deltaTime;

		charController.Move(moveDirection * Time.deltaTime);
	}

	/**********************HandleClimbing**********************/
	void HandleClimbing()
	{
		if(isClimbing)
		{
			if(charController.isGrounded)
			{
				//get direction to move away from climbable object
				Vector3 directionToMove = climbableObject.transform.position - transform.position;
				//project on horizontal plane
				directionToMove = Vector3.Normalize(Vector3.ProjectOnPlane(directionToMove, Vector3.up));
				//move character away from climbable object, depending on input
				charController.Move(directionToMove * charController.skinWidth * Mathf.Sign(player.GetAxis(Action.CharacterControl.MoveVertical)));

				climbableObject = null;
				isClimbing = false;
				moveTowardsClimbable = false;
			}
			else
			{
				moveDirection = Vector3.zero;
				moveDirection += player.GetAxis(Action.CharacterControl.MoveVertical) * transform.up;
				moveDirection *= climbingSpeed * 0.1f; //climbing speed has to be scaled down to allow working with whole numbers in the editor

				animationController.ClimbingSpeed = player.GetAxis(Action.CharacterControl.MoveVertical); //multiply animation speed by value of input (-1..0..1)

				charController.Move(moveDirection);
			}
		}

		if(rotateTowardsClimbable)
		{
			//make character look at the climbable object
			if(climbableObject != null)
			{
				Vector3 directionToLook = climbableObject.transform.position - transform.position;
				//project on horizontal plane
				directionToLook = Vector3.ProjectOnPlane(directionToLook, Vector3.up);
				//set look direction
				Quaternion newRotation = Quaternion.identity;
				newRotation.SetLookRotation(new Vector3(directionToLook.x, 0.0f, directionToLook.z));

				if(Quaternion.Angle(newRotation, transform.rotation) <= 0.1)
				{
					transform.LookAt(climbableObject.transform, Vector3.up);
					transform.rotation = newRotation;

					rotateTowardsClimbable = false;
				}

				//if current rotation is not looking at climbable object rotate character
				newRotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * turnSpeed * 5);
				transform.rotation = newRotation;
			}
		}

		if(moveTowardsClimbable && climbableObject != null)
		{
			if(charController.isGrounded)
			{
				moveTowardsClimbable = false;
			}

			Vector3 directionToMove = climbableObject.transform.position - climbableObject.ClimbDownPositionTransform.position;
			directionToMove = Vector3.Normalize(Vector3.ProjectOnPlane(directionToMove, Vector3.up)) * walkSpeed;

			Vector3 positionWithClimbableHeight = new Vector3(transform.position.x, climbableObject.transform.position.y, transform.position.z);
			Vector3 distanceCalculationPosition = Vector3.zero;

			RaycastHit hit;
			if(Physics.Raycast(positionWithClimbableHeight, directionToMove, out hit, float.MaxValue, ~0, QueryTriggerInteraction.Ignore)) //~0 -> raycast shall hit every layer
			{
				distanceCalculationPosition = hit.point;
			}

			if(Vector3.Distance(distanceCalculationPosition, positionWithClimbableHeight) <= charController.radius + charController.skinWidth)
			{
				Debug.Log(Vector3.Distance(distanceCalculationPosition, positionWithClimbableHeight));
				moveTowardsClimbable = false;
			}

			charController.Move(directionToMove * Time.deltaTime);
		}

		//if the player jumps on a climbable canJump needs to be set true otherwise next jump button press is not going to make the character jump
		if(!canJump)
		{
			canJump = true;
		}

		if(hasJumped)
		{
			hasJumped = false;
		}
	}
	#endregion

	/********************HandleInteraction*********************/
	void HandleInteraction()
	{
		if(HidingObject != null && lastIsHiding == isHiding)
		{
			isHiding = true;
		}
	}

	/**********************HandleHiding************************/
	void HandleHiding()
	{
		Vector3 directionToMove = HidingObject.HideAnimationStartPosition.position - transform.position;

		if(directionToMove.magnitude > 0.1f)
		{
			charController.Move(directionToMove * walkSpeed * Time.deltaTime);
		}
		else
		{
			if(!startedHide)
			{
				//rotate towards hiding place based on HideAnimationStartPosition (Make sure green arrow points upwards !!!!)
				if(Quaternion.Angle(transform.rotation, HidingObject.HideAnimationStartPosition.rotation) > 1.0f)
				{
					float angle = Quaternion.Angle(transform.rotation, HidingObject.HideAnimationStartPosition.rotation);
					Quaternion newRotation = Quaternion.identity;
					newRotation.SetLookRotation(HidingObject.HideAnimationStartPosition.forward);
					newRotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * turnSpeed);
					transform.rotation = newRotation;
				}
				else
				{
					animationController.HideBegin();
					startedHide = true;
					//charController.detectCollisions = false;
				}
			}

			if(player.GetButtonDown(Action.CharacterControl.Interact) && isHiding)
			{
				if(startedHide)
				{
					animationController.HideEnd();
				}
				isHiding = false;
				startedHide = false;
				//charController.detectCollisions = true;
			}
		}
	}

	/*********************HandleBlocking***********************/
	void HandleBlocking()
	{
		//XXXXXXXXXX_________DO SOMETHING_________XXXXXXXXXX//
	}

	/*********************HandleAttacking**********************/
	void HandleAttacking()
	{
		//XXXXXXXXXX_________DO SOMETHING_________XXXXXXXXXX//
	}

	#region Unity callbacks
	/**********************OnTriggerEnter**********************/  //////////////////D A S  W I R D  N I E  I M  L E B E N  F U N K T I O N I E R E N//////////////////
	private void OnTriggerEnter(Collider other)                   //////////////////        S P I E L E R  H A T   K E I N E N  T R I G G E R       //////////////////
	{                                                             //////////////////         ! ! ! !    Ü B E R A R B E I T E N    ! ! ! !          //////////////////
		#region climbing
		if(other.tag == "Climbable" && isClimbing)
		{
			climbableObject = other.GetComponent<ClimbableObjectController>();
		}

		if(other.tag == "Climbable" && !isClimbing)
		{
			if(charController.isGrounded)
			{
				//move character up so it is not touching the floor
				charController.Move(new Vector3(0, charController.skinWidth, 0));
			}
			climbableObject = other.gameObject.GetComponent<ClimbableObjectController>();
			isClimbing = true;
			rotateTowardsClimbable = true;
		}

		if(other.tag == "ClimbDownPosition" && !isClimbing && !hasJumped && player.GetButton(Action.CharacterControl.Sneak))
		{
			climbableObject = other.GetComponentInParent<ClimbableObjectController>();
			if(climbableObject != null)
			{
				if(climbableObject.ClimbDownPositionTransform)
				{
					transform.position = climbableObject.ClimbDownPositionTransform.position;
					isClimbing = true;
					rotateTowardsClimbable = true;
					moveTowardsClimbable = true;
				}
			}
		}
		#endregion
	}

	/**********************OnTriggerExit***********************/
	private void OnTriggerExit(Collider other)
	{
		#region climbing
		if(other.tag == "Climbable" && isClimbing)
		{
			if(!charController.isGrounded)
			{
				//get direction to move away from climbable object
				Vector3 directionToMove = climbableObject.transform.position - transform.position;
				//project on horizontal plane
				directionToMove = Vector3.Normalize(Vector3.ProjectOnPlane(directionToMove, Vector3.up));
				//move character away from climbable object, depending on input
				charController.Move(directionToMove * charController.skinWidth * Mathf.Sign(player.GetAxis(Action.CharacterControl.MoveVertical)));

				climbableObject = null;
				isClimbing = false;
				moveTowardsClimbable = false;
			}
		}
		#endregion
	}
	#endregion
}
