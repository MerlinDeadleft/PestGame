using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ModularAI;
using ModularAI.BT;

public abstract class EnemyBehaviour
{
	protected Transform transform = null;
	protected NavMeshAgent navMeshAgent = null;
	protected AIBrain aiBrain = null;

	protected float walkSpeed = 1.5f;
	protected float runSpeed = 4.5f;
	protected float minAwareness = 1.0f;
	protected float minDistance = 0.0f;

	protected GameObject player = null;

	public Node Root { get; protected set; } = null;

	protected void Init(NavMeshAgent agent, AIBrain brain, float walkingSpeed, float runningSpeed, float awarenessThreshold, float distanceToPlayerThreshold, GameObject playerGameObject)
	{
		navMeshAgent = agent;
		aiBrain = brain;
		walkSpeed = walkingSpeed;
		runSpeed = runningSpeed;
		minAwareness = awarenessThreshold;
		minDistance = distanceToPlayerThreshold;
		player = playerGameObject;
		transform = agent.transform;
	}

	protected abstract Node InitBehaviourTree();
}
