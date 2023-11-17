Shader "custom/unlit-transparent" {
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "Queue" = "Transparent" }
		LOD 200
		
		Pass
		{
			lighting OFF
			Blend SrcAlpha OneMinusSrcAlpha
			
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct v2f
			{
				fixed4 vertex : SV_POSITION;
				half4 color : COLOR0;
				float2 uv : TEXCOORD0;
				half3 normal : TEXCOORD1;
			};

			float4 _MainTex_ST;
			uniform half4 unity_FogStart;
			uniform half4 unity_FogEnd;

			v2f vert(appdata_full v)
			{
				v2f o;

				float4 snappedVertex = UnityObjectToClipPos(v.vertex);
				o.vertex = snappedVertex;
				o.vertex.xyz = snappedVertex.xyz / snappedVertex.w; // convert to normalised device coordinates (NDC)
				o.vertex.x = floor(160 * o.vertex.x) / 160; // Round x, adjusted for resolution
				o.vertex.y = floor(120 * o.vertex.y) / 120; // Round y, adjusted for resolution
				o.vertex.xyz *= snappedVertex.w; // convert back to projection-space
				
				o.color = half4(v.color.rgb + UNITY_LIGHTMODEL_AMBIENT.rgb, v.color.a);

				float distance = length(mul(UNITY_MATRIX_MV,v.vertex));
	
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.uv *= distance + o.vertex.w * (UNITY_LIGHTMODEL_AMBIENT.a * 8) / distance / 2;
				o.normal = distance + o.vertex.w * (UNITY_LIGHTMODEL_AMBIENT.a * 8) / distance / 2;

				
				float4 fogColor = unity_FogColor;
				float fogDensity = (unity_FogEnd - distance) / (unity_FogEnd - unity_FogStart);
				o.normal.g = fogDensity;
				o.normal.b = 1;

				if (distance > unity_FogStart.z + unity_FogColor.a * 255)
				{
					o.vertex.w = 0;
				}

				return o;
			}

			sampler2D _MainTex;

			float4 frag(v2f IN) : COLOR
			{
				half4 color = tex2D(_MainTex, IN.uv / IN.normal.r) * IN.color;
				return color;
			}
			ENDCG
		}
	}
}