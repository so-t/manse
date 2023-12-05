Shader "custom/linear-gamma correction" {
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		
		Pass
		{
			lighting OFF
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

			v2f vert(appdata_full v)
			{
				v2f o;

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = pow(v.color, 2.2);
	
				float distance = length(mul(UNITY_MATRIX_MV,v.vertex));
	
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.normal = distance + o.vertex.w * (UNITY_LIGHTMODEL_AMBIENT.a * 8) / distance / 2;

				return o;
			}

			sampler2D _MainTex;

			float4 frag(v2f IN) : COLOR
			{
				half4 color = tex2D(_MainTex, IN.uv) * IN.color;
				return color;
			}
			ENDCG
		}
	}
}