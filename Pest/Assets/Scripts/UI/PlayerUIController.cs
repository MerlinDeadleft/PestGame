using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
	[SerializeField] Image manaBar = null;
	[SerializeField] Image healthBar = null;

	[SerializeField] PlayerController playerController = null;


	// Start is called before the first frame update
	void Start()
    {
		manaBar.fillAmount = 1.0f;
		healthBar.fillAmount = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
		if(playerController.HitPoints == 3)
		{
			healthBar.fillAmount = 1.0f;
		}
		else if(playerController.HitPoints == 2)
		{
			healthBar.fillAmount = 0.5f;
		}
		else if(playerController.HitPoints == 1)
		{
			healthBar.fillAmount = 0.333f;
		}
		else if(playerController.HitPoints == 0)
		{
			healthBar.fillAmount = 0.0f;
		}

		manaBar.fillAmount = Mathf.Lerp(manaBar.fillAmount, playerController.Mana / playerController.MaxMana, Time.deltaTime * 5.0f);
    }
}
