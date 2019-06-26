using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightStuff : MonoBehaviour
{
	public Texture2D m_AttenTex = null;
    // Start is called before the first frame update
    void Start()
    {
		//=== Point Light Attenutation
		//Texture2D m_AttenTex = new Texture2D(256, 1, TextureFormat.ARGB32, false);
		//m_AttenTex.filterMode = FilterMode.Bilinear;
		//m_AttenTex.wrapMode = TextureWrapMode.Clamp;
		//Color[] AttenColor = new Color[256];

		//for(int i = 0; i < 256; ++i)
		//{
		//	float v;

		//	if(i < 255)
		//	{
		//		v = i / 255.0f;
		//		v = 1.0f / (1.0f + 25.0f * v);
		//	}
		//	else
		//		v = 0.0f;

		//	AttenColor[i] = new Color(v, v, v, v);
		//}

		//m_AttenTex.SetPixels(AttenColor);
		//m_AttenTex.Apply();
		Shader.SetGlobalTexture("_LightTextureB02", m_AttenTex);
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
