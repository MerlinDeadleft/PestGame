using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBehaviour : MonoBehaviour
{
	[SerializeField] float maxDistanceFromPath = 10.0f;
	public float MaxDistanceFromPath { get { return maxDistanceFromPath; } }
	[SerializeField] float maxTimeOffPath = 5.0f;
	public float MaxTimeOffPath { get { return maxTimeOffPath; } }

	[SerializeField] List<Waypoint> waypoints = new List<Waypoint>();

	public Vector3 PathLeftPoint { get; set; } = Vector3.zero;
	public bool PathLeft { get; set; } = false;

	int currentWaypointIndex = -1;
	public Waypoint CurrentWaypoint
	{
		get
		{
			if(currentWaypointIndex >= 0)
			{
				return waypoints[currentWaypointIndex];
			}
			else
			{
				return null;
			}
		}
	}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public Waypoint GetNextWayPoint()
	{
		currentWaypointIndex++;
		if(currentWaypointIndex >= waypoints.Count)
		{
			currentWaypointIndex = 0;
		}

		return waypoints[currentWaypointIndex];
	}

	//public Vector3 GetClosestWayPointPosition(Vector3 positionToCompareTo)
	//{
	//	Waypoint closestWayPoint = null;

	//	foreach(Waypoint waypoint in waypoints)
	//	{
	//		if(closestWayPoint == null)
	//		{
	//			closestWayPoint = waypoint;
	//		}
	//		else
	//		{
	//			if(Vector3.SqrMagnitude(positionToCompareTo - waypoint.Position) < Vector3.SqrMagnitude(positionToCompareTo - closestWayPoint.Position))
	//			{
	//				closestWayPoint = waypoint;
	//			}
	//		}
	//	}

	//	return closestWayPoint.Position;
	//}

	[ContextMenu("Add Waypoint")]
	void AddWayPoint()
	{
		GameObject waypoint = new GameObject("Waypoint " + waypoints.Count);
		waypoint.transform.parent = gameObject.transform;
		waypoint.transform.localPosition = Vector3.zero;
		waypoint.AddComponent<Waypoint>();
		waypoints.Add(waypoint.GetComponent<Waypoint>());
	}
}
