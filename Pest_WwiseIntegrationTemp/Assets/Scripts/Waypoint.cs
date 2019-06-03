using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class Waypoint : MonoBehaviour
{
	[SerializeField] bool guardPoint = false;
	public bool GuardWaypoint { get { return guardPoint; } }

	[ConditionalField("guardPoint")] public float guardTime = 3.0f;
	public Vector3 Position { get { return transform.position; } }

	public bool WasGuarded { get; set; } = false;
}
