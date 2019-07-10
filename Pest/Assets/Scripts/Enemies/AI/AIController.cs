using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using MyBox;

[RequireComponent(typeof(SphereCollider))]
public class AIController : MonoBehaviour
{
	[SerializeField] EnemyAnimationController animController = null;
	[Header("Nav Mesh Agent")]
	[SerializeField] NavMeshAgent navMeshAgent = null;

	[SerializeField] float walkSpeed = 3.5f;
	[SerializeField] float runSpeed = 7.0f;

	SphereCollider aiComponentActivationRange = null;
	GameObject player = null;

	List<MonoBehaviour> aiComponents = new List<MonoBehaviour>();

	[Header("Vision")]
	[SerializeField] bool hasEyes = false;
	[SerializeField, ConditionalField("hasEyes")] AISight eyes = null;
	[SerializeField, ConditionalField("hasEyes")] float horFOVAngle = 110.0f;
	[SerializeField, ConditionalField("hasEyes")] float verFOVAngle = 180.0f;
	[SerializeField, ConditionalField("hasEyes")] float rangeOfVision = 15.0f;

	[Header("Proximity")]
	[SerializeField] bool hasProximitySensor = false;
	[SerializeField, ConditionalField("hasProximitySensor")] ProximitySensor proxSensor = null;
	[SerializeField, ConditionalField("hasProximitySensor")] float detectionRange = 2.0f;
	[SerializeField, ConditionalField("hasProximitySensor")] float detectionTime = 2.0f;

	[Header("Behaviour")]
	[SerializeField] bool guard = false;
	[SerializeField, ConditionalField("guard")] Transform pointToGuard = null;
	[SerializeField, ConditionalField("guard")] float maxDistanceFromGuardPoint = 10.0f;
	[SerializeField, ConditionalField("guard")] float timeToReturnToGuard = 5.0f;
	float returnToGuardTimer = 0.0f;

	[Space(20)]
	[SerializeField] bool patrol = false;
	[SerializeField, ConditionalField("patrol")] PatrolBehaviour patrolBehaviour = null;
	float timeToReturnToPatrol = 0.0f;
	float returnToPatrolTimer = 0.0f;
	bool walkToPathLeftPoint = false;
	bool isPatrolling = true;

	public bool AttackingPlayer { get; private set; } = false;
	public bool SwordActive { get; private set; } = false;
	bool attackStarted = false;
	float waitForAttackTime = 1.0f;
	float waitAfterAttackTime = 1.0f;
	float attackTimer = 0.0f;
	bool isFacingPlayer = false;

	// Start is called before the first frame update
	void Start()
	{
		navMeshAgent.speed = walkSpeed;

		aiComponentActivationRange = GetComponent<SphereCollider>();

		player = GameObject.FindGameObjectWithTag("Player");

		if(hasEyes && eyes != null)
		{
			aiComponents.Add(eyes);

			eyes.Player = player.transform;
			eyes.HorFOVAngle = horFOVAngle;
			eyes.VerFOVAngle = verFOVAngle;
			eyes.RangeOfVision = rangeOfVision;

			if(rangeOfVision > aiComponentActivationRange.radius)
			{
				aiComponentActivationRange.radius = rangeOfVision;
			}
		}

		if(hasProximitySensor && proxSensor != null)
		{
			aiComponents.Add(proxSensor);
			proxSensor.Player = player.transform;
			proxSensor.DetectionRange = detectionRange;
			proxSensor.DetectionTime = detectionTime;
		}

		returnToGuardTimer = timeToReturnToGuard;

		if(patrol)
		{
			guard = false;
		}
	}

	// Update is called once per frame
	void Update()
	{
		if(AttackingPlayer)
		{
			HandleAttacking();
		}
		else if(patrol && patrolBehaviour != null)
		{
			HandlePatrolling();
		}
		else if(guard && pointToGuard != null)
		{
			HandleGuarding();
		}
		else
		{
			HandleDefaultBehaviour();
		}

		if(!animController.CanMove)
		{
			navMeshAgent.isStopped = true;
		}
		else
		{
			navMeshAgent.isStopped = false;
		}

		if(eyes != null && eyes.PlayerInSight && proxSensor != null)
		{
			proxSensor.DetectImmediately = true;
		}

		if(proxSensor != null && proxSensor.PlayerCloseBy)
		{
			FacePlayer();
		}

		animController.Velocity = navMeshAgent.velocity.magnitude;
		SwordActive = animController.SwordActive;
	}

