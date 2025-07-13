Shader "Custom/MagicalParticleLiquid"
{
    Properties
    {
        _BaseColor("Base Color", Color) = (0.1, 0.4, 1, 1)
        _ParticleColor("Particle Color", Color) = (0.8, 0.9, 1.5, 1)
        _RippleColor("Ripple Color", Color) = (0.5, 0.7, 1.2, 1)
        
        _TimeVal("Time", Float) = 0
        _SpoonPos("Spoon Position", Vector) = (0.5, 0.5, 0, 0)
        
        _ParticleCount("Particle Count", Range(1, 100)) = 30
        _ParticleSize("Particle Size", Range(0.001, 0.1)) = 0.02
        _ParticleSpeed("Particle Speed", Range(0, 5)) = 0.5
        _ParticleGlow("Particle Glow", Range(1, 10)) = 3
        
        _DistortStrength("Distortion Strength", Range(0, 1)) = 0.2
        _WaveSpeed("Wave Speed", Range(0, 10)) = 3
        _WaveFrequency("Wave Frequency", Range(0, 100)) = 30
        
        _EdgeWidth("Edge Width", Range(0, 0.5)) = 0.1
        _EdgeGlow("Edge Glow", Range(0, 5)) = 2
    }
    
    SubShader
    {
        Tags { 
            "RenderPipeline"="UniversalPipeline" 
            "RenderType"="Transparent" 
            "Queue"="Transparent" 
        }
        
        Blend One OneMinusSrcAlpha
        ZWrite Off
        
        Pass
        {
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
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };
            
            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float4 _ParticleColor;
                float4 _RippleColor;
                
                float _TimeVal;
                float4 _SpoonPos;
                
                int _ParticleCount;
                float _ParticleSize;
                float _ParticleSpeed;
                float _ParticleGlow;
                
                float _DistortStrength;
                float _WaveSpeed;
                float _WaveFrequency;
                
                float _EdgeWidth;
                float _EdgeGlow;
            CBUFFER_END

            // Hash function for pseudo-random numbers
            float hash(float n)
            {
                return frac(sin(n)*43758.5453);
            }
            
            // Generate particle position with spoon interaction
            float2 particlePos(int i, float time)
            {
                float seed = float(i) * 100.0;
                float x = hash(seed + 0.3);
                float y = hash(seed + 0.7);
                float speedX = hash(seed + 1.3) * 2.0 - 1.0;
                float speedY = hash(seed + 1.7) * 2.0 - 1.0;
                
                // Circular motion with random variations
                float angle = time * _ParticleSpeed * (0.5 + hash(seed + 2.3) * 0.5);
                float radius = 0.1 + hash(seed + 3.1) * 0.3;
                
                // Base position with random offset
                float2 basePos = float2(0.5, 0.5) + float2(sin(angle), cos(angle)) * radius;
                
                // Random drift over time
                basePos += float2(sin(time * speedX * 0.3), cos(time * speedY * 0.3)) * 0.1;
                
                // Spoon interaction - get distance to spoon
                float spoonDistance = distance(basePos, _SpoonPos.xy);
                
                // Reduced reaction to spoon movement
                if (spoonDistance < 0.2)
                {
                    float2 repelDir = normalize(basePos - _SpoonPos.xy);
                    basePos += repelDir * (0.2 - spoonDistance) * 0.3;
                }
                
                return basePos;
            }
            
            // Calculate distance to nearest particle
            float particleField(float2 uv, float time)
            {
                float minDist = 1000.0;
                
                for (int i = 0; i < _ParticleCount; i++)
                {
                    float2 pPos = particlePos(i, time);
                    float dist = distance(uv, pPos);
                    
                    // Spoon interaction - particles are gently repelled
                    float spoonDistance = distance(pPos, _SpoonPos.xy);
                    if (spoonDistance < 0.2)
                    {
                        dist += (0.2 - spoonDistance) * 0.5;
                    }
                    
                    minDist = min(minDist, dist);
                }
                
                return minDist;
            }

            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionCS = TransformObjectToHClip(input.positionOS);
                output.uv = input.uv;
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                float2 uv = input.uv;
                float time = _TimeVal;
                
                // Base waves
                float wave1 = sin(uv.y * _WaveFrequency + time * _WaveSpeed) * _DistortStrength;
                float wave2 = cos(uv.x * _WaveFrequency * 0.7 + time * _WaveSpeed * 1.3) * _DistortStrength;
                
                // Particle field
                float pDist = particleField(uv, time);
                float particle = smoothstep(_ParticleSize, 0.0, pDist) * _ParticleGlow;
                
                // Ripple effect from spoon
                float spoonDistance = distance(uv, _SpoonPos.xy);
                float ripple = 0.0;
                if (spoonDistance < 0.2)
                {
                    ripple = sin(spoonDistance * 50 - time * 30) * (0.2 - spoonDistance) * 5.0;
                }
                
                // Combine distortions
                float2 distortedUV = uv + float2(wave1, wave2) * 0.5 + ripple * 0.1;
                
                // Circular mask
                float centerDist = length(uv - float2(0.5, 0.5));
                float mask = smoothstep(0.5, 0.5 - _EdgeWidth, centerDist);
                float edge = smoothstep(0.5 - _EdgeWidth * 0.5, 0.5 - _EdgeWidth, centerDist);
                
                // Final color
                half4 col = _BaseColor;
                col.rgb += particle * _ParticleColor.rgb;
                col.rgb += edge * _EdgeGlow * _RippleColor.rgb;
                col.rgb += ripple * _RippleColor.rgb;
                col.a = mask;
                
                return col;
            }
            ENDHLSL
        }
    }
}