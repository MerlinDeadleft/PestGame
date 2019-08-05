using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ModularAI;
using ModularAI.BT;

public class AttackPlayerBehaviour : EnemyBehaviour
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

	public AttackPlayerBehaviour(NavMeshAgent agent, AIBrain brain, float walkingSpeed, float runningSpeed, float awarenessThreshold, float distanceToPlayerThreshold, GameObject playerGameObject)
	{
		Init(agent, brain, walkingSpeed, runningSpeed, awarenessThreshold, distanceToPlayerThreshold, playerGameObject);
		animator = navMeshAgent.GetComponent<Animator>();

		Root = InitBehaviourTree();
	}

	protected override Node InitBehaviourTree()
	{
		return new ActionNode(Attack);
	}

	NodeStates Attack()
	{
		if(animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
		{
			isAttacking = true;
			return NodeStates.Running;
		}
		else
		{
			if(isAttacking)
			{
				isAttacking = false;
				return NodeStates.Success;
			}
			else
			{
				animator.SetTrigger(Animator.StringToHash("Attack"));
				return NodeStates.Running;
			}
		}
	}
}
