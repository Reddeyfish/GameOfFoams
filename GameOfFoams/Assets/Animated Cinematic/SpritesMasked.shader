Shader "Custom/SpriteMasked"
{
	Properties
	{
		_MainTex("Main Texture", 2D) = "white" {}
		_EffectTex1("Bumpmap1 (RG)", 2D) = "bump" {}
		_EffectTex2("Bumpmap2 (RG)", 2D) = "bump" {}
		_Shift("Shift", Vector) = (1, 1, 1, 1)
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
				half2 uvMain  : TEXCOORD0;
				half2 uvNoise1 : TEXCOORD1;
				half2 uvNoise2 : TEXCOORD2;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _EffectTex1;
			sampler2D _EffectTex2;
			float4 _MainTex_ST;
			float4 _EffectTex1_ST;
			float4 _EffectTex2_ST;
			half4 _Shift;
			
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
				half4 col = tex2D(_MainTex, i.uvMain);
				float mask = tex2D(_EffectTex1, i.uvNoise1  + _Shift.xy * _Time.gg).r
				 * tex2D(_EffectTex2, i.uvNoise2  + _Shift.zw * _Time.gg).r;

				 mask *= 2.5;
				 mask -= 0.5;

				return mask * col;
			}
			ENDCG
		}
	}
}
