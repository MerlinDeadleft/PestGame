using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingIconRotator : MonoBehaviour
{
	[SerializeField] AnimationCurve rotationCurve = new AnimationCurve();
	float timer = 0.0f;
	RectTransform rect = null;
	[SerializeField] float fullRotationTime;
	void Start()
	{
		rect = GetComponent<RectTransform>();
	}

	// Update is called once per frame
	void Update()
    {
		rect.localRotation = Quaternion.Euler(0.0f, 0.0f, rotationCurve.Evaluate(timer/fullRotationTime) * -360.0f);
		timer += Time.deltaTime;
		if(timer >= fullRotationTime)
		{
			timer = 0.0f;
		}
    }
}
