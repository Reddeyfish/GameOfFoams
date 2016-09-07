Shader "Custom/Wave"
{
	Properties
	{
		_MainTex("Main Texture", 2D) = "white" {}
		_EffectMask("Mask", 2D) = "white" {}
		_EffectTex1("Bumpmap1 (RG)", 2D) = "bump" {}
		_EffectTex2("Bumpmap2 (RG)", 2D) = "bump" {}
		_Shift("Shift", Vector) = (1, 1, 1, 1)
		_LightColor ("Light Color", Color) = (0,0,1,1)
		_LightColor2 ("Light Color2", Color) = (0,0,1,1)
		_Offset ("Offset", Float) = 0
		_Speed ("Speed", Float) = 1
	}
	SubShader
	{
		Tags { "Queue"="Transparent" "PreviewType"="Plane"}
		LOD 100

		Pass
		{
			ZWrite Off
			ZTest Off
			Blend One One
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

			inline float Repeat(float t, float length)
			{
				return t - floor(t / length) * length;
			}

			inline float PingPong(float t, float length)
			{
				t = Repeat(t, length * 2);
				return length - abs(t - length);
			}

			struct v2f
			{
				half2 uvMain  : TEXCOORD0;
				half2 uvNoise1 : TEXCOORD1;
				half2 uvNoise2 : TEXCOORD2;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _EffectMask;
			sampler2D _EffectTex1;
			sampler2D _EffectTex2;
			float4 _MainTex_ST;
			float4 _EffectMask_ST;
			float4 _EffectTex1_ST;
			float4 _EffectTex2_ST;
			float _Offset;
			float _Speed;
			half4 _Shift;
			half4 _LightColor;
			half4 _LightColor2;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uvNoise1 = TRANSFORM_TEX(v.uv, _EffectTex1);
				o.uvNoise2 = TRANSFORM_TEX(v.uv, _EffectTex2);
				o.uvMain = v.uv;
				return o;
			}
			
			fixed4 frag (v2f i) : COLOR
			{
				float uvMainYCentered = i.uvMain.y - 0.5;
				uvMainYCentered *= 1 + 10 * (1 - i.uvMain.x);
				float uvMainX = frac((3 + _Offset) * i.uvMain.x
								- (_Time.x * _Speed)
								 + _Offset);
				float2 uvMain = float2(uvMainX, uvMainYCentered + 0.5);
				half4 lerpedLightColor = lerp(_LightColor, _LightColor2, PingPong(_Time.x * _Speed, 1));
				float4 col = tex2D(_MainTex, uvMain);
				float mask = tex2D(_EffectTex1, i.uvNoise1  + _Shift.xy * _Time.x).r
				 * tex2D(_EffectTex2, i.uvNoise2  + _Shift.zw * _Time.x).r;
				return col * lerpedLightColor * mask * 3;
			}
			ENDCG
		}
	}
}
