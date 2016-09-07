Shader "Custom/SpriteDistortion"
{
	Properties
	{
		_MainTex("Main Texture", 2D) = "white" {}
		_EffectMask("Mask", 2D) = "white" {}
		_EffectMask2("Mask 2", 2D) = "white" {}
		_EffectTex1("Bumpmap1 (RG)", 2D) = "bump" {}
		_EffectTex2("Bumpmap2 (RG)", 2D) = "bump" {}
		_Shift("Shift", Vector) = (1, 1, 1, 1)
		_Shift2("Shift2", Vector) = (1, 1, -1, 1)
		_Intensity ("Distortion Strength", Float) = 0.1
		_Intensity2 ("Light Strength", Float) = 0.1
		_LightColor ("Light Color", Color) = (0,0,1,1)
		_LightColor2 ("Light Color 2", Color) = (0,0,1,1)
	}
	SubShader
	{
		Tags { "Queue"="Transparent" "PreviewType"="Plane"}
		LOD 100

		Pass
		{
			ZWrite Off
			ZTest Off
			Blend SrcAlpha OneMinusSrcAlpha
			Lighting Off
			Fog { Mode Off }
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				half2 uv : TEXCOORD0;
			};

			struct v2f
			{
				half2 uvMain  : TEXCOORD0;
				half2 uvNoise1 : TEXCOORD1;
				half2 uvNoise2 : TEXCOORD2;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _EffectMask;
			sampler2D _EffectMask2;
			sampler2D _EffectTex1;
			sampler2D _EffectTex2;
			float4 _MainTex_ST;
			float4 _EffectMask_ST;
			float4 _EffectTex1_ST;
			float4 _EffectTex2_ST;
			half4 _Shift;
			half4 _Shift2;
			half _Intensity;
			half _Intensity2;
			half4 _LightColor;
			half4 _LightColor2;

			inline half2 distortion(half2 rg)
			{
				return (2*rg - 1);
			}

			inline float2 Repeat(float2 t, float2 length)
			{
				return t - floor(t / length) * length;
			}

			inline float2 PingPong(float2 t, float2 length)
			{
				t = Repeat(t, length * 2);
				return length - abs(t - length);
			}
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uvMain = TRANSFORM_TEX(v.uv, _MainTex);
				o.uvNoise1 = TRANSFORM_TEX(v.uv, _EffectTex1);
				o.uvNoise2 = TRANSFORM_TEX(v.uv, _EffectTex2);
				return o;
			}
			
			fixed4 frag (v2f i) : COLOR
			{
				half2 effectColor = (
					tex2D(_EffectTex1, i.uvNoise1  + _Shift.xy * _Time.gg) +
					tex2D(_EffectTex2, i.uvNoise2  + _Shift.zw * _Time.gg)
					/2).rg;
				half effectIntensity = _Intensity * (tex2D(_EffectMask, i.uvMain).r);
				half2 uv = i.uvMain + effectIntensity * distortion(effectColor);

				half4 col = tex2D(_MainTex, uv);
				half4 lerpedLightColor = lerp(_LightColor, _LightColor2, _SinTime.w);
				half3 light = ((1 + abs(_CosTime.y)) / 2) * lerpedLightColor.a * lerpedLightColor * tex2D(_EffectMask2, uv);
				col.rgb += light;

				return col;
			}
			ENDCG
		}
	}
}
