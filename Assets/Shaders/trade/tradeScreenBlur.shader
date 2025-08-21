
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _BlurSize ("Blur Size", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _BlurSize;

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                return OUT;
            }

            float4 frag (Varyings IN) : SV_Target
            {
                float2 uv = IN.uv;
                float2 blurOffset = float2(_BlurSize / _ScreenParams.x, _BlurSize / _ScreenParams.y);

                float4 col = float4(0,0,0,0);
                col += tex2D(_MainTex, uv + blurOffset * float2(-1, -1));
                col += tex2D(_MainTex, uv + blurOffset * float2(-1,  0));
                col += tex2D(_MainTex, uv + blurOffset * float2(-1,  1));
                col += tex2D(_MainTex, uv + blurOffset * float2( 0, -1));
                col += tex2D(_MainTex, uv + blurOffset * float2( 0,  0));
                col += tex2D(_MainTex, uv + blurOffset * float2( 0,  1));
                col += tex2D(_MainTex, uv + blurOffset * float2( 1, -1));
                col += tex2D(_MainTex, uv + blurOffset * float2( 1,  0));
                col += tex2D(_MainTex, uv + blurOffset * float2( 1,  1));

                col /= 9;
                return col;
            }
            ENDHLSL
        }
    }
}