Shader "Custom/FiveOctaveTwinklingGalaxy"
{
    Properties
    {
        [Header(Star Settings)]
        _StarDensity ("Star Density", Range(10, 500)) = 250
        _TwinkleSpeed ("Twinkle Speed", Range(0, 10)) = 1.5
        _Threshold ("Star Threshold", Range(0.9, 0.999)) = 0.97
        _StarColor1 ("Star Color 1", Color) = (0.6, 0.8, 1.0, 1) 
        _StarColor2 ("Star Color 2", Color) = (0.9, 0.9, 1.0, 1)

        [Header(Nebula Settings)]
        _NebulaColor1 ("Nebula Background (Deep Space)", Color) = (0.01, 0.02, 0.06, 1)
        _NebulaColor2 ("Nebula Highlight 1 (Cyan)", Color) = (0.0, 0.7, 0.9, 1)
        _NebulaColor3 ("Nebula Highlight 2 (Purple)", Color) = (0.5, 0.2, 0.8, 1)
        _NebulaScale ("Nebula Scale", Range(0.1, 10)) = 2.5
        _NebulaIntensity ("Nebula Intensity", Range(0, 5)) = 2.0
        _NebulaSpeed ("Nebula Drift Speed", Range(0, 1)) = 0.02

        [Header(Meteor Settings)]
        _MeteorColor ("Meteor Color", Color) = (1.5, 2.0, 2.5, 1) 
        _MeteorFrequency ("Meteor Spawn Frequency", Range(0.1, 5.0)) = 0.5
        _MeteorSpeed ("Meteor Travel Speed", Range(0.5, 10.0)) = 2.0 
        _MeteorTailLength ("Meteor Tail Length", Range(0.01, 1.5)) = 0.4 
        _MeteorThickness ("Meteor Thickness", Range(0.0001, 0.02)) = 0.002 
    }
    SubShader
    {
        Tags { "RenderType"="Background" "Queue"="Background" "PreviewType"="Skybox" }
        LOD 100
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing 

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 objectPos : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            // Star Properties
            float _StarDensity;
            float _TwinkleSpeed;
            float _Threshold;
            float4 _StarColor1; 
            float4 _StarColor2; 

            // Nebula Properties
            float4 _NebulaColor1;
            float4 _NebulaColor2;
            float4 _NebulaColor3;
            float _NebulaScale;
            float _NebulaIntensity;
            float _NebulaSpeed;

            // Meteor Properties
            float4 _MeteorColor;
            float _MeteorFrequency;
            float _MeteorSpeed;        
            float _MeteorTailLength;   
            float _MeteorThickness;    

            // *** NOISE & HASH FUNCTIONS ***
            float hash13(float3 p3)
            {
                p3  = frac(p3 * 0.1031);
                p3 += dot(p3, p3.zyx + 31.32);
                return frac((p3.x + p3.y) * p3.z);
            }

            float noise3D(float3 x)
            {
                float3 p = floor(x);
                float3 f = frac(x);
                f = f * f * (3.0 - 2.0 * f);

                return lerp(lerp(lerp( hash13(p + float3(0,0,0)), hash13(p + float3(1,0,0)), f.x),
                                 lerp( hash13(p + float3(0,1,0)), hash13(p + float3(1,1,0)), f.x), f.y),
                            lerp(lerp( hash13(p + float3(0,0,1)), hash13(p + float3(1,0,1)), f.x),
                                 lerp( hash13(p + float3(0,1,1)), hash13(p + float3(1,1,1)), f.x), f.y), f.z);
            }

            // fbm with 5 octaves and domain warping for wispy details
            float fbm(float3 x)
            {
                float v = 0.0;
                float a = 0.5;
                float3 shift = float3(100.0, 100.0, 100.0);
                
                // Domain warp offset
                float3 warp = float3(noise3D(x + 1.5), noise3D(x + 2.5), noise3D(x + 3.5));
                x += warp * 0.5;

                for (int i = 0; i < 5; ++i) 
                {
                    v += a * noise3D(x);
                    x = x * 2.0 + shift;
                    a *= 0.5;
                }
                return v;
            }

            // Helper function to generate a star layer
            float generateStars(float3 viewDir, float density, float threshold, out float3 starColorMod)
            {
                float3 gridPos = viewDir * density;
                float3 cellID = floor(gridPos);
                float3 localPos = frac(gridPos) - 0.5;

                float randomVal = hash13(cellID);
                starColorMod = hash13(cellID + float3(12.3, 45.6, 78.9)).xxx; 

                float hasStar = step(threshold, randomVal);
                float dist = length(localPos);
                
                // Sharper star shape
                float starShape = smoothstep(0.3, 0.02, dist);

                float twinkle = sin(_Time.y * _TwinkleSpeed + randomVal * 100.0) * 0.5 + 0.5;
                return hasStar * starShape * twinkle;
            }

            // *** MAIN SHADER ***

            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                
                o.pos = UnityObjectToClipPos(v.vertex);
                o.objectPos = v.vertex.xyz; 
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 viewDir = normalize(i.objectPos);

                // Nebula generation
                float3 driftOffset = float3(0.5, 0.8, 0.3) * _Time.y * _NebulaSpeed;
                float3 noisePos = (viewDir * _NebulaScale) + driftOffset;
                
                // Core gas density
                float rawFBM = fbm(noisePos);
                
                // Carve out dark dust lanes and boost bright spots using power
                float gasDensity = smoothstep(0.2, 0.9, rawFBM);
                gasDensity = pow(gasDensity, 1.8) * _NebulaIntensity;

                // Color mixing based on density and secondary noise
                float colorMap = noise3D(noisePos * 0.5 + float3(15.2, 38.4, 91.1));
                fixed4 multiColorHighlight = lerp(_NebulaColor2, _NebulaColor3, colorMap);
                
                // Deep space background with slight fbm variation
                fixed4 backgroundCol = lerp(float4(0,0,0,1), _NebulaColor1, rawFBM * 1.5);
                fixed4 nebulaCol = lerp(backgroundCol, multiColorHighlight, gasDensity);

                // STAR GENERATION (Two Layers + Clustering)
                // Layer 1: Dense, tiny background stars
                float3 starMod1;
                float starLayer1 = generateStars(viewDir, _StarDensity, _Threshold, starMod1);
                
                // Layer 2: Sparse, larger foreground stars
                float3 starMod2;
                float starLayer2 = generateStars(viewDir + float3(1,1,1), _StarDensity * 0.4, _Threshold - 0.02, starMod2);
                
                // Cluster stars around the nebula (brighter where gas is denser)
                float clusterFactor = lerp(0.3, 1.5, rawFBM);
                
                float finalStarInfo = (starLayer1 * 0.5 + starLayer2 * 1.5) * clusterFactor;
                fixed4 currentStarColor = lerp(_StarColor1, _StarColor2, starMod1.x);

                // METEOR GENERATION
                float timeBlock = floor(_Time.y * _MeteorFrequency); 
                float localTime = frac(_Time.y * _MeteorFrequency); 

                float mRand1 = hash13(float3(timeBlock, 1.0, 2.0));
                float mRand2 = hash13(float3(timeBlock, 3.0, 4.0));
                float3 pole = normalize(float3(mRand1 - 0.5, mRand2 - 0.5, mRand1 - mRand2));
                
                float equatorDist = 1.0 - abs(dot(viewDir, pole));
                float band = smoothstep(1.0 - _MeteorThickness, 1.0, equatorDist);
                
                float3 ortho = normalize(cross(pole, pole.yzx)); 
                float progress = dot(viewDir, ortho); 
                
                float movingPoint = (localTime * _MeteorSpeed) - (_MeteorSpeed * 0.5); 
                float tail = smoothstep(movingPoint - _MeteorTailLength, movingPoint, progress) * step(progress, movingPoint);
                float lifeFade = sin(localTime * 3.14159);
                
                fixed4 meteorGlow = band * tail * lifeFade * _MeteorColor;

                // Additive blending for stars to make them pop against the nebula
                fixed4 finalCol = nebulaCol + (finalStarInfo * currentStarColor) + meteorGlow;
                
                return finalCol;
            }
            ENDCG
        }
    }
}