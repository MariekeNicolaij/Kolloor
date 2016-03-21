Shader "Custom/ColorShader" 
{
	Properties 
	{
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		Smoothness ("Smoothness", Range(0,1)) = 0.5
		Metallicness ("Metallic", Range(0,1)) = 0.0
		ColorStartPoint ("ColorOrigen", Vector) = (0,0,0)
		ColorRadius ("ColorRadius", float) = 0.0
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input 
		{
			float2 uv_MainTex;
			float3 worldPos;
		};

		half Smoothness;
		half Metallicness;
		fixed4 _Color;
		float3 ColorStartPoint;
		float ColorRadius;

		float hash( float n )
		{
			return frac(sin(n)*43758.5453);
		}
 
		float noise( float3 x )
		{
			// The noise function returns a value in the range -1.0f -> 1.0f
 
			float3 p = floor(x);
			float3 f = frac(x);
 
			f       = f*f*(3-2*f);
			float n = p.x + p.y*57.0 + 113.0*p.z;
 
			return lerp(lerp(lerp( hash(n+0.0), hash(n+1.0),f.x),
						   lerp( hash(n+57.0), hash(n+58.0),f.x),f.y),
					   lerp(lerp( hash(n+113.0), hash(n+114.0),f.x),
						   lerp( hash(n+170.0), hash(n+171.0),f.x),f.y),f.z);
		}

		float GetGrayScaleValue(fixed4 c, Input IN)
		{
			float dist = length(ColorStartPoint - IN.worldPos);

			float extraDist = noise(IN.worldPos / 3.0)*3.0;
			dist += extraDist;
			
			float outOfRange = (dist - ColorRadius);
			float needGrayScale = clamp(min(1.0, max(0.0, outOfRange) * 10000000.0), 0.0, 1.0);

			return needGrayScale;
		}

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			// Albedo comes from a texture tinted by color

			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			fixed3 grayScale = dot(c.rgb, float3(0.3, 0.59, 0.11));
			grayScale *= 1.5f;


			float grayScaleValue = GetGrayScaleValue(c, IN);
			float normalColor = clamp(1.0 - grayScaleValue, 0.0, 1.0);

			c.rgb = (grayScale*grayScaleValue) + (c.rgb*normalColor);

			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = Metallicness;
			o.Smoothness = Smoothness;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}