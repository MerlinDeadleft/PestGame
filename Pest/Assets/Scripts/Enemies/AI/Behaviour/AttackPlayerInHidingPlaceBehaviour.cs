using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ModularAI;
using ModularAI.BT;

public class AttackPlayerInHidingPlaceBehaviour : EnemyBehaviour
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

	Animator animator = null;
	bool isAttacking = false;

	public AttackPlayerInHidingPlaceBehaviour(NavMeshAgent agent, AIBrain brain, float walkingSpeed, float runningSpeed, float awarenessThreshold, float distanceToPlayerThreshold, GameObject playerGameObject)
	{
		Init(agent, brain, walkingSpeed, runningSpeed, awarenessThreshold, distanceToPlayerThreshold, playerGameObject);
		animator = navMeshAgent.GetComponent<Animator>();

		Root = InitBehaviourTree();
	}

	protected override Node InitBehaviourTree()
	{
		return
			new Sequence(
				new List<Node>()
				{
					new ActionNode(MoveToHidingPlaceKillPosition),
					new ActionNode(RotateTowardsHidingPlace),
					new ActionNode(Attack)
				}
				);
	}

	NodeStates MoveToHidingPlaceKillPosition()
	{
		HidingObjectController hidingPlace = player.GetComponent<PlayerController>().HidingObject;
		if(hidingPlace == null)
		{
			return NodeStates.Failure;
		}

		Vector3 hidingPlaceKillPosition;

		if(hidingPlace.hasFixedKillAnimationStartPoint)
		{
			hidingPlaceKillPosition = hidingPlace.KillAnimationStartPosition.position;
		}
		else
		{
			hidingPlaceKillPosition = hidingPlace.transform.position + (Vector3.Normalize(transform.position - hidingPlace.transform.position) * hidingPlace.killAnimationStartDistance);
		}

		if(!navMeshAgent.pathPending)
		{
			if((navMeshAgent.destination - hidingPlaceKillPosition).magnitude >= 0.5)
			{
				navMeshAgent.SetDestination(hidingPlaceKillPosition);
				navMeshAgent.isStopped = false;
				return NodeStates.Running;
			}
			else
			{
				if(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
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

	NodeStates RotateTowardsHidingPlace()
	{
		HidingObjectController hidingPlace = player.GetComponent<PlayerController>().HidingObject;
		if(hidingPlace == null)
		{
			return NodeStates.Failure;
		}

		//rotate towards hiding place based on HideAnimationStartPosition (Make sure green arrow points upwards !!!!)
		if(hidingPlace.hasFixedKillAnimationStartPoint && Quaternion.Angle(transform.rotation, hidingPlace.KillAnimationStartPosition.rotation) > 0.5f)
		{
			float angle = Quaternion.Angle(transform.rotation, hidingPlace.KillAnimationStartPosition.rotation);
			Quaternion newRotation = Quaternion.identity;
			newRotation.SetLookRotation(hidingPlace.KillAnimationStartPosition.forward);
			newRotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 5.0f);
			transform.rotation = newRotation;

			return NodeStates.Running;
		}
		else if(!hidingPlace.hasFixedKillAnimationStartPoint && Quaternion.Angle(transform.rotation, Quaternion.LookRotation(hidingPlace.transform.position - transform.position, Vector3.up)) > 6.0f)
		{
			float angle = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(hidingPlace.transform.position - transform.position, Vector3.up));
			Quaternion newRotation = Quaternion.identity;
			newRotation.SetLookRotation(hidingPlace.transform.position - transform.position, Vector3.up);
			newRotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 5.0f);
			transform.rotation = newRotation;

			return NodeStates.Running;
		}
		else
		{
			return NodeStates.Success;
		}
	}

	NodeStates Attack()
	{
		if(animator.GetCurrentAnimatorStateInfo(0).IsName("Hiding Place Kill"))
		{
			isAttacking = true;
			return NodeStates.Running;
		}
		else
		{
			if(isAttacking)
			{
				isAttacking = false;
				player.GetComponent<PlayerController>().Die();
				return NodeStates.Success;
			}
			else
			{
				HidingObjectController hidingPlace = player.GetComponent<PlayerController>().HidingObject;
				int hidingPlaceNumber = (int)hidingPlace.hidingObjectType;
				hidingPlace.AnimatorDie();
				animator.SetFloat(Animator.StringToHash("AttackAnimation"), hidingPlaceNumber);
				animator.SetTrigger(Animator.StringToHash("AttackHidingPlace"));
				return NodeStates.Running;
			}
		}
	}
}
