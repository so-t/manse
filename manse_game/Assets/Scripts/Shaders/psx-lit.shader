Shader "custom/lit" {
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "LightMode" = "ForwardBase" }
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

				float4 snappedVertex = UnityObjectToClipPos(v.vertex);
				o.vertex = snappedVertex;
				o.vertex.xyz = snappedVertex.xyz / snappedVertex.w; // convert to normalised device coordinates (NDC)
				o.vertex.x = floor(160 * o.vertex.x) / 160; // Round x, adjusted for resolution
				o.vertex.y = floor(120 * o.vertex.y) / 120; // Round y, adjusted for resolution
				o.vertex.xyz *= snappedVertex.w; // convert back to projection-space
				
				float3 posWorld = mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1.0));
			    float3 normalWorld = UnityObjectToWorldNormal(v.normal);
			    o.light = Shade4PointLights (
			        unity_4LightPosX0, unity_4LightPosY0, unity_4LightPosZ0,
			        unity_LightColor[0].rgb, unity_LightColor[1].rgb, unity_LightColor[2].rgb, unity_LightColor[3].rgb,
			        unity_4LightAtten0, posWorld, normalWorld);
				o.color = v.color;
				o.color += UNITY_LIGHTMODEL_AMBIENT - .6;
				
				float distance = length(mul(UNITY_MATRIX_MV,v.vertex));
	
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.uv *= distance + o.vertex.w * ( UNITY_LIGHTMODEL_AMBIENT.a * 8) / distance / 2;
				o.normal = distance + o.vertex.w * (UNITY_LIGHTMODEL_AMBIENT.a * 8) / distance / 2;
				
				float4 fogColor = unity_FogColor;
				float fogDensity = (unity_FogEnd - distance) / (unity_FogEnd - unity_FogStart);
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
                fixed4 color = tex2D(_MainTex, IN.uv / IN.normal.r) * IN.fog.a;
                fixed4 vertexLights = float4(clamp(IN.light.rgb, 0, 1), 1.0);
                color.rgb *= vertexLights + IN.color;
				color.rgb += IN.fog.rgb * (1 - IN.fog.a);
				return color;
			}
			ENDCG
		}
	}
}