using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximitySensor : MonoBehaviour
{
	public Transform Player { get; set; } = null;
	public bool PlayerCloseBy { get; private set; } = false;
	public Vector3 PlayerLastSensedPosition { get; set; } = Vector3.zero;

	public float DetectionRange { get; set; } = 0.0f;
	public bool DetectImmediately { get; set; } = false;
	float coolDownTimer = 0.0f;
	float coolDownTime = 3.0f;
	float detectionTimer = 0.0f;
	public float DetectionTime { get; set; } = 0.0f;

	void OnDisable()
	{
		PlayerCloseBy = false;
	}

	// Update is called once per frame
	void Update()
	{
		if(Player == null)
		{
			return;
		}

		RaycastHit hit;
		Ray ray = new Ray(transform.position + Vector3.up * 0.15f, (Player.position + new Vector3(0.0f, 1.0f, 0.0f)) - transform.position);
		//offset player position, so AI agent is not looking at player's feet
		if(Physics.Raycast(ray, out hit, DetectionRange, Physics.AllLayers, QueryTriggerInteraction.Ignore))
		{
			if(hit.transform == Player)
			{
				Debug.DrawRay(ray.origin, ray.direction.normalized * DetectionRange, Color.green);
				if(DetectImmediately)
				{
					PlayerCloseBy = true;
					PlayerLastSensedPosition = hit.transform.position;
				}
				else
				{
					detectionTimer += Time.deltaTime;
					if(detectionTimer >= DetectionTime)
					{
						DetectImmediately = true;
						PlayerCloseBy = true;
						PlayerLastSensedPosition = hit.transform.position;
					}
				}
			}
			else
			{
				PlayerCloseBy = false;
				detectionTimer = 0.0f;
			}
		}

		if(DetectImmediately)
		{
			coolDownTimer += Time.deltaTime;

			if(coolDownTimer >= coolDownTime)
			{
				DetectImmediately = false;
				coolDownTimer = 0.0f;
			}
		}
	}
}
