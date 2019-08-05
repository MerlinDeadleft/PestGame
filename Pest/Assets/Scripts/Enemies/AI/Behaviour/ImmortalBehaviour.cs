using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ModularAI;
using ModularAI.BT;

public class ImmortalBehaviour : EnemyBehaviour
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

	public ImmortalBehaviour(NavMeshAgent agent, AIBrain brain, float walkingSpeed, float runningSpeed, float awarenessThreshold, float distanceToPlayerThreshold, GameObject playerGameObject)
	{
		Init(agent, brain, walkingSpeed, runningSpeed, awarenessThreshold, distanceToPlayerThreshold, playerGameObject);
		Root = InitBehaviourTree();
	}

	protected override Node InitBehaviourTree()
	{
		return
			new Selector(
				new List<Node>()
				{
					new Conditional(
						new ActionNode(Idle),
						new ActionNode(IsPlayerDead)
						),
					new Conditional(
						new AttackPlayerBehaviour(navMeshAgent, aiBrain, walkSpeed, runSpeed, minAwareness, minDistance, player).Root,
						new ActionNode(IsPlayerCloserThanMinDistance)
						),
					new ActionNode(ChasePlayer)
				}
				);
	}

	NodeStates Idle()
	{
		navMeshAgent.isStopped = true;
		return NodeStates.Success;
	}

	NodeStates IsPlayerDead()
	{
		if(player.GetComponent<PlayerController>().HitPoints > 0)
		{
			return NodeStates.Failure;
		}
		else
		{
			return NodeStates.Success;
		}
	}

	NodeStates IsPlayerCloserThanMinDistance()
	{
		if(CalcDistanceToPlayerPosition() < minDistance)
		{
			return NodeStates.Success;
		}
		else
		{
			return NodeStates.Failure;
		}
	}

	NodeStates ChasePlayer()
	{
		if(!navMeshAgent.pathPending)
		{
			if(navMeshAgent.destination != player.transform.position)
			{
				navMeshAgent.SetDestination(player.transform.position);
				navMeshAgent.isStopped = false;
				navMeshAgent.speed = runSpeed;
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

	float CalcDistanceToPlayerPosition()
	{
		return Vector3.Magnitude(player.transform.position - transform.position);
	}
}
