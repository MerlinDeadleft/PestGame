using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ModularAI;

public class AwarenessIndicatorController : MonoBehaviour
{
	[SerializeField] AIBrain aiBrain = null;
	[SerializeField] GameObject awarenessBar = null;
	[SerializeField] GameObject detectedIcon = null;
	[SerializeField] Image awarenessBarFill = null;
	float detectedIconShowtime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
		GetComponent<Canvas>().worldCamera = Camera.main;
		awarenessBar.SetActive(false);
		detectedIcon.SetActive(false);
	}

    // Update is called once per frame
    void Update()
    {
		LookAtCamera();
		float scaledAwareness = Mathf.Clamp01(aiBrain.Awareness * 1.05f);
		awarenessBarFill.fillAmount = scaledAwareness;

		if(awarenessBarFill.fillAmount <= 0.01f)
		{
			awarenessBar.SetActive(false);
			detectedIcon.SetActive(false);
		}

		if(awarenessBarFill.fillAmount > 0.01f)
		{
			awarenessBar.SetActive(true);
			detectedIcon.SetActive(false);
		}

		if(awarenessBarFill.fillAmount >= 1.0f)
		{
			awarenessBar.SetActive(false);
			detectedIcon.SetActive(true);
		}
		else
		{
			detectedIconShowtime = 0.0f;
		}

		detectedIconShowtime += Time.deltaTime;

		if(detectedIconShowtime >= 1.0f)
		{
			detectedIcon.SetActive(false);
		}
	}

	void LookAtCamera()
	{
		transform.LookAt(Camera.main.transform, Vector3.up);
		//transform.Rotate(transform.up, 180.0f);
	}
}