	void HandleAttacking()
	{
		navMeshAgent.isStopped = true;
		if(isFacingPlayer)
		{
			if(!attackStarted)
			{
				attackTimer += Time.deltaTime;

				if(attackTimer >= waitForAttackTime)
				{
					animController.Attack();
					attackStarted = true;
					attackTimer = 0.0f;
				}
			}

			if(!animController.Attacking)
			{
				attackTimer += Time.deltaTime;

				if(attackTimer >= waitAfterAttackTime)
				{
					AttackingPlayer = false;
					attackStarted = false;
					attackTimer = 0.0f;
				}
			}
		}
	}

	void HandleDefaultBehaviour()
	{
		if(eyes.PlayerInSight)
		{
			navMeshAgent.SetDestination(eyes.PlayerLastSeenPosition);
			navMeshAgent.speed = runSpeed;
		}

		if(eyes.PlayerInSight && Vector3.Magnitude(transform.position - eyes.PlayerLastSeenPosition) < navMeshAgent.radius + player.GetComponent<CharacterController>().radius + 0.5f)
		{
			AttackingPlayer = true;
		}
	}

	void HandlePatrolling()
	{
		if(guard && !eyes.PlayerInSight && isPatrolling)
		{
			returnToPatrolTimer += Time.deltaTime;

			if(returnToPatrolTimer < timeToReturnToPatrol && !eyes.PlayerInSight)
			{
				HandleGuarding(patrolBehaviour.CurrentWaypoint.transform, patrolBehaviour.MaxDistanceFromPath, patrolBehaviour.MaxTimeOffPath);
			}
			else
			{
				guard = false;
			}
		}
		else if(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && !eyes.PlayerInSight && isPatrolling)
		{
			if(patrolBehaviour.CurrentWaypoint != null && patrolBehaviour.CurrentWaypoint.GuardWaypoint && !patrolBehaviour.CurrentWaypoint.WasGuarded)
			{
				if(!guard)
				{
					timeToReturnToPatrol = patrolBehaviour.CurrentWaypoint.guardTime;
					returnToPatrolTimer = 0.0f;
				}
				patrolBehaviour.CurrentWaypoint.WasGuarded = true;
				guard = true;
			}
			else
			{
				if(patrolBehaviour.CurrentWaypoint != null)
				{
					patrolBehaviour.CurrentWaypoint.WasGuarded = false; //Next time Agent comes to this waypoint guard behaviour has to be repeated
				}
				navMeshAgent.SetDestination(patrolBehaviour.GetNextWayPoint().Position);
				navMeshAgent.speed = walkSpeed;
				//Debug.Log("126:" + navMeshAgent.destination);
			}
		}
		else if(!isPatrolling || eyes.PlayerInSight)
		{
			if(!patrolBehaviour.PathLeft)
			{
				patrolBehaviour.PathLeftPoint = transform.position;
				patrolBehaviour.PathLeft = true;
			}

			float distanceToPathLeftPoint = Vector3.Magnitude(transform.position - patrolBehaviour.PathLeftPoint);

			if(eyes.PlayerInSight)
			{
				float distanceToPlayerLastSeenPos = Vector3.Magnitude(transform.position - eyes.PlayerLastSeenPosition);

				if(distanceToPlayerLastSeenPos > navMeshAgent.radius + player.GetComponent<CharacterController>().radius + 0.5f)
				{
					navMeshAgent.SetDestination(eyes.PlayerLastSeenPosition);
					navMeshAgent.speed = runSpeed;

					//Debug.Log("146:" + navMeshAgent.destination);
					attackTimer = 0.0f;
				}
				else
				{
					AttackingPlayer = true;
				}

				returnToPatrolTimer = 0.0f;
				isPatrolling = false;
				walkToPathLeftPoint = false;
			}
			else
			{
				returnToPatrolTimer += Time.deltaTime;

				if(returnToPatrolTimer >= patrolBehaviour.MaxTimeOffPath)
				{
					if(distanceToPathLeftPoint > navMeshAgent.stoppingDistance)
					{
						if(!walkToPathLeftPoint)
						{
							navMeshAgent.SetDestination(patrolBehaviour.PathLeftPoint);
							navMeshAgent.speed = walkSpeed;
							//Debug.Log("163:" + navMeshAgent.destination);
							walkToPathLeftPoint = true;
						}
					}
					else if(patrolBehaviour.CurrentWaypoint != null)
					{
						navMeshAgent.SetDestination(patrolBehaviour.CurrentWaypoint.Position);
						navMeshAgent.speed = walkSpeed;
						//Debug.Log("170:" + navMeshAgent.destination);
						walkToPathLeftPoint = false;
						patrolBehaviour.PathLeft = false;
						isPatrolling = true;
					}
				}
			}
		}
	}

