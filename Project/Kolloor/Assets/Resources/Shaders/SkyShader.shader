Shader "Unlit/SkyShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			uniform float3 ColorStartPoint;
			uniform float ColorRadius;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float4 worldPos : POSITION1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldPos = mul(_Object2World, v.vertex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}


			float hash(float n)
			{
				return frac(sin(n)*43758.5453);
			}

			float noise(float3 x)
			{
				// The noise function returns a value in the range -1.0f -> 1.0f

				float3 p = floor(x);
				float3 f = frac(x);

				f = f*f*(3.0 - 2.0*f);
				float n = p.x + p.y*57.0 + 113.0*p.z;

				return lerp(lerp(lerp(hash(n + 0.0), hash(n + 1.0), f.x),
					lerp(hash(n + 57.0), hash(n + 58.0), f.x), f.y),
					lerp(lerp(hash(n + 113.0), hash(n + 114.0), f.x),
						lerp(hash(n + 170.0), hash(n + 171.0), f.x), f.y), f.z);
			}

			float GetGrayScaleValue(half4 c, float4 pos)
			{
				float dist = length(ColorStartPoint - pos);


				float extraDist = noise(pos / 5.0)*5.0;
				dist += extraDist;



				float outOfRange = (dist - ColorRadius);
				float needGrayScale = clamp(min(1.0, max(0.0, outOfRange) * 10000000.0), 0.0, 1.0);

				return needGrayScale;
			}
			
			#pragma target 3.0
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 color = tex2D(_MainTex, i.uv);

				half4 grayScale = dot(color, float3(0.3, 0.59, 0.11));
				grayScale *= 1.8f;

				float grayScaleValue = GetGrayScaleValue(color, i.worldPos);
				float normalColor = clamp(1.0 - grayScaleValue, 0.0, 1.0);

				color = (grayScale*grayScaleValue) + (color*normalColor);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, color);
				return color;
			}
			ENDCG
		}
	}
}
