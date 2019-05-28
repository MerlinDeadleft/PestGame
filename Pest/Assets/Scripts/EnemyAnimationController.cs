using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
	Animator animator = null;
	/********************animation hashes**********************/
	int dieHash = Animator.StringToHash("Die");

	private void Start()
	{
		animator = GetComponent<Animator>();
	}

	public void Die()
	{
		animator.SetTrigger(dieHash);
	}
}
