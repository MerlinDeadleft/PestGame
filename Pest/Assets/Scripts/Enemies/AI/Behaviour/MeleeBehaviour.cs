using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ModularAI;
using ModularAI.BT;

public class MeleeBehaviour : EnemyBehaviour
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

	
	EnemyBehaviour defaultBehaviour = null;
	EnemyController.MeleeInfo meleeInfo = new EnemyController.MeleeInfo();

	public MeleeBehaviour(EnemyController.MeleeInfo info, NavMeshAgent agent, AIBrain brain, float walkingSpeed, float runningSpeed, float awarenessThreshold, float distanceToPlayerThreshold, GameObject playerGameObject)
	{
		Init(agent, brain, walkingSpeed, runningSpeed, awarenessThreshold, distanceToPlayerThreshold, playerGameObject);
		meleeInfo = info;
		defaultBehaviour = InitDefaultMeleeBahviour(meleeInfo.Type);
		Root = InitBehaviourTree();
	}

	protected override Node InitBehaviourTree()
	{
		return
			new Selector(
				new List<Node>()
				{
					new Conditional(
						new Selector(
							new List<Node>()
							{
								new Conditional(
									new Selector(
										new List<Node>()
										{
											new Conditional(
												new Selector(
													new List<Node>()
													{
														new Conditional(
															new Conditional(
																new AttackPlayerInHidingPlaceBehaviour(navMeshAgent, aiBrain, walkSpeed, runSpeed, minAwareness, minDistance, player).Root,
																new ActionNode(IsPlayerHiding)
																),
															new ActionNode(IsPlayerUsingHiding)
															),
														new AttackPlayerBehaviour(navMeshAgent, aiBrain, walkSpeed, runSpeed, minAwareness, minDistance, player).Root
													}
													),
												new ActionNode(IsPlayerCloserThanMinDistance)
												),
											new MoveToPlayerBehaviour(navMeshAgent, aiBrain, walkSpeed, runSpeed, minAwareness, minDistance, player).Root
										}
										),
									new ActionNode(IsAwarenessHigherThanMinAwareness)
									),
								new Conditional(
									new ActionNode(LookAtLastPlayerPos),
									new ActionNode(IsAwarenessHigherThanTwentyfivePercent)
									)
							}
							),
						new ActionNode(PlayerHasMoreThanZeroHP)
						),
					defaultBehaviour.Root
				}
				);
	}

	NodeStates IsPlayerHiding()
	{
		if(aiBrain.PlayerInfo.IsHiding)
		{
			return NodeStates.Success;
		}
		else
		{
			return NodeStates.Failure;
		}
	}

	NodeStates IsPlayerUsingHiding()
	{
		if(aiBrain.PlayerInfo.useIsHiding)
		{
			return NodeStates.Success;
		}
		else
		{
			return NodeStates.Failure;
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

	NodeStates IsAwarenessHigherThanMinAwareness()
	{
		if(aiBrain.Awareness >= minAwareness)
		{
			return NodeStates.Success;
		}
		else
		{
			return NodeStates.Failure;
		}
	}

	NodeStates IsAwarenessHigherThanTwentyfivePercent()
	{
		if(aiBrain.Awareness > 0.25f)
		{
			return NodeStates.Success;
		}
		else
		{
			return NodeStates.Failure;
		}
	}

	NodeStates LookAtLastPlayerPos()
	{
		navMeshAgent.isStopped = true;

		float angle = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(aiBrain.LastKnownPlayerPosition - transform.position, Vector3.up));
		Quaternion newRotation = Quaternion.identity;
		newRotation.SetLookRotation(aiBrain.LastKnownPlayerPosition - transform.position, Vector3.up);
		newRotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 5.0f);
		transform.rotation = newRotation;

		return NodeStates.Running;
	}

	NodeStates PlayerHasMoreThanZeroHP()
	{
		if(player.GetComponent<PlayerController>().HitPoints > 0)
		{
			return NodeStates.Success;
		}
		else
		{
			return NodeStates.Failure;
		}
	}

	float CalcDistanceToPlayerPosition()
	{
		return Vector3.Magnitude(aiBrain.LastKnownPlayerPosition - transform.position);
	}

	EnemyBehaviour InitDefaultMeleeBahviour(EnemyController.MeleeType type)
	{
		switch(type)
		{
			case EnemyController.MeleeType.None:
				Debug.LogError("No melee enemy type chosen. Enemy will have no default behaviour!");
				return null;
			case EnemyController.MeleeType.Patrol:
				return new NewPatrolBehaviour(meleeInfo, navMeshAgent, aiBrain, walkSpeed, runSpeed, minAwareness, minDistance, player);
			case EnemyController.MeleeType.Guard:
				return new GuardBehaviour(meleeInfo, navMeshAgent, aiBrain, walkSpeed, runSpeed, minAwareness, minDistance, player);
		}

		return null;
	}
}
