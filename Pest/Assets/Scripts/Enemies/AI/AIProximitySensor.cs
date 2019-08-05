using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularAI
{
	[AddComponentMenu("AI/AI Proximity Sensor")]
	public class AIProximitySensor : AIComponent
	{
		//PARENTCLASS FIELDS
		/*
		public GameObject Player { get; set; } = null;

		[SerializeField, Tooltip("The farther right a key, the higher it should be. A key's value should never be lower than the value of the key to the right.")]
		protected AnimationCurve awarenessRampUpCurve = new AnimationCurve();
		public float Awareness { get; protected set; } = 0.0f;

		public Vector3 LastPlayerPosition { get; protected set; } = Vector3.zero;

		public bool DetectingPlayer { get; protected set; } = false;
		public bool DetectImmediately { get; protected set; } = false;
		public float DetectionRange { get; protected set; } = 0.0f;
		[SerializeField, Tooltip("Time unitl awareness reaches peak value")] protected float detectionTime = 0.0f;
		protected float detectionTimer = 0.0f;
		[SerializeField, Tooltip("Time unitl awareness starts to drop")] protected float detectionCoolDownTime = 0.0f;
		protected float detectionCoolDownTimer = 0.0f;

		[SerializeField] protected bool hasHeightOffset = false;
		[SerializeField, ConditionalField("hasHeightOffset")] float heightOffset = 0.0f;
		*/
		//PARENTCLASS FIELDS

		// Update is called once per frame
		void Update()
		{
			if(Player == null)
			{
				return;
			}

			RaycastHit hit;
			Ray ray = new Ray();

			if(hasHeightOffset)
			{
				//AI Agent position is offset so AI Agent is not looking from it's pivot, which could interfere with detection of player
				ray.origin = transform.position + Vector3.up * heightOffset;
			}
			else
			{
				ray.origin = transform.position;
			}

			if(playerHasHeightOffset)
			{
				//Player position is offset so AI Agent is not looking at player's pivot, which could interfere with detection of player
				ray.direction = (Player.transform.position + Vector3.up * playerHeightOffset) - ray.origin;
			}
			else
			{
				ray.direction = Player.transform.position - transform.position;
			}

			if(Physics.Raycast(ray.origin, ray.direction, out hit, detectionRange, Physics.AllLayers, QueryTriggerInteraction.Ignore))
			{
				if(hit.transform.gameObject == Player)
				{
					detectingPlayer = true;
					LastPlayerPosition = Player.transform.position;

#if UNITY_EDITOR
					if(debug)
					{
						Debug.DrawRay(ray.origin, Vector3.Normalize(ray.direction) * detectionRange, Color.green);
					}
#endif
				}
				else
				{
					detectingPlayer = false;
				}
			}
			else
			{
				detectingPlayer = false;
			}

			if(detectingPlayer)
			{
				if(detectionTimer < 0.0f)
				{
					detectionTimer = 0.0f;
				}
				detectionTimer += Time.deltaTime;
				detectionCoolDownTimer = 0.0f;
			}
			else
			{
				detectionCoolDownTimer += Time.deltaTime;

				if(detectionCoolDownTimer >= detectionCoolDownTime)
				{
					if(detectionTimer > detectionTime)
					{
						detectionTimer = detectionTime;
					}
					detectionTimer -= Time.deltaTime;
				}
			}

			UpdateAwareness();

#if UNITY_EDITOR
			if(debug)
			{
				debugAwarenessPercentage = awarenessPercentage;
				debugAwareness = Awareness;
				debugdetectionTimer = detectionTimer;
				debugdetectionCooldownTimer = detectionCoolDownTimer;
			}
#endif
		}
	}
}