using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularAI
{
	/// <summary>
	/// Class used to hold information that is used by the AI System
	/// </summary>
	[AddComponentMenu("AI/AI Player Info")]
	public class AIPlayerInfo : MonoBehaviour
	{
		public bool useVelocity = true;
		public Vector3 Velocity { get; set; } = Vector3.zero;
		public float maxVelocity { get; set; } = 0.0f;

		public bool useIsHiding = true;
		public bool IsHiding { get; set; } = false;

		public bool useIsInLight = true;
		public bool IsInLight { get; set; } = true;

		// Start is called before the first frame update
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{

		}
	}
}