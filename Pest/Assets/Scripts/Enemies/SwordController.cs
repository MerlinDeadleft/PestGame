using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
	[SerializeField] EnemyAnimationController enemyAnimController = null;
	[SerializeField] bool oneHitKills = false;

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player" && enemyAnimController.SwordActive)
		{
			if(oneHitKills)
			{
				other.GetComponent<PlayerController>().Die();
			}
			else
			{
				other.GetComponent<PlayerController>().DamagePlayer();
			}
		}
	}
}
