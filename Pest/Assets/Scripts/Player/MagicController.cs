using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicController : MonoBehaviour
{
	[SerializeField] float maxMana = 100.0f;
	public float MaxMana { get { return maxMana; } }
	public float Mana { get; set; } = 50.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void TurnOffLight(Light light)
	{
		light.enabled = false;
	}
}
