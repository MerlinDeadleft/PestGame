using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ModularAI;
using ModularAI.BT;
using System;

public class GuardBehaviour : EnemyBehaviour
{
	//PARENTCLASS FIELDS
	/*
	protected NavMeshAgent navMeshAgent = null;
	protected AIBrain aiBrain = null;

	protected float walkSpeed = 1.5f;
	protected float runSpeed = 4.5f;
	protected float minAwareness = 1.0f;
	protected float minDistance = 0.0f;

	protected GameObject player = null;

	public Node Root { get; private set; } = null;
	*/
	// PARENTCLASS FIELDS
	public enum LookDirections { Forward, Right, Behind, Left }

	EnemyController.MeleeInfo meleeInfo = new EnemyController.MeleeInfo();

	Transform pointToGuard = null;
	bool changeLookDirection = false;
	List<LookDirections> lookDirections = null;
	int currentLookDirectionIndex = 0;
	float timeToChangeLookDirection = 0.0f;
	float timer = 0.0f;

	float lookAngleThreshold = 0.5f;

	public GuardBehaviour(
		EnemyController.MeleeInfo info,
		NavMeshAgent agent,
		AIBrain brain,
		float walkingSpeed,
		float runningSpeed,
		float awarenessThreshold,
		float distanceToPlayerThreshold,
		GameObject playerGameObject
		)
	{
		Init(agent, brain, walkingSpeed, runningSpeed, awarenessThreshold, distanceToPlayerThreshold, playerGameObject);
		meleeInfo = info;

		pointToGuard = meleeInfo.PointToGuard;
		changeLookDirection = meleeInfo.ChangeLookDirection;
		lookDirections = meleeInfo.LookDirections;
		timeToChangeLookDirection = meleeInfo.TimeToChangeLookDirection;

		Root = InitBehaviourTree();
	}

	protected override Node InitBehaviourTree()
	{
		return
			new Selector(
				new List<Node>()
				{
					new Conditional(new ActionNode(MoveToPointToGuard), new ActionNode(IsFarFromGuardpoint)),
					new Conditional(new ActionNode(ChangeLookDirection), new ActionNode(ShouldChangeLookDirection)),
					new Conditional(new ActionNode(RotateTowardsPointToGuardLookDirection), new ActionNode(IsLookingInPointToGuardLookDirection)),
					new ActionNode( () => { return NodeStates.Running; } )
				}
				);
	}

	private NodeStates MoveToPointToGuard()
	{
		if(!navMeshAgent.pathPending)
		{
			if(navMeshAgent.destination != pointToGuard.position)
			{
				navMeshAgent.SetDestination(pointToGuard.position);
				navMeshAgent.isStopped = false;
				navMeshAgent.speed = walkSpeed;
				return NodeStates.Running;
			}
			else
			{
				if(navMeshAgent.remainingDistance <= minDistance)
				{
					navMeshAgent.isStopped = true;
					return NodeStates.Success;
				}
				else
				{
					return NodeStates.Running;
				}
			}
		}
		else
		{
			return NodeStates.Running;
		}
	}

	NodeStates ChangeLookDirection()
	{
		if(timer > 0.0f)
		{
			timer -= Time.deltaTime;
			return NodeStates.Running;
		}
		else
		{
			currentLookDirectionIndex++;
			if(currentLookDirectionIndex > lookDirections.Count)
			{
				currentLookDirectionIndex = 0;
			}

			Transform newTransform = pointToGuard;

			switch(lookDirections[currentLookDirectionIndex])
			{
				case LookDirections.Forward:
					RotateTowards(newTransform);
					break;
				case LookDirections.Right:
					newTransform.Rotate(newTransform.up, 90.0f);
					RotateTowards(newTransform);
					break;
				case LookDirections.Behind:
					newTransform.Rotate(newTransform.up, 180.0f);
					RotateTowards(newTransform);
					break;
				case LookDirections.Left:
					newTransform.Rotate(newTransform.up, -90.0f);
					RotateTowards(newTransform);
					break;
			}

			timer = timeToChangeLookDirection;

			return NodeStates.Running;
		}
	}

	private void RotateTowards(Transform transformToRotateTo)
	{
		if(Quaternion.Angle(transform.rotation, transformToRotateTo.rotation) > lookAngleThreshold)
		{
			float angle = Quaternion.Angle(transform.rotation, transformToRotateTo.rotation);
			Quaternion newRotation = Quaternion.identity;
			newRotation.SetLookRotation(transformToRotateTo.forward);
			newRotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 5.0f);
			transform.rotation = newRotation;
		}
	}

	NodeStates IsLookingInPointToGuardLookDirection()
	{
		//Is Agent looking in pointToGuard.forward direction? (Make sure green arrow points upwards !!!!)
		if(Quaternion.Angle(transform.rotation, pointToGuard.rotation) > lookAngleThreshold)
		{
			return NodeStates.Failure;
		}
		else
		{
			return NodeStates.Success;
		}
	}

	NodeStates RotateTowardsPointToGuardLookDirection()
	{
		if(Quaternion.Angle(transform.rotation, pointToGuard.rotation) > lookAngleThreshold)
		{
			float angle = Quaternion.Angle(transform.rotation, pointToGuard.rotation);
			Quaternion newRotation = Quaternion.identity;
			newRotation.SetLookRotation(pointToGuard.forward);
			newRotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 5.0f);
			transform.rotation = newRotation;

			return NodeStates.Running;
		}
		else
		{
			return NodeStates.Success;
		}
	}

	NodeStates IsFarFromGuardpoint()
	{
		if(Vector3.Magnitude(transform.position - pointToGuard.position) > 1.0f)
		{
			return NodeStates.Success;
		}
		else
		{
			return NodeStates.Success;
		}
	}

	NodeStates ShouldChangeLookDirection()
	{
		if(changeLookDirection)
		{
			return NodeStates.Success;
		}
		else
		{
			return NodeStates.Success;
		}
	}
}
