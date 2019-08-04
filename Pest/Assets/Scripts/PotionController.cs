using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionController : MonoBehaviour
{
	[SerializeField] GameObject bossPrefab = null;
	[SerializeField] Transform bossSpawnPosition = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void PickUp()
	{
		Instantiate(bossPrefab, bossSpawnPosition.position, Quaternion.identity);
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
