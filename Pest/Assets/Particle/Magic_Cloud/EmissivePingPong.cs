using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmissivePingPong : MonoBehaviour
{
    [SerializeField] AnimationCurve emissiveCurve = new AnimationCurve();
    [SerializeField, Min(0.001f)] float duration = 0.0f;
    float timer = 0.0f;
    Color color = Color.black;
    Material material = null;

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<MeshRenderer>().material;
        color = material.GetColor("_EmissionColor");
		enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer >= duration)
        {
            timer = 0.0f;
        }

        float intensity = emissiveCurve.Evaluate(timer / duration);

        material.SetColor("_EmissionColor", color * intensity);
    }

	private void OnEnable()
	{
		if(material != null)
		{
			material.SetColor("_EmissionColor", color);
		}
		timer = 0.0f;
	}

	private void OnDisable()
	{
		if(material != null)
		{
			material.SetColor("_EmissionColor", color);
		}
	}
}
