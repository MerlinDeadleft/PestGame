using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularAI
{
	[AddComponentMenu("AI/AI Brain"), RequireComponent(typeof(SphereCollider))]
	public class AIBrain : MonoBehaviour
	{
		SphereCollider aiComponentActivationTrigger = null;

		[SerializeField] GameObject player = null;
		public GameObject Player { get { return player; } }
		[SerializeField] AIPlayerInfo playerInfo = null;
		public AIPlayerInfo PlayerInfo { get { return playerInfo; } }

		[SerializeField] List<AIComponent> aiComponents = new List<AIComponent>();
		public List<AIComponent> AIComponents { get { return aiComponents; } }

		public float Awareness { get; private set; } = 0.0f;
		public Vector3 LastKnownPlayerPosition { get; private set; } = Vector3.zero;
		public bool KnowsPlayerIsHiding { get; private set; } = false;
		bool componentsRunning = false;

		// Start is called before the first frame update
		void Start()
		{
			if(player == null)
			{
				player = GameObject.FindGameObjectWithTag("Player");
			}
			playerInfo = player.GetComponentInChildren<AIPlayerInfo>();

			aiComponentActivationTrigger = GetComponent<SphereCollider>();
			aiComponentActivationTrigger.isTrigger = true;

			if(aiComponents.Count == 0)
			{
				FindAIComponents();
			}

			foreach(AIComponent component in aiComponents)
			{
				component.Player = player;
				component.PlayerInfo = player.GetComponent<AIPlayerInfo>();
			}
		}

		[ContextMenu("Find AI components")]
		void FindAIComponents()
		{
			if(aiComponents.Count > 0)
			aiComponents.Clear();
			aiComponents.AddRange(GetComponentsInChildren<AIComponent>());
		}

		[ContextMenu("Find Player GameObject")]
		public void FindPlayer()
		{
			GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

			if(playerObject == null)
			{
				Debug.Log("Could not find Player GameObject! It has to be tagged \"Player\"!");
				return;
			}

			player = playerObject;

			AIPlayerInfo tempPlayerInfo = player.GetComponent<AIPlayerInfo>();

			if(tempPlayerInfo == null)
			{
				Debug.Log("Could not find AI Player Info Component!");
				return;
			}

			playerInfo = tempPlayerInfo;
		}

		// Update is called once per frame
		void Update()
		{
			if(!componentsRunning)
			{
				return;
			}

			AIComponent componentWithHighestAwareness = null;

			//Take LastPlayerPosition and Awareness of AIComponent with highest Awareness
			//otherwise do nothing
			foreach(AIComponent component in aiComponents)
			{
				if(componentWithHighestAwareness == null)
				{
					componentWithHighestAwareness = component;
				}

				if(component.Awareness >= componentWithHighestAwareness.Awareness)
				{
					componentWithHighestAwareness = component;
					Awareness = componentWithHighestAwareness.Awareness;
					LastKnownPlayerPosition = componentWithHighestAwareness.LastPlayerPosition;
				}

				if(component is AIEyes)
				{
					KnowsPlayerIsHiding = ((AIEyes)component).KnowsPlayerisHiding;
				}
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			foreach(AIComponent aiComponent in aiComponents)
			{
				aiComponent.enabled = true;
			}

			componentsRunning = true;
		}

		private void OnTriggerExit(Collider other)
		{
			foreach(AIComponent aiComponent in aiComponents)
			{
				aiComponent.enabled = false;
			}

			componentsRunning = false;
			KnowsPlayerIsHiding = false;
		}
	}
}