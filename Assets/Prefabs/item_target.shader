Shader "Custom/item_target" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_TargetColor ("Target Color", Color) = (1,0,0,1)
		_ShineColor ("Shine Color", Color) = (1,1,1,1)
		_ShinePower ("Shine Power", Range(0.5,8.0)) = 3.0
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_ShineTex ("Shine Texture", 2D) = "white"{}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_Targeted ("targeted", Int) = 0
		_PulseSpeed ("Pulse Speed", float) = 3.0
 	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _ShineTex;
		
		struct Input {
			float2 uv_MainTex;
			float2 uv_ShineTex;
			float3 viewDir;
		};

		half _Glossiness;
		half _Metallic;
		half _Targeted;
		half _PulseSpeed;
		half _ShinePower;
		fixed4 _TargetColor;
		fixed4 _Color;
		fixed4 _ShineColor;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			if (_Targeted){
				fixed2 shine_uv = IN.uv_ShineTex;
				fixed4 c = tex2D (_MainTex, IN.uv_MainTex) + abs(cos(_Time * _PulseSpeed) * _TargetColor);
				half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
				o.Albedo = c.rgb;
				o.Metallic = _Metallic;
				o.Smoothness = _Glossiness;
				o.Alpha = c.a;
				o.Emission = _ShineColor.rgb * pow(rim, _ShinePower);
			}
			else{
				fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
				o.Albedo = c.rgb;
				o.Metallic = _Metallic;
				o.Smoothness = _Glossiness;
				o.Alpha = c.a;
			}
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
