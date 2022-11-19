Shader "Polygonal/GradientBackgroundShader"
{
	Properties
	{
		_ColorTop ("Top Color", Color) = (1, 1, 1, 1)
		_ColorBottom ("Bottom Color", Color) = (1, 1, 1, 1)
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

			#include "UnityCG.cginc"

			float4 _ColorTop;
			float4 _ColorBottom;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			float4 CalculateColor(float4 bottomColor, float4 topColor, float2 uv)
			{
				return lerp(bottomColor, topColor, uv.y);
			}

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = CalculateColor(_ColorBottom, _ColorTop, i.uv);

				return col;
			}
			ENDCG
		}
	}
}
