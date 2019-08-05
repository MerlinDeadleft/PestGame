using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ModularAI;
using ModularAI.BT;

public class MoveToPlayerBehaviour : EnemyBehaviour
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


	public MoveToPlayerBehaviour(NavMeshAgent agent, AIBrain brain, float walkingSpeed, float runningSpeed, float awarenessThreshold, float distanceToPlayerThreshold, GameObject playerGameObject)
	{
		Init(agent, brain, walkingSpeed, runningSpeed, awarenessThreshold, distanceToPlayerThreshold, playerGameObject);

		Root = InitBehaviourTree();
	}

	protected override Node InitBehaviourTree()
	{
		return new ActionNode(new ActionNode.ActionNodeDelegate(MoveToPlayer));
	}

	NodeStates MoveToPlayer()
	{
		if(!navMeshAgent.pathPending)
		{
			if(navMeshAgent.destination != aiBrain.LastKnownPlayerPosition)
			{
				navMeshAgent.SetDestination(aiBrain.LastKnownPlayerPosition);
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
}
