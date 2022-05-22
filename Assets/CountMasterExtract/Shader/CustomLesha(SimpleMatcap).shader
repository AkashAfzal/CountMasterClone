Shader "Custom/Lesha(SimpleMatcap)" {
	Properties {
		_Color ("Color", Vector) = (1,1,1,1)
		_ShadingColor ("ShadingColor", Vector) = (0,0,0,1)
		_MainTex ("MainTex", 2D) = "white" {}
		_Emission ("Emission", Range(0, 1)) = 0
		_Matcap_Intensity ("Matcap_Intensity", Range(0, 1)) = 0
		_Matcap ("Matcap", 2D) = "white" {}
		[HideInInspector] _texcoord ("", 2D) = "white" {}
	}
	
	//DummyShaderTextExporter - One Sided
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard fullforwardshadows
#pragma target 3.0
		sampler2D _MainTex;
		struct Input
		{
			float2 uv_MainTex;
		};
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
		}
		ENDCG
	}
	//CustomEditor "ASEMaterialInspector"
}