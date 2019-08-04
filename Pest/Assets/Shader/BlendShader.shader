// Talis Code
Shader "Custom/BlendShader"
{
    Properties
    {
        _Color ("Color", Color)					   = (1,1,1,1)
		_Blend ("Texture Blend", Range(0,1))	   = 0.0
		_Transparency ("Transparency", Range(0,1)) = 1
		_FirstTex("First Albedo Texture", 2D)	   = "white" {}
		_SecondTex ("Second Albedo Texture", 2D)   = "black" {}
        _Glossiness ("Smoothness", Range(0,1))	   = 0.5
        _Metallic ("Metallic", Range(0,1))		   = 0.0
    }
    SubShader
    {
		Tags {
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"Queue" = "Transparent"
		}
		//ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		Cull back
		LOD 100

		Pass
		{
			Name "ShadowCaster"
			Tags { "LightMode" = "ShadowCaster" }

			Fog {Mode Off}
			ZWrite On ZTest Less Cull Off
			Offset 1, 1

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile_shadowcaster
			//#include "ShadowCastCG.cginc"
			#include "UnityCG.cginc"

			uniform float4 _FirstTex_ST;

			sampler2D _FirstTex;
			sampler2D _SecondTex;

			half _Blend;
			half _Transparency;
			half _Glossiness;
			half _Metallic;
			fixed4 _Color;
			
			
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 uv : TEXCOORD1;
			};

			v2f vert(appdata_full v)
			{
				v2f o;
				TRANSFER_SHADOW_CASTER(o)
				o.uv = TRANSFORM_TEX(v.texcoord, _FirstTex);

			  return o;
			}

			float4 frag(v2f i) : COLOR
			{
				fixed4 texcol = tex2D(_FirstTex, i.uv);
				clip(texcol.a*_Color.a - _Transparency);
				SHADOW_CASTER_FRAGMENT(i)
			}
			ENDCG
		}

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows alpha:fade addshadow
		
        // Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0


        sampler2D _FirstTex;
		sampler2D _SecondTex;

        struct Input
        {
			float2 uv_FirstTex;
			float2 uv_SecondTex;
        };

		half _Blend;
		half _Transparency;
		half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
			fixed4 c = lerp(tex2D(_FirstTex, IN.uv_FirstTex), tex2D(_SecondTex, IN.uv_SecondTex), _Blend) * _Color;
			c.a = _Transparency;
			//c.a = _TransparencyMult;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
// Talis Code end