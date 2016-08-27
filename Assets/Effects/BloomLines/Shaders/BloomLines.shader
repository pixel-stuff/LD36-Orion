Shader "Custom/BloomLines" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Addition("Base (RGB)", 2D) = "white" {}
		_Color("Boom Color", Color) = (1, 1, 1, 1)
		_Size("Size", Vector) = (1, 1, 0, 0)
		_PatchSize("Patch Size", Float) = 5
		_Quality("Quality", Float) = 2.5
	}
	SubShader{
		Pass{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _Addition;
			uniform float4 _Color;
			uniform float2 _Size;
			uniform int _PatchSize = 5; // pixels per axis; higher = bigger glow, worse performance
			uniform float _Quality = 2.5f; // lower = smaller glow, better quality

			float4 bloom(float4 color, sampler2D tex, float2 uv)
			{
				float4 source = tex2D(tex, uv);
				float4 sum = float4(0.0f, 0.0f, 0.0f, 0.0f);
				int diff = (_PatchSize - 1) / 2;
				float sizeFactor = float2(1.0f, 1.0f) / _Size * _Quality;

				for (int x = -diff; x <= diff; x++)
				{
					for (int y = -diff; y <= diff; y++)
					{
						float2 offset = float2(x, y) *sizeFactor;
						sum += tex2D(tex, uv + offset);
					}
				}

				return ((sum / (_PatchSize * _PatchSize)) + source) * color;
			}

			float4 customEffect(float4 color, sampler2D tex, float2 uv)
			{
				float4 source = tex2D(tex, uv);
				float4 sum = float4(0.0f, 0.0f, 0.0f, 0.0f);
				int diff = (_PatchSize - 1) / 2;
				float sizeFactor = float2(1.0f, 1.0f) / _Size * _Quality;
				float3 max = float3(0.0f, 0.0f, 0.0f);
				float3 min = float3(-10000.0f, -10000.0f, 0.0f);
				for (int x = -diff; x <= diff; x++)
				{
					for (int y = -diff; y <= diff; y++)
					{
						float2 offset = float2(x, y);// *sizeFactor;

						float4 color = tex2D(tex, uv + offset);
						if (color.r > source.r)
						{
							max.z = color.r;
							max.xy = offset;
						}
					}
				}

				float v = lerp(0.0f, 1.0f, length(max.xy) / _PatchSize);
				return float4(v, v, v, 1.0f) * color;
			}

			fixed4 frag(v2f_img i) : COLOR
			{
				float2 uv_add = float2(i.uv.x, 1.0-i.uv.y);
				return tex2D(_Addition, i.uv);
				//return bloom(_Color, _Addition, i.uv);
				return tex2D(_MainTex, i.uv) + bloom(_Color, _Addition, i.uv);
			}
		ENDCG
		}
	}
}