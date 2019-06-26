// <Talis' Code>
using UnityEngine;

public class TextureBlendingManager : MonoBehaviour
{
    /// <summary>
    /// Singleton: use this to access class
    /// </summary>
    public static TextureBlendingManager Instance = null;


    [SerializeField] Texture firstTexture  = null;
    [SerializeField] Texture secondTexture = null;

    /// <summary>
    /// How much textures are blended
    /// 0 = full first texture
    /// 1 = full second texture
    /// </summary>
    [SerializeField, Range(0.0f, 1.0f)]
    float textureBlend = 0.0f;

    /// <summary>
    /// Transparency value (alpha)
    /// </summary>
    [SerializeField, Range(0.0f, 1.0f)]
    float transparency = 1.0f;

    Material mat         = null;
    Shader   blendShader = null;

    float changedTextureBlend = 0.0f;
    float changedTransparency = 1.0f;
    
    // **** Properties ****

    public float TextureBlend {
        get { return textureBlend;  }
        set { textureBlend = value; }
    }

    public float Transparency {
        get { return transparency;  }
        set { transparency = value; }
    }

    // **** Methods ****

    void Awake()
    {
        Instance = this;
        mat = GetComponent<Renderer>().material;
        mat.shader = Shader.Find("Custom/BlendShader");
        mat.SetTexture("_FirstTex",  firstTexture);
        mat.SetTexture("_SecondTex", secondTexture);
    }

    void Update()
    {
        if (mat)
        {
            if(changedTextureBlend != textureBlend)
            {
                mat.SetFloat("_Blend", textureBlend);
                changedTextureBlend = textureBlend;
            }

            if (changedTransparency != transparency)
            {
                mat.SetFloat("_Transparency", transparency);
                changedTransparency = transparency;
            }
        }

        if(transparency <= 0)
        {
            GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }
        else
        {
            GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }
    }
}
// </Talis' Code>
