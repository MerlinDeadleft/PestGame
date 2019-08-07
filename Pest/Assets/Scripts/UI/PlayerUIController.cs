using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
	[SerializeField] Image manaBar = null;
	[SerializeField] Image healthBar = null;

	[SerializeField] PlayerController playerController = null;

	[Space]
	[SerializeField] Image PlayerUI = null;
	[SerializeField] float hideShowTime = 1.0f;

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

		if(Input.GetKeyDown(KeyCode.M))
		{
			HidePlayerUI();
		}

		if(Input.GetKeyDown(KeyCode.N))
		{
			ShowPlayerUI();
		}
	}

	[ContextMenu("Hide UI")]
	public void HidePlayerUI()
	{
		StopCoroutine(ShowUI());
		StartCoroutine(HideUI());
	}

	[ContextMenu("Show UI")]
	public void ShowPlayerUI()
	{
		StopCoroutine(HideUI());
		StartCoroutine(ShowUI());
	}

	IEnumerator HideUI()
	{
		float timer = 0.0f;

		while(timer < hideShowTime)
		{
			Vector2 vec2 = PlayerUI.rectTransform.anchoredPosition;
			vec2.y = Mathf.Lerp(vec2.y, 245.0f, timer / hideShowTime);
			PlayerUI.rectTransform.anchoredPosition = vec2;

			timer += Time.deltaTime;
			yield return new WaitForEndOfFrame();
			continue;
		}

		PlayerUI.rectTransform.anchoredPosition = new Vector2(PlayerUI.rectTransform.anchoredPosition.x, 245.0f);
	}

	IEnumerator ShowUI()
	{
		float timer = 0.0f;

		while(timer < hideShowTime)
		{
			Vector2 vec2 = PlayerUI.rectTransform.anchoredPosition;
			vec2.y = Mathf.Lerp(vec2.y, 0.0f, timer / hideShowTime);
			PlayerUI.rectTransform.anchoredPosition = vec2;

			timer += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		PlayerUI.rectTransform.anchoredPosition = new Vector2(PlayerUI.rectTransform.anchoredPosition.x, 0.0f);
	}
}
