using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ModularAI;
using ModularAI.BT;
using System;

public class NewPatrolBehaviour : EnemyBehaviour
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

	EnemyController.MeleeInfo meleeInfo = new EnemyController.MeleeInfo();
	PatrolBehaviour patrolBehaviour = null;
	bool arrivedAtWaypoint = false;
	float timer = 0.0f;

	public NewPatrolBehaviour(
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
		patrolBehaviour = meleeInfo.PatrolBehaviour;

		Root = InitBehaviourTree();
	}

	protected override Node InitBehaviourTree()
	{
		Node behaviourTree =
			new Selector(
				new List<Node>()
				{
					new Conditional(
						new Selector(
							new List<Node>()
							{
								new Conditional(
									new ActionNode(MoveToWayPoint),
									new ActionNode(IsNotArrivedAtWaypoint)
									),
								new Conditional(
									new Sequence(
										new List<Node>()
										{
											new ActionNode(RotateTowardsGuardpoint),
											new ActionNode(GuardWaypoint)
										}
										),
									new ActionNode(ShouldGuardWaypoint)
									),
								new ActionNode(GetNextWaypoint)
							}
							),
						new ActionNode(IsDestinationCurrentWayPoint)
						),
					new ActionNode(SetCurrentWaypointAsDestination),
					new ActionNode(GetNextWaypoint)
				}
			);

		return behaviourTree;
	}

	NodeStates MoveToWayPoint()
	{
		if(navMeshAgent.pathPending)
		{
			arrivedAtWaypoint = false;
			return NodeStates.Running;
		}
		else
		{
			if(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
			{
				navMeshAgent.isStopped = true;
				arrivedAtWaypoint = true;
				return NodeStates.Success;
			}
			else
			{
				return NodeStates.Running;
			}
		}
	}

	NodeStates RotateTowardsGuardpoint()
	{
		if(Quaternion.Angle(transform.rotation, patrolBehaviour.CurrentWaypoint.Rotation) > 0.5f)
		{
			float angle = Quaternion.Angle(transform.rotation, patrolBehaviour.CurrentWaypoint.Rotation);
			Quaternion newRotation = Quaternion.identity;
			newRotation.SetLookRotation(patrolBehaviour.CurrentWaypoint.transform.forward);
			newRotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 5.0f);
			transform.rotation = newRotation;

			timer = patrolBehaviour.CurrentWaypoint.guardTime;
			return NodeStates.Running;
		}
		else
		{
			return NodeStates.Success;
		}
	}

	NodeStates GuardWaypoint()
	{
		timer -= Time.deltaTime;

		if(timer <= 0.0f)
		{
			return NodeStates.Failure; //Return Failure on purpose, so that next node in Selector is invoked
		}
		else
		{
			return NodeStates.Running;
		}
	}

	NodeStates GetNextWaypoint()
	{
		patrolBehaviour.GetNextWayPoint();
		return NodeStates.Success;
	}

	NodeStates IsDestinationCurrentWayPoint()
	{
		if(patrolBehaviour.CurrentWaypoint == null)
		{
			return NodeStates.Failure;
		}

		float mag = (navMeshAgent.destination - patrolBehaviour.CurrentWaypoint.Position).magnitude;

		if((navMeshAgent.destination - patrolBehaviour.CurrentWaypoint.Position).magnitude <= 0.5)
		{
			return NodeStates.Success;
		}
		else
		{
			return NodeStates.Failure;
		}
	}

	NodeStates SetCurrentWaypointAsDestination()
	{
		Waypoint currentWaypoint = patrolBehaviour.CurrentWaypoint;

		if(currentWaypoint == null)
		{
			return NodeStates.Failure;
		}
		else
		{
			navMeshAgent.SetDestination(currentWaypoint.Position);
			navMeshAgent.isStopped = false;
			navMeshAgent.speed = walkSpeed;
			arrivedAtWaypoint = false;
			return NodeStates.Success;
		}
	}

	NodeStates IsNotArrivedAtWaypoint()
	{
		if(!arrivedAtWaypoint)
		{
			return NodeStates.Success;
		}
		else
		{
			return NodeStates.Failure;
		}
	}

	NodeStates ShouldGuardWaypoint()
	{
		if(patrolBehaviour.CurrentWaypoint.GuardWaypoint)
		{
			return NodeStates.Success;
		}
		else
		{
			return NodeStates.Failure;
		}
	}
}
