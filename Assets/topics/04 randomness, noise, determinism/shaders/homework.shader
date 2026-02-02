  Shader "shader lab/week 4/homework" {
      Properties
    {
        _Blend ("Blend With Color", Range(0,1)) = 0
        _BlendColor ("Blend Color", Color) = (0,0,0,1)
    }
    SubShader {
        Tags { "RenderPipeline" = "UniversalPipeline"  "LightMode" = "UniversalForward"}
        
        Pass {
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            
            CBUFFER_START(UnityPerMaterial)
                float _Blend;
                float4 _BlendColor;
            CBUFFER_END
            float rand(float2 uv) {
                return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453123);
            }

            float value_noise (float2 uv) {
                float2 ipos = floor(uv);
                float2 fpos = frac(uv); 
                
                float o  = rand(ipos);
                float x  = rand(ipos + float2(1, 0));
                float y  = rand(ipos + float2(0, 1));
                float xy = rand(ipos + float2(1, 1));

                float2 smooth = smoothstep(0, 1, fpos);
                return lerp( lerp(o,  x, smooth.x), 
                             lerp(y, xy, smooth.x), smooth.y);
            }

            struct MeshData {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Interpolators {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Interpolators vert (MeshData v) {
                Interpolators o;
                o.vertex = TransformObjectToHClip(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (Interpolators i) : SV_Target {
                float2 uv = i.uv;

                // pushed aurora up a bit because it looked bad cut off by mountains
                float auroraOffset = -0.2; 
                uv.y += auroraOffset;
                float time = _Time.y * 0.3; // sloed down for "aurora" speed
                float curtains = 0.0; // for aurora streaks

                // mask for gaps between aurora (adds gaps so its not just all aurora)
                float mask = value_noise(float2(uv.x * 3.0, time * 0.3));
                mask = smoothstep(0.3, 0.7, mask);

                // layered aurora streaks(looping over 3 layered streaks)
                for (int j = 0; j < 3; j++) {
                    // the streaks are made on uv.x (horizonata), then its stretched iwht 18 + j*3... (compared to just uv.x * 1.0), which adds more frequency of streaks, then warped, then apply noise
                    float warp = sin(uv.y * (6 + j*2) + time * (1.5 + j*0.3)) * (0.03 + j*0.01); 
                    float xCoord = uv.x * (18.0 + j*3) + warp; // x is warped diff every layer
                    // x coordinate for noise

                    float streak = value_noise(float2(xCoord, time*0.5 + j*0.8)); // noise x and time
                    streak = smoothstep(0.25, 0.75, streak); // width of streak

                    // adding limit for aurora so it doesnt fill whole screen
                    float bottomMin = 0.0;
                    float bottomMax = .7;
                    float topMin = 0.55;
                    float topMax = 1.05;

                    // noise to add variety
                    float bottomShift = 0.05 * value_noise(float2(time * 0.1,  uv.x * 2.0));
                    float topShift = 0.05 * value_noise(float2(time * 0.1 + 5.0, uv.x * 2.0));

                    // little up and down movement to look more like aurora
                    float auroraEffect = 0.05 + 0.02 * sin(time * 0.1);
                    float effectBottom = auroraEffect * sin(time * 0.5);
                    float effectTop = auroraEffect * sin(time * 0.5 + 1.5);

                    bottomShift += effectBottom;
                    topShift += effectTop;

                    // fading top and bottom of aurora to make it blend into the sky(ended up not using the top blend because it wasnt coorperating and looked bad)
                    // I actually wanted the whole aurora to show on the screen, but I couldn't figure out how to do it wihtout making the gradient look weird, so I just decided to make it go off the screen
                    float bottomFade = smoothstep(bottomMin + bottomShift, bottomMax + bottomShift, uv.y);
                    float topFade = 1.0 - smoothstep(topMin + topShift, topMax + topShift, uv.y);

                    bottomFade = pow(bottomFade, 0.6); // used exponent to try and make the gradient softer, not sure if it actually helped
                    topFade = pow(topFade, 0.6);
                    float verticalFade = bottomFade * topFade;

                    float cutoff = 0.05 * sin(uv.x * 20.0 + time * 0.7); // horizontal sine to try and make bottom of aurora look uneven
                    verticalFade *= smoothstep(0.0, 1.0, uv.y + cutoff);

                    // blending all the streaks together
                    float warpAllX = 0.05 * sin(uv.y * 1.0 + time * 0.1);
                    float warpAllY = 0.05 * sin(uv.x * 1.0 + time * 0.1);
                    uv.x += warpAllX;
                    uv.y += warpAllY;
                    
                    streak *= (1.0 - j*0.3); // layers get
                    curtains += streak * verticalFade * mask;
                }

                curtains = saturate(curtains); // stay 0-1

                // aurora colors
                float3 bottomCol = float3(0.0, 0.5, 0.1); //rgb
                float3 midCol = float3(0.1, 0.1, 0.9);
                float3 topCol = float3(0.5, 0.1, 0.5);

                // just like real aurora, different altitude has different color. based on the pictures i looked at, it seemed generally red/pink is at the top, and fades down to a green at lower altitude
                float3 auroraCol = lerp(bottomCol, midCol, uv.y);
                auroraCol = lerp(auroraCol, topCol, smoothstep(0.4, 1.0, uv.y));

                // stars, stars that twinkle and move slowly
                float starDensity = 200.0;
                float2 cell = floor(uv * starDensity);
                float2 offset = float2(rand(cell), rand(cell * 2.71)); // the stars were lining up weirdly
                float2 starPos = (cell + offset) / starDensity;
                float d = length(uv - starPos); // dist from star center
                float size = lerp(0.002, 0.005, rand(cell * 11.13)); // random star size
                float star = smoothstep(size, 0.0, d);

                // star twinkling: each star has random time phsae for blinking, random speed, and sine chanes brightness
                float noiseVal = rand(cell);
                float phase = noiseVal * 10.0;
                float starSpeed = lerp(2.0, 6.0, rand(cell * 7.77));
                float twinkle = 0.6 + 0.4 * sin(_Time.y * starSpeed + phase);

                star *= step(0.995, noiseVal); // had tog et rid of most of the stars because or else it just fills te whole screen
                star *= (1.0 - curtains); // i didnt want them on top of aurora

                float brightness = lerp(0.2, 1, rand(cell * 19.31)); // random brightness
                star *= brightness;
                star *= twinkle;

                float3 starCol = float3(0.9, 0.95, 1.0) * star;

                // blend
                float3 sky = float3(0.01, 0.01, 0.03);
                float3 color = sky + auroraCol * curtains + starCol;

                // mountains : used noise because sin didnt look right, too smooth
                float horizon = 0.1; // start height
                // frequency and variation of mountain peaks
                float jagged1 = value_noise(float2(i.uv.x * 12.5, 1.0));
                float jagged2 = value_noise(float2(i.uv.x * 5.0, 2.0));
                float horizonHeight = horizon + 0.1 * jagged2 + 0.15 * jagged1;

                float3 finalColor;

                if (i.uv.y < horizonHeight)
                {
                    finalColor = float3(0.006, 0.006, 0.02);
                }
                else
                {
                    finalColor = saturate(color);
                }

                // 外部可控 lerp
                finalColor = lerp(finalColor, _BlendColor.rgb, _Blend);

                return float4(finalColor, 1.0);
            }
            ENDHLSL
        }
    }
}

