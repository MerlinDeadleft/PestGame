using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
	[SerializeField] EnemyAnimationController enemyAnimController = null;

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player" && enemyAnimController.SwordActive)
		{
			other.GetComponent<PlayerController>().Die();
		}
	}
}
