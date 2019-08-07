using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTextController : MonoBehaviour
{
	[SerializeField] GameObject player = null;

	[SerializeField] float hideShowTime = 1.0f;
	[SerializeField] Image blackBarTop = null;
	[SerializeField] Image blackBarBottom = null;
	[SerializeField] Text tutorialText = null;
	public Text TutorialText { get { return tutorialText; } set { tutorialText = value; } }

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.L))
		{
			player.GetComponentInChildren<PlayerUIController>().HidePlayerUI();
			StartCoroutine(Show());
		}

		if(Input.GetKeyDown(KeyCode.K))
		{
			player.GetComponentInChildren<PlayerUIController>().ShowPlayerUI();
			StartCoroutine(Hide());
		}
	}

	private void OnEnable()
	{
		player.GetComponentInChildren<PlayerUIController>().HidePlayerUI();
		StartCoroutine(Show());
	}

	private void OnDisable()
	{
		player.GetComponentInChildren<PlayerUIController>().ShowPlayerUI();
		StartCoroutine(Hide());

	}

	IEnumerator Show()
	{
		float timer = 0.0f;

		while(timer < hideShowTime)
		{
			Vector2 vec2 = blackBarTop.rectTransform.anchoredPosition;
			vec2.y = Mathf.Lerp(vec2.y, -65.0f, timer / hideShowTime);
			blackBarTop.rectTransform.anchoredPosition = vec2;

			vec2 = blackBarBottom.rectTransform.anchoredPosition;
			vec2.y = Mathf.Lerp(vec2.y, 65.0f, timer / hideShowTime);
			blackBarBottom.rectTransform.anchoredPosition = vec2;

			timer += Time.deltaTime;
			yield return new WaitForEndOfFrame();
			continue;
		}

		blackBarTop.rectTransform.anchoredPosition = new Vector2(blackBarTop.rectTransform.anchoredPosition.x, -65.0f);
		blackBarBottom.rectTransform.anchoredPosition = new Vector2(blackBarBottom.rectTransform.anchoredPosition.x, 65.0f);
	}

	IEnumerator Hide()
	{
		float timer = 0.0f;

		while(timer < hideShowTime)
		{
			Vector2 vec2 = blackBarTop.rectTransform.anchoredPosition;
			vec2.y = Mathf.Lerp(vec2.y, 65.0f, timer / hideShowTime);
			blackBarTop.rectTransform.anchoredPosition = vec2;

			vec2 = blackBarBottom.rectTransform.anchoredPosition;
			vec2.y = Mathf.Lerp(vec2.y, -65.0f, timer / hideShowTime);
			blackBarBottom.rectTransform.anchoredPosition = vec2;

			timer += Time.deltaTime;
			yield return new WaitForEndOfFrame();
			continue;
		}

		blackBarTop.rectTransform.anchoredPosition = new Vector2(blackBarTop.rectTransform.anchoredPosition.x, 65.0f);
		blackBarBottom.rectTransform.anchoredPosition = new Vector2(blackBarBottom.rectTransform.anchoredPosition.x, -65.0f);
	}

	[ContextMenu("Find Player")]
	void FindPlayer()
	{
		player = GameObject.FindGameObjectWithTag("Player");
	}
}
