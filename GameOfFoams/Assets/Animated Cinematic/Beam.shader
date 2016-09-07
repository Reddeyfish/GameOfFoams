Shader "Custom/Wave"
{
	Properties
	{
		_EffectTex1("Bumpmap1 (RG)", 2D) = "bump" {}
		_EffectTex2("Bumpmap2 (RG)", 2D) = "bump" {}
		_Shift("Shift", Vector) = (1, 1, 1, 1)
		_LightColor ("Light Color", Color) = (0,0,1,1)
		_LightColor2 ("Light Color2", Color) = (0,0,1,1)
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

			struct v2f
			{
				half2 uvMain : TEXCOORD0;
				half2 uvNoise1 : TEXCOORD1;
				half2 uvNoise2 : TEXCOORD2;
				float4 vertex : SV_POSITION;
			};

			sampler2D _EffectTex1;
			sampler2D _EffectTex2;
			float4 _EffectTex1_ST;
			float4 _EffectTex2_ST;
			half4 _Shift;
			half4 _LightColor;
			half4 _LightColor2;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uvMain = v.uv;
				o.uvNoise1 = TRANSFORM_TEX(v.uv, _EffectTex1);
				o.uvNoise2 = TRANSFORM_TEX(v.uv, _EffectTex2);
				return o;
			}
			
			fixed4 frag (v2f i) : COLOR
			{
				float mask1 = tex2D(_EffectTex1, i.uvNoise1  + _Shift.xy * _Time.x).r;
				float2 mask2 = tex2D(_EffectTex2, i.uvNoise2  + _Shift.zw * _Time.x).rg;
				float mask = mask1 * mask2.r;
				mask *= min(i.uvMain.y, 1 - i.uvMain.y);
				mask *= min(i.uvMain.x * 40, 1);
				float4 col = lerp(_LightColor, _LightColor2, mask2.g);
				return col * mask;
			}
			ENDCG
		}
	}
}
