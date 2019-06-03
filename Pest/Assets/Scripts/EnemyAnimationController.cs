using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
	Animator animator = null;

	public bool Attacking { get; set; } = false;

	/********************animation hashes**********************/
	int dieHash = Animator.StringToHash("Die");
	int attackHash = Animator.StringToHash("Attack");

	private void Start()
	{
		animator = GetComponent<Animator>();
	}

	public void Die()
	{
		animator.SetTrigger(dieHash);
	}

	public void Attack()
	{
		animator.SetTrigger(attackHash);
		Attacking = true;
	}

	void OnAttackEnd()
	{
		Attacking = false;
	}
}
