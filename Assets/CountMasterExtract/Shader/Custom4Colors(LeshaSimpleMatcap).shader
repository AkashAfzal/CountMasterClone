Shader "Custom/4Colors(LeshaSimpleMatcap)" {
	Properties {
		_Color1 ("Color 1", Vector) = (1,0,0,1)
		_Color2 ("Color 2", Vector) = (0.06159949,1,0,1)
		_Color3 ("Color3", Vector) = (0.0874176,0,1,1)
		_Color4 ("Color4", Vector) = (1,0.9935301,0,1)
		_ShadingColor ("ShadingColor", Vector) = (0,0,0,1)
		_Emission ("Emission", Range(0, 1)) = 0
		_Matcap_Intensity ("Matcap_Intensity", Range(0, 1)) = 0
		_Matcap ("Matcap", 2D) = "white" {}
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