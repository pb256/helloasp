Shader "Custom/DistortionShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_DistortMap ("Dirstort Map Texture", 2D) = "white" {}
		_DistortPower ("Distort Power", Float) = 0
		_ChannelDivision ("ChannelDivision", Float) = 0
        _ChannelDivisionMaskTex ("ChannelDivisionMaskTexture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

		GrabPass {
			"_GrabTex"
		}

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
				float4 grabPos : TEXCOORD0;
				float2 uv01 : TEXCOORD1;
				float2 maskUv : TEXCOORD2;
            };

            sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _GrabTex;
			sampler2D _DistortMap;
			sampler2D _ChannelDivisionMaskTex;
			float4 _ChannelDivisionMaskTex_ST;

			float _DistortPower;
			float _ChannelDivision;

            v2f vert (appdata v)
            {
                v2f o;
				
				// TODO : normalize 된 v.uv값 구해야함

                o.vertex = UnityObjectToClipPos(v.vertex);
				o.grabPos = ComputeGrabScreenPos(o.vertex);
				o.maskUv = TRANSFORM_TEX(v.uv, _ChannelDivisionMaskTex);
				o.uv01 = v.uv;
                return o;
            }

			fixed2 UvInverse(fixed2 uv)
			{
				return fixed2(uv.x, 1.0 - uv.y);
			}

            fixed4 frag (v2f i) : SV_Target
            {
				fixed2 invUv01 = UvInverse(i.uv01);
				
				#if UNITY_UV_STARTS_AT_TOP == 1
				// hlsl
				fixed4 distortCol  = tex2D(_DistortMap, i.uv01);
				fixed2 dirstortUv = invUv01 + (fixed2(distortCol.r, distortCol.g) - 0.5) * _DistortPower * 0.01;
				#else
				// glsl
				fixed4 distortCol  = tex2D(_DistortMap, i.uv01);
				fixed2 dirstortUv = i.uv01 + (fixed2(distortCol.r, distortCol.g) - 0.5) * _DistortPower * 0.01;
				#endif


				fixed chnDivAmount = _ChannelDivision * tex2D(_ChannelDivisionMaskTex, i.maskUv).r;

                fixed originalColR = tex2D(_GrabTex, dirstortUv + fixed2(chnDivAmount, 0) * 0.01).r;

                // fixed originalColG1 = tex2D(_GrabTex, dirstortUv + fixed2(chnDivAmount, 0) * 0.01).g;
                // fixed originalColG2 = tex2D(_GrabTex, dirstortUv - fixed2(chnDivAmount, 0) * 0.01).g;

				fixed originalColG = tex2D(_GrabTex, dirstortUv).g;

                fixed originalColB = tex2D(_GrabTex, dirstortUv - fixed2(chnDivAmount, 0) * 0.01).b;

				// fixed4 res = fixed4(originalColR, (originalColG1 + originalColG2) * 0.5, originalColB, 1.0);
				fixed4 res = fixed4(originalColR, originalColG, originalColB, 1.0);

				// test
				// res = fixed4(i.uv01.r, i.uv01.g, 0, 1);

                return res;
            }
            ENDCG
        }
    }
}
