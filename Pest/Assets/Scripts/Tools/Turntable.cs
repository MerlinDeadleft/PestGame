using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turntable : MonoBehaviour
{
	[SerializeField] float turnspeed = 10.0f;

    // Update is called once per frame
    void Update()
    {
		transform.Rotate(transform.up, turnspeed * Time.deltaTime);
    }
}
