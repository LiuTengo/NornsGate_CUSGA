Shader "NornsGate/Universal/Skybox"
{
    Properties
    {
        [Header(Test)]
        [Space(10)]
        _MainColor ("MainColor", Color) = (.25, .5, .5, 1)
        
        [Header(SunAndMoon)]
        [Space(10)]
        [HDR]_SunColor ("太阳颜色", Color) = (.25, .5, .5, 1)
        _SunRadius ("太阳大小", range(0, 20)) = 1
        _MoonRadius ("月亮大小", range(0, 0.5)) = 0.5
        _MoonSmooth("Moon Smooth", Range(0, 0.01)) = 0.01
        // 月亮周围光晕的范围
        [HDR]_MoonColor ("月亮基础颜色", Color) = (.25, .5, .5, 1)
        _CubeMapLOD ("CubeMapLOD", Range(1, 6)) = 2
        [NoScaleOffset]_MoonTexCube ("月亮纹理CubeMap", Cube) = "_Skybox" {}
        
        [Header(SkyGradient)]
        [Space(10)]
        _DayC01 ("清晨天空颜色", Color) = (.25, .5, .5, 1)
        _DayC02 ("正午天空颜色", Color) = (.25, .5, .5, 1)
        _DayC03 ("晚上天空颜色", Color) = (.25, .5, .5, 1)
        _NightC01 ("夜间天空颜色", Color) = (.25, .5, .5, 1)
        _HorWidth ("地平线范围(宽度)", range(0,0.5)) = 0.1
        _NightHorColor ("夜间地平线颜色", Color) = (.25, .5, .5, 1)
        _DayHorColor ("白天地平线颜色", Color) = (.25, .5, .5, 1)
        
        [Header(Stars)]
        [Space(10)]
        _StarTex ("星空纹理", 2D) = "white"{}
        _StarNoise3D ("星空噪声纹理", 3D) = "black"{}
        _GalaxyTex ("银河纹理", 2D) = "white"{}
        _GalaxyNoiseTex ("银河噪声纹理", 2D) = "black"{}
        _GalaxyColor1 ("银河颜色01", Color) = (0, 0, 0, 1)
        _GalaxyColor2 ("银河颜色02", Color) = (0, 0, 0, 1)
        _GalaxyRadius ("银河范围", range(0, 5)) = 0.5
    }
    
    
    SubShader
    {
        Tags { "RenderType" = "Background" "PreviewType" = "Skybox" "RenderPipeline" = "UniversialPipeline" "Queue" = "Background"}
        LOD 100
        Cull off
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            uniform half4 _MainColor;
            // 太阳和月亮 的变量
            uniform half4 _SunColor;
            uniform half _SunRadius;
            uniform half4 _MoonColor;
            uniform half _MoonRadius;
            uniform half _MoonExposure;
            uniform half _MoonSmooth;
            uniform samplerCUBE _MoonTexCube;
            uniform half _CubeMapLOD;
            // 天空渐变色
            uniform half4 _DayC01;
            uniform half4 _DayC02;
            uniform half4 _DayC03;
            uniform half4 _NightC01;
            uniform float _HorWidth;
            uniform half4 _NightHorColor;
            uniform half4 _DayHorColor;
            // 星空
            uniform sampler _StarTex;
            uniform half4 _StarTex_ST;
            uniform sampler _StarNoise3D;
            uniform half4 _StarNoise3D_ST;
            // 银河
            uniform sampler _GalaxyNoiseTex;
            uniform half4 _GalaxyNoiseTex_ST;
            uniform sampler _GalaxyTex;
            uniform half4 _GalaxyTex_ST;
            uniform half4 _GalaxyColor1;
            uniform half4 _GalaxyColor2;
            uniform float _GalaxyRadius;
            
            struct appdata
            {
                float4 vertex : POSITION;
                float3 uv : TEXCOORD0;
                float4 texcoord : TEXCOORD1;
            };

            struct v2f
            {
                float3 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD2;
            };
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }
            
            float sphIntersect(float3 rayDir, float3 spherePos, float radius)
            {
                float3 oc = -spherePos;
                float b = dot(oc, rayDir);
                float c = dot(oc, oc) - radius * radius;
                float h = b * b - c;
                if(h < 0.0) return -1.0;
                h = sqrt(h);
                return -b - h;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                float3 uv = i.uv;
                // 不同于采样图片的UV，skybox的uv有3个轴

                // 日月绘制
                // sun
                float sunDist = distance(i.uv.xyz, _WorldSpaceLightPos0);
                float sunArea = 1 - smoothstep(0.7, 1, sunDist * _SunRadius);
                // smoothstep 测试值<=0.7 输出0；测试值>=1 输出1。因此用这种方式控制边缘厚度。
                // moon
                float3 posWS = normalize(i.worldPos);
                float3 lDir = normalize(_WorldSpaceLightPos0 - float3(0,0,0));
                // 从原点指向_WorldSpaceLightPos0的方向
                half moonIntersect = sphIntersect(posWS, lDir, _MoonRadius);
                float moonDist = distance(i.uv.xyz, -_WorldSpaceLightPos0);
                half moonMask = 1 - step(_MoonSmooth, moonDist * _MoonRadius);
                // moon 采样问题
                half3 moonNormal = normalize(lDir - posWS * moonIntersect);
                float4 moonTex = texCUBElod(_MoonTexCube, half4(moonNormal, _CubeMapLOD));
                half4 moonColor = _MoonColor * moonTex;
                half4 sunAndMoon = sunArea * _SunColor + moonMask * moonColor;

                
                // 昼夜交替天空渐变
                half4 dayColor = half4(0,0,0,1);
                half4 nightColor = half4(0,0,0,1);
                lDir = (lDir + 1) * 0.5; // 将取值范围转换到[0,1]
                if(lDir.y > 0.5 && lDir.y < 1){
                    dayColor = lerp(lerp(_DayC01, _DayC02, smoothstep(0, 0.5, lDir.z)), _DayC03, smoothstep(0.9, 1, lDir.z)) * step(0.5, lDir.y);
                }
                else{
                    //nightColor = half4(0,0,0,1);
                    nightColor = lerp(_DayC01, lerp(_NightC01, _DayC03, smoothstep(0.05, 1, lDir.z)), smoothstep(0, 0.05, lDir.z))* step(-0.5, -lDir.y);
                    // 两段渐变的写法 如果有需求可以再加几层。
                }

                
                // 地平线颜色
                float sunNightStep = smoothstep(0, 0.4, lDir.z);
                // 地平线范围
                float horLineMask = smoothstep(_HorWidth, 0, i.uv.y) * smoothstep(-_HorWidth, 0, i.uv.y);
                float3 horLineGradients = lerp(_NightHorColor, _DayHorColor, sunNightStep);
                half4 finalHorCol = half4(horLineMask * horLineGradients,1.0);

                
                // 星空
                float starTex = tex2D(_StarTex,i.uv.xz * _StarTex_ST.xy + _StarTex_ST.zw);
                float starMask = smoothstep(0.1, 0.5, i.uv.y) + 1 - smoothstep(-0.9, -0.2, i.uv.y);
                // 这个算是强行（采样）的，不关注时间，只关注位置。
                // 动态采样3D噪声
                float4 starNoiseTex = tex3D(_StarNoise3D,i.uv.xyz*_StarNoise3D_ST.x + _Time.x * 0.2);
                float starPos = smoothstep(0.21,0.31,starTex.r);
                float starBright = smoothstep(0.5,0.8,starNoiseTex.r);
                float starColor = starPos * starBright;
                //float starMask = lerp((1 - smoothstep(-0.7,-0.2,-i.uv.y)), 0, sunNightStep);

                
                // 银河
                float4 galaxyNoiseTex = tex3D(_StarNoise3D,i.uv.xyz*_StarNoise3D_ST.x + _Time.x * 0.05);
                float4 galaxy = tex2D(_GalaxyTex,(i.uv.xy + (galaxyNoiseTex - 0.5) * 0.1) * _GalaxyTex_ST.xy + _GalaxyTex_ST.zw);
                float galaxyMask = galaxy.r * (1 - moonDist * _GalaxyRadius);

                
                float4 galaxyColor = _GalaxyColor1 * galaxyMask;
                //galaxyNoiseTex = tex2D(_GalaxyNoiseTex,(i.uv.xz )*_GalaxyNoiseTex_ST.xy + _GalaxyNoiseTex_ST.zw - float2(_Time.x*0.1,_Time.x*0.1));
                //galaxyColor +=  (_GalaxyColor1 * (-galaxy.r + galaxy.g) + _GalaxyColor2 * galaxy.r) * smoothstep(0, 0.3, 1-galaxy.g);
                
                //return galaxyMask + sunAndMoon;
                return dayColor + nightColor + sunAndMoon + finalHorCol + starMask * starColor + galaxyMask * galaxyColor;
            }
            ENDCG
        }
    }
}
