Shader "custom/lit" {
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "LightMode" = "Vertex" } // Legacy LightMode required to invoke ShadeVertexLightsFull()
		LOD 200
		
		Pass
		{	
			lighting ON
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			
            #include "UnityCG.cginc"

			struct v2f
			{
				fixed4 vertex : SV_POSITION;
				half4 color : COLOR0;
				half3 light : COLOR1;
				half4 fog : COLOR2;
				float2 uv : TEXCOORD0;
				half3 normal : TEXCOORD1;
			};

			float4 _MainTex_ST;
			uniform half4 unity_FogStart;
			uniform half4 unity_FogEnd;

			v2f vert(appdata_full v)
			{
				v2f o;

				// Snap vertex positions to pixel locations
				float4 snappedVertex = UnityObjectToClipPos(v.vertex);
				o.vertex = snappedVertex;
				o.vertex.xyz = snappedVertex.xyz / snappedVertex.w; // Convert to normalised device coordinates
				o.vertex.x = floor(160 * o.vertex.x) / 160; // Round x, adjusted for resolution
				o.vertex.y = floor(120 * o.vertex.y) / 120; // Round y, adjusted for resolution
				o.vertex.xyz *= snappedVertex.w; // Convert back to projection-space

				// Handle vertex lighting
				o.light = ShadeVertexLightsFull(v.vertex, v.normal, 8, true);

				// Adjust color with ambient light
				o.color = v.color;
				o.color += UNITY_LIGHTMODEL_AMBIENT - 0.5;

				// Transform vertex position into camera relative space
				float distance = length(mul(UNITY_MATRIX_MV,v.vertex));

				// Handle affine texture mapping
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.uv *= distance + o.vertex.w * ( UNITY_LIGHTMODEL_AMBIENT.a * 8) / distance / 2;
				o.normal = distance + o.vertex.w * (UNITY_LIGHTMODEL_AMBIENT.a * 8) / distance / 2;

				// Handle fog
				float fogDensity = (unity_FogEnd - distance) / (unity_FogEnd - unity_FogStart);
				float4 fogColor = unity_FogColor;
				
				o.normal.g = fogDensity;
				o.normal.b = 1;
				
				o.fog = fogColor;
				o.fog.a = clamp(fogDensity, 0, 1);

				if (distance > unity_FogStart.z + unity_FogColor.a * 255)
				{
					o.vertex.w = 0;
				}

				return o;
			}

			sampler2D _MainTex;

			float4 frag(v2f IN) : COLOR
			{
                fixed4 color = tex2D(_MainTex, IN.uv / IN.normal.r) * IN.color * IN.fog.a;
                fixed4 vertexLights = float4(clamp(IN.light.rgb, 0, 1), 1.0);
                color.rgb *= vertexLights + IN.color * UNITY_LIGHTMODEL_AMBIENT;
				color.rgb += IN.fog.rgb * (1 - IN.fog.a);
				return color;
			}
			ENDCG
		}
	}
}