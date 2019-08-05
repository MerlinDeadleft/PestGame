using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ModularAI;
using MyBox;

public class EnemyController : MonoBehaviour
{
	public enum EnemyType { None, Melee, Ranged, Immortal }
	public enum MeleeType { None, Patrol, Guard }

	public struct MeleeInfo
	{
		public MeleeType Type { get; set; }

		//MeleeType.Guard
		public Transform PointToGuard { get; set; }
		public bool ChangeLookDirection { get; set; }
		public List<GuardBehaviour.LookDirections> LookDirections { get; set; }
		public float TimeToChangeLookDirection { get; set; }

		//MeleeType.Patrol
		public PatrolBehaviour PatrolBehaviour { get; set; }
	}

	[SerializeField] EnemyType type = EnemyType.None;
	[SerializeField, ConditionalField("type", EnemyType.Melee)] MeleeType meleeType = MeleeType.None;
	[SerializeField, ConditionalField("meleeType", MeleeType.Patrol)] PatrolBehaviour patrolBehaviour = null;
	[SerializeField, ConditionalField("meleeType", MeleeType.Guard)] Transform pointToGuard = null;
	[SerializeField, ConditionalField("meleeType", MeleeType.Guard)] bool changeLookDirection = false;
	[SerializeField, Tooltip("Keep list empty if Meleetype is not Guard")] List<GuardBehaviour.LookDirections> lookDirections = null;
	[SerializeField, ConditionalField("changeLookDirection")] float timeToChangeLookDirection = 1.0f;


	[Space(30)]
	[SerializeField] NavMeshAgent navMeshAgent = null;
	[SerializeField] AIBrain aiBrain = null;

	[Space(30)]
	[SerializeField] float walkSpeed = 1.5f;
	[SerializeField] float runSpeed = 4.5f;
	[SerializeField] float minAwareness = 1.0f;
	[SerializeField] float minDistance = 4.0f;

	[Space(30)]
	[SerializeField] GameObject player = null;
	[SerializeField] EnemyAnimationController animController = null;

	EnemyBehaviour behaviour = null;

	// Start is called before the first frame update
	void Start()
	{
		if(player == null)
		{
			FindPlayer();
		}

		switch(type)
		{
			case EnemyType.None:
				Debug.Log("No Enemytype chosen. Enemy will not execute any behaviour!");
				break;
			case EnemyType.Melee:
				MeleeInfo meleeInfo = new MeleeInfo()
				{
					Type = meleeType,
					PointToGuard = pointToGuard,
					ChangeLookDirection = changeLookDirection,
					LookDirections = lookDirections,
					TimeToChangeLookDirection = timeToChangeLookDirection,
					PatrolBehaviour = patrolBehaviour
				};

				behaviour = new MeleeBehaviour(meleeInfo, navMeshAgent, aiBrain, walkSpeed, runSpeed, minAwareness, minDistance, player);
				break;
			case EnemyType.Ranged:
				Debug.LogError("No ranged behaviour implemented!");
				//behaviour = new RangedBehaviour(navMeshAgent, aiBrain, walkSpeed, runSpeed, minAwareness, minDistance, player);
				break;
			case EnemyType.Immortal:
				behaviour = new ImmortalBehaviour(navMeshAgent, aiBrain, walkSpeed, runSpeed, minAwareness, minDistance, player);
				break;
		}
	}

	// Update is called once per frame
	void Update()
	{
		if(behaviour != null)
		{
			behaviour.Root.Evaluate();
		}

		animController.Velocity = navMeshAgent.velocity.magnitude;
	}

	public void Die()
	{
		foreach(AIComponent aiComponent in aiBrain.AIComponents)
		{
			aiComponent.enabled = false;
		}

		this.enabled = false;
	}

	private void OnValidate()
	{
		if(meleeType == MeleeType.Patrol || meleeType == MeleeType.None)
		{
			changeLookDirection = false;
			pointToGuard = null;
			if(lookDirections != null)
			{
				lookDirections.Clear();
			}
		}

		if(meleeType == MeleeType.Guard || meleeType == MeleeType.None)
		{
			patrolBehaviour = null;
		}

		if(minAwareness < 0.0f)
		{
			minAwareness = 0.0f;
		}

		if(minAwareness > 1.0f)
		{
			minAwareness = 1.0f;
		}

		if(minDistance < 0.0f)
		{
			minDistance = 0.0f;
		}
	}

	[ContextMenu("Find Melee Patrol Behaviour")]
	void FindMeleePatrolBehaviour()
	{
		if(type != EnemyType.Melee)
		{
			Debug.Log("Type must be set to \"Melee\"!");
			return;
		}

		if(meleeType != MeleeType.Patrol)
		{
			Debug.Log("Melee Type must be set to \"Patrol\"!");
			return;
		}

		AIController controller = GetComponentInChildren<AIController>();

		if(controller == null)
		{
			Debug.Log("Could not find AI Controller Component in GameObject's Hierarchy!");
			return;
		}

		PatrolBehaviour patrol = controller.GetPatrolBehaviour();

		if(patrol == null)
		{
			Debug.Log("There is no Patrol Behaviour on AI Controller Component!");
			return;
		}

		patrolBehaviour = patrol;
	}

	[ContextMenu("Find AI Brain Component")]
	void FindAIBrain()
	{
		AIBrain brain = GetComponentInChildren<AIBrain>();

		if(brain == null)
		{
			Debug.Log("Could not find AI Brain Component in GameObject's Hierarchy!");
			return;
		}

		aiBrain = brain;
	}

	[ContextMenu("Find Player Game Object")]
	public void FindPlayer()
	{
		GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

		if(playerObject == null)
		{
			Debug.Log("Could not find Player GameObject! It has to be tagged \"Player\"!");
			return;
		}

		player = playerObject;
	}
}