	void HandleGuarding()
	{
		float distanceToGuardPoint = Vector3.Magnitude(transform.position - pointToGuard.position);
		float distanceToPlayerLastSeenPos = Vector3.Magnitude(transform.position - eyes.PlayerLastSeenPosition);

		if(eyes.PlayerInSight && distanceToPlayerLastSeenPos < navMeshAgent.radius + player.GetComponent<CharacterController>().radius + 0.5f)
		{
			AttackingPlayer = true;
		}
		else if(eyes.PlayerInSight && distanceToGuardPoint < maxDistanceFromGuardPoint)
		{
			navMeshAgent.SetDestination(eyes.PlayerLastSeenPosition);
			navMeshAgent.speed = runSpeed;
			returnToGuardTimer = 0.0f;
		}
		else
		{
			returnToGuardTimer += Time.deltaTime;

			if(returnToGuardTimer >= timeToReturnToGuard)
			{
				if(distanceToGuardPoint > 0.3f)
				{
					navMeshAgent.SetDestination(pointToGuard.position);
					navMeshAgent.speed = walkSpeed;
				}
				else
				{
					if(Quaternion.Angle(navMeshAgent.transform.rotation, pointToGuard.rotation) > 5.0f)
					{
						Quaternion tempQuaternion = new Quaternion();
						tempQuaternion.SetLookRotation(pointToGuard.forward, Vector3.up);

						navMeshAgent.transform.rotation = Quaternion.Slerp(navMeshAgent.transform.rotation, tempQuaternion, Time.deltaTime);
					}
				}
			}
		}
	}

	void HandleGuarding(Transform guardPoint, float maxDistanceFromPoint, float returnTime)
	{
		float distanceToGuardPoint = Vector3.Magnitude(transform.position - guardPoint.position);
		float distanceToPlayerLastSeenPos = Vector3.Magnitude(transform.position - eyes.PlayerLastSeenPosition);

		if(eyes.PlayerInSight && distanceToPlayerLastSeenPos < navMeshAgent.radius + player.GetComponent<CharacterController>().radius + 0.5f)
		{
			AttackingPlayer = true;
		}
		else if(eyes.PlayerInSight && distanceToGuardPoint < maxDistanceFromPoint)
		{
			navMeshAgent.SetDestination(eyes.PlayerLastSeenPosition);
			navMeshAgent.speed = runSpeed;
			returnToGuardTimer = 0.0f;
		}
		else
		{
			returnToGuardTimer += Time.deltaTime;

			if(returnToGuardTimer >= returnTime)
			{
				if(distanceToGuardPoint > navMeshAgent.stoppingDistance)
				{
					navMeshAgent.SetDestination(guardPoint.position);
					navMeshAgent.speed = walkSpeed;
				}
				else
				{
					if(Quaternion.Angle(navMeshAgent.transform.rotation, guardPoint.rotation) > 5.0f)
					{
						Quaternion tempQuaternion = new Quaternion();
						tempQuaternion.SetLookRotation(guardPoint.forward, Vector3.up);

						navMeshAgent.transform.rotation = Quaternion.Slerp(navMeshAgent.transform.rotation, tempQuaternion, Time.deltaTime);
					}
				}
			}
		}
	}

	void FacePlayer()
	{
		if(navMeshAgent.destination != proxSensor.PlayerLastSensedPosition)
		{
			navMeshAgent.SetDestination(proxSensor.PlayerLastSensedPosition);
		}

		Vector3 enemyToPlayer = player.transform.position - transform.position;
		Quaternion lookRotation = Quaternion.LookRotation(enemyToPlayer, Vector3.up);

		if(Quaternion.Angle(navMeshAgent.transform.rotation, lookRotation) > 10.0f)
		{
			isFacingPlayer = false;
			navMeshAgent.transform.rotation = Quaternion.Slerp(navMeshAgent.transform.rotation, lookRotation, Time.deltaTime * 2.5f);
		}
		else
		{
			isFacingPlayer = true;
		}
	}

	public void Die()
	{
		foreach(MonoBehaviour aiComponent in aiComponents)
		{
			aiComponent.enabled = false;
		}

		this.enabled = false;
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject == player)
		{
			foreach(MonoBehaviour aiComponent in aiComponents)
			{
				aiComponent.enabled = true;
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if(other.gameObject == player)
		{
			foreach(MonoBehaviour aiComponent in aiComponents)
			{
				aiComponent.enabled = false;
			}
		}
	}
}
