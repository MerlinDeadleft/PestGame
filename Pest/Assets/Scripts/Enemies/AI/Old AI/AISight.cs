using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISight : MonoBehaviour
{
	public float HorFOVAngle { get; set; } = 0.0f;
	public float VerFOVAngle { get; set; } = 0.0f;
	public float RangeOfVision { get; set; } = 0.0f;

	public Transform Player { get; set; } = null;
	public bool PlayerInSight { get; private set; } = false;
	public Vector3 PlayerLastSeenPosition { get; set; } = Vector3.zero;

    void OnDisable()
    {
		PlayerInSight = false;
    }

    // Update is called once per frame
    void Update()
	{
		RaycastHit hit;
		Ray ray = new Ray(transform.position, (Player.position + new Vector3(0.0f, 1.0f, 0.0f)) - transform.position);
																//offset player position, so AI agent is not looking at player's feet
		if(Physics.Raycast(ray, out hit, RangeOfVision, Physics.AllLayers, QueryTriggerInteraction.Ignore))
		{
			if(hit.transform == Player)
			{
				//Project Ray on local ZY-plane to check vertical FOV
				Vector3 projectedRay = Vector3.ProjectOnPlane(ray.direction, transform.right);

				if(Vector3.Angle(transform.forward, projectedRay) <= VerFOVAngle * 0.5f) //FOVAngle * 0.5 because angle is from middle of FOV
				{
					//Project Ray on local XZ-plane to check horizontal FOV
					projectedRay = Vector3.ProjectOnPlane(ray.direction, transform.up);

					if(Vector3.Angle(transform.forward, projectedRay) <= HorFOVAngle * 0.5f) //FOVAngle * 0.5 because angle is from middle of FOV
					{
						Debug.DrawRay(ray.origin, ray.direction * RangeOfVision, Color.green);
						//Debug.Log("player in sight");
						PlayerInSight = true;
						PlayerLastSeenPosition = Player.position;
					}
					else
					{
						PlayerInSight = false;
					}
				}
				else
				{
					PlayerInSight = false;
				}
			}
			else
			{
				PlayerInSight = false;
			}
		}
	}
}
