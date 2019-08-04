using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

namespace ModularAI
{
	[AddComponentMenu("AI/AI Component")]
	public abstract class AIComponent : MonoBehaviour
	{
		public GameObject Player { get; set; } = null;
		public AIPlayerInfo PlayerInfo { get; set; } = null;

		[SerializeField, Tooltip("The farther right a key, the higher it should be. A key's value should never be lower than the value of the key to the right.")]
		protected AnimationCurve awarenessRampUpCurve = new AnimationCurve();
		public float Awareness { get; protected set; } = 0.0f;
		protected float awarenessPercentage = 0.0f;

		public Vector3 LastPlayerPosition { get; protected set; } = Vector3.zero;

		protected bool detectingPlayer = false;
		[SerializeField] protected float detectionRange = 0.0f;
		[SerializeField, Min(0.001f), Tooltip("Time unitl awareness reaches peak value")] protected float detectionTime = 0.0001f;
		protected float detectionTimer = 0.0f;
		[SerializeField, Min(0.0001f), Tooltip("Time unitl awareness starts to drop")] protected float detectionCoolDownTime = 0.0001f;
		protected float detectionCoolDownTimer = 0.0f;

		[SerializeField] protected bool hasHeightOffset = false;
		[SerializeField, ConditionalField("hasHeightOffset")] protected float heightOffset = 0.0f;

		[SerializeField] protected bool playerHasHeightOffset = false;
		[SerializeField, ConditionalField("playerHasHeightOffset")] protected float playerHeightOffset = 0.0f;

#if UNITY_EDITOR
		[SerializeField] protected bool debug = false;
		[SerializeField, ConditionalField("debug")] protected float debugAwarenessPercentage = 0.0f;
		[SerializeField, ConditionalField("debug")] protected float debugAwareness = 0.0f;
		[SerializeField, ConditionalField("debug")] protected float debugdetectionTimer = 0.0f;
		[SerializeField, ConditionalField("debug")] protected float debugdetectionCooldownTimer = 0.0f;
#endif

		// Start is called before the first frame update
		protected void Start()
		{
			//playerInfo = Player.GetComponent<AIPlayerInfo>();
		}

		protected void UpdateAwareness()
		{
			awarenessPercentage = Mathf.Clamp01(detectionTimer / detectionTime);
			Awareness = awarenessRampUpCurve.Evaluate(awarenessPercentage);
		}

		protected void OnDisable()
		{
			detectingPlayer = false;
			detectionTimer = 0.0f;
			detectionCoolDownTimer = 0.0f;
			Awareness = 0.0f;
		}
	}
}