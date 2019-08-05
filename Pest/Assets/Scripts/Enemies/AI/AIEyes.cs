using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

namespace ModularAI
{
	[AddComponentMenu("AI/AI Eyes")]
	public class AIEyes : AIComponent
	{
		//PARENTCLASS FIELDS
		/*
		public GameObject Player { get; set; } = null;
		protected AIPlayerInfo playerInfo = null;


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

		[SerializeField] float horizontalFOVAngle = 0.0f;
		[SerializeField] float verticalFOVAngle = 0.0f;

		[SerializeField] bool hasPrimaryAndPeripheralVision = true;
		[SerializeField, ConditionalField("hasPrimaryAndPeripheralVision")] float primaryHorizontalFOVAngle = 0.0f;
		[SerializeField, ConditionalField("hasPrimaryAndPeripheralVision")] float primaryVerticalFOVAngle = 0.0f;

		bool isInPrimaryHorizontalViewfield = false;
		bool isInPrimaryVerticalViewfield = false;
		bool isInPrimaryViewfield = false;

		[SerializeField, Tooltip("If the player gets closer, awareness rises faster")] bool useDistanceModificator = true;
		[SerializeField, ConditionalField("useDistanceModificator")] AnimationCurve distanceModificatorCurve = new AnimationCurve();

		bool lastPlayerIsHiding = false;
		public bool KnowsPlayerisHiding { get; private set; } = false;

		// Update is called once per frame
		void Update()
		{
			if(Player == null)
			{
				return;
			}

			if(PlayerInfo.useIsHiding)
			{
				if(!PlayerInfo.IsHiding)
				{
					KnowsPlayerisHiding = false;
				}
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
				ray.direction = Player.transform.position + Vector3.up * playerHeightOffset - transform.position;
			}
			else
			{
				ray.direction = Player.transform.position - transform.position;
			}

			if(Physics.Raycast(ray.origin, ray.direction, out hit, detectionRange, Physics.AllLayers, QueryTriggerInteraction.Ignore))
			{
				if(hit.transform.gameObject == Player)
				{
					//Project Ray on local ZY-plane to check vertical FOV
					Vector3 projectedRay = Vector3.ProjectOnPlane(ray.direction, transform.right);

					if(Vector3.Angle(transform.forward, projectedRay) <= verticalFOVAngle * 0.5f) //FOVAngle * 0.5 because angle is from middle of FOV
					{
						if(hasPrimaryAndPeripheralVision)
						{
							if(Vector3.Angle(transform.forward, projectedRay) <= primaryVerticalFOVAngle * 0.5f)
							{
								isInPrimaryVerticalViewfield = true;
							}
							else
							{
								isInPrimaryVerticalViewfield = false;
							}
						}

						//Project Ray on local XZ-plane to check horizontal FOV
						projectedRay = Vector3.ProjectOnPlane(ray.direction, transform.up);

						if(Vector3.Angle(transform.forward, projectedRay) <= horizontalFOVAngle * 0.5f) //FOVAngle * 0.5 because angle is from middle of FOV
						{
							if(hasPrimaryAndPeripheralVision)
							{
								if(Vector3.Angle(transform.forward, projectedRay) <= primaryHorizontalFOVAngle * 0.5f)
								{
									isInPrimaryHorizontalViewfield = true;
								}
								else
								{
									isInPrimaryHorizontalViewfield = false;
								}
							}

#if UNITY_EDITOR
							if(debug)
							{
								Debug.DrawRay(ray.origin, ray.direction * detectionRange, Color.green);
							}
#endif
							detectingPlayer = true;
							LastPlayerPosition = Player.transform.position;
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


			if(PlayerInfo.useIsHiding)
			{
				if(PlayerInfo.IsHiding == true && lastPlayerIsHiding == true && !KnowsPlayerisHiding)
				{
					detectingPlayer = false;
				}

				if(detectingPlayer)
				{
					if(PlayerInfo.IsHiding == true && lastPlayerIsHiding == false) //if AI Agent sees player going into hinding, it knows player is hiding
					{
						KnowsPlayerisHiding = true;
					}
				}

				if(KnowsPlayerisHiding)
				{
					detectingPlayer = true;
				}
			}


			if(detectingPlayer)
			{
				float detectionModificator = 1.0f;

				if(useDistanceModificator)
				{
					detectionModificator = distanceModificatorCurve.Evaluate(ray.direction.magnitude / detectionRange);
					//ray is used for the raycast farther up and points to the player
					//distanceModificatorCurve has to fall off
					//(the closer the player the lower ray.direction.magnitude is => result of division goes to zero the closer the player is)
				}

				if(hasPrimaryAndPeripheralVision)
				{
					if(isInPrimaryHorizontalViewfield && isInPrimaryVerticalViewfield)
					{
						isInPrimaryViewfield = true;
					}
					else
					{
						isInPrimaryViewfield = false;
					}

					if(!isInPrimaryViewfield)
					{
						detectionModificator -= 0.25f;
					}
				}

				if(PlayerInfo.useIsInLight && PlayerInfo.IsInLight)
				{
					detectionModificator += 0.25f;
				}

				if(PlayerInfo.useVelocity)
				{
					detectionModificator += (PlayerInfo.Velocity.magnitude / PlayerInfo.maxVelocity) * 0.5f; //reduce maximum effect of velocity to 0.5f;
				}

				if(detectionTimer < 0.0f)
				{
					detectionTimer = 0.0f;
				}
				detectionTimer += Time.deltaTime * detectionModificator;
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

			lastPlayerIsHiding = PlayerInfo.IsHiding;

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