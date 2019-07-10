using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
	[SerializeField] AIController ai = null;

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player" && ai.SwordActive)
		{
			other.GetComponent<PlayerController>().Die();
		}
	}
}
