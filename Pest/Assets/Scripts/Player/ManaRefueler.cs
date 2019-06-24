using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		MagicController magicController = other.GetComponent<MagicController>();

		if(magicController.Mana < magicController.MaxMana)
		{
			if(limitedSupply && manaAmount <= 0.0f)
			{
				return;
			}
			else if(limitedSupply && manaAmount > 0.0f)
			{
				manaAmount -= regenerationRate * Time.deltaTime;
			}

			magicController.Mana += regenerationRate * Time.deltaTime;
		}

		if(limitedSupply && magicController.Mana > magicController.MaxMana)
		{
			manaAmount += magicController.Mana - magicController.MaxMana;
			magicController.Mana = magicController.MaxMana;
		}
	}
}
