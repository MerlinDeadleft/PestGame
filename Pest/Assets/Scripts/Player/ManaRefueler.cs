using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Talis' code
public class ManaRefueler : MonoBehaviour
{
	[SerializeField, Tooltip("How much Mana/Sec is Regenerated")] float regenerationRate = 0.0f;
	[SerializeField, Tooltip("Has this Manasource a limited Supply?")] bool limitedSupply = false;
	[SerializeField, MyBox.ConditionalField("limitedSupply")] float manaAmount = 50.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerStay(Collider other)
	{
		if(other.tag != "Player")
		{
			return;
		}
		PlayerController player = other.GetComponent<PlayerController>();

		if(player.Mana < player.MaxMana)
		{
			if(limitedSupply && manaAmount <= 0.0f)
			{
				return;
			}
			else if(limitedSupply && manaAmount > 0.0f)
			{
				manaAmount -= regenerationRate * Time.deltaTime;
			}

			player.Mana += regenerationRate * Time.deltaTime;
		}

		if(limitedSupply && player.Mana > player.MaxMana)
		{
			manaAmount += player.Mana - player.MaxMana;
			player.Mana = player.MaxMana;
		}
	}
}
//Talis' code ende
