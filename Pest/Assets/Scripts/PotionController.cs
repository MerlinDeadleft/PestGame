using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionController : MonoBehaviour
{
	[SerializeField] GameObject bossPrefab = null;
	[SerializeField] Transform bossSpawnPosition = null;

	public void PickUp()
	{
		GameObject boss = Instantiate(bossPrefab, bossSpawnPosition.position, Quaternion.identity);
		boss.GetComponent<EnemyController>().FindPlayer();
		boss.GetComponent<ModularAI.AIBrain>().FindPlayer();
		Destroy(transform.parent.gameObject);
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			PlayerController player = other.GetComponent<PlayerController>();

			if(player != null)
			{
				player.Potion = this;
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if(other.tag == "Player")
		{
			PlayerController player = other.GetComponent<PlayerController>();

			if(player != null)
			{
				player.Potion = null;
			}
		}
	}
}
