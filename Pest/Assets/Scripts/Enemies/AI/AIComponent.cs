using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIComponent : MonoBehaviour
{
	public GameObject Player { get; set; } = null;
	public AnimationCurve awarenessRampUpCurve = new AnimationCurve();
	public float Awareness { get; protected set; } = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
		Player = GameObject.FindGameObjectWithTag("Player");
    }
}
