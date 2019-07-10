using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
	Animator animator = null;

	public bool Attacking { get; set; } = false;
	public float Velocity { get; set; } = 0.0f;
	public bool CanMove { get; private set; } = true;
	public bool SwordActive { get; private set; } = false;

	/********************animation hashes**********************/
	int dieHash = Animator.StringToHash("Die");
	int attackHash = Animator.StringToHash("Attack");
	int velocityHash = Animator.StringToHash("Velocity");
	int canMoveHash = Animator.StringToHash("CanMove");

	private void Start()
	{
		animator = GetComponent<Animator>();
	}

	private void Update()
	{
		animator.SetFloat(velocityHash, Velocity);
		CanMove = animator.GetBool(canMoveHash);
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

	public void SetSwordActive(int isActive)
	{
		if(isActive != 0)
		{
			SwordActive = true;
		}
		else
		{
			SwordActive = false;
		}
	}
}
