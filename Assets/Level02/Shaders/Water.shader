Shader "Custom/Water"
{
    Properties
    {
        [Header(Basic Information)]
        [Space(10)]
        _ShallowWater ("表面水的颜色", Color) = (1.0, 1.0, 1.0, 1.0)
        _DeepWater ("深度水的颜色", Color) = (1.0, 1.0, 1.0, 1.0)
        _WaterDepth ("水的深度", float) = 1.0
        
        [Header(Refraction)]
        [Space(10)]
        _RefractionSpeed ("折射扭曲速度", range(0.0, 1.0)) = 1.0
        _RefractionScale ("折射扭曲幅度", range(0.0, 1.0)) = 1.0
        _NoiseTexture ("折射噪音贴图", 2D) = "black" { }
        _NoiseInfluenceX ("折射噪音影响程度X", float) = 1.0
        _NoiseInfluenceZ ("折射噪音影响程度Z", float) = 1.0
        _SceneMoveDepth ("折射噪音影响深度效果", float) = 1.0
        
        [Header(Water Splash)]
        [Space(10)]
        _FoamSpeed ("水花扭曲速度", range(0.0, 1.0)) = 1.0
        _FoamScale ("水花扭曲幅度", float) = 1.0
        _FoamAmount ("水花深度", range(0.0, 1.0)) = 1.0
        _FoamCutoff ("水花清晰程度", range(0.0, 1.0)) = 1.0
        _FoamAlpha ("水花透明度", range(0.0, 1.0)) = 1.0
        _FoamColor ("水花颜色", Color) = (1.0, 1.0, 1.0, 1.0)
        
        [Header(Shadows and PBR)]
        [Space(10)]
        _shadowStrength ("阴影衰弱", range(0.0, 1.0)) = 1.0
        _Metallic ("金属度", range(0.0, 1.0)) = 1.0
        _Roughness ("光滑度", range(0.0, 1.0)) = 1.0
        
        [Header(Noise Speed)]
        [Space(10)]
        _WaterSpeed ("水面移动速度", range(0.0, 3.0)) = 1.0
        _WaterNoiseSize ("水面噪音强度", range(0.0, 3.0)) = 1.0
        _WaterStrength ("水面扰动强度", range(0.0, 1.0)) = 1.0
        //blend Mode
        [Header(blend_Mode)]
        [Space(10)]
        [Enum(UnityEngine.Rendering.BlendMode)]_SrcBlend ("Src Blend", float) = 1
        [Enum(UnityEngine.Rendering.BlendMode)]_DstBlend ("Dst Blend", float) = 1
        //Z write
        [Header(Z_write)]
        [Space(10)]
        [Enum(Off, 0, On, 1)] _ZWrite ("Z Write", Float) = 1
        [Enum(Off, 0, On, 1)] _CullMode ("Cull Mode", Float) = 1
        _NormalScale ("法线强度", Range(0, 1)) = 1
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" "RenderPipeline" = "UniversalRenderPipeline" "IgnoreProjector" = "True" "Queue" = "Transparent" }
        LOD 100

        Pass
        {
            Blend[_SrcBlend][_DstBlend]
            ZWrite[_ZWrite]
            Cull[_CullMode]
            HLSLPROGRAM

            #pragma target 3.5

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareOpaqueTexture.hlsl"
            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE

            float4 _ShallowWater;
            float4 _DeepWater;
            float _WaterDepth;
            float _RefractionSpeed;
            float _RefractionScale;
            float _NoiseInfluenceX;
            float _NoiseInfluenceZ;
            float _SceneMoveDepth;
            float _FoamSpeed;
            float _FoamScale;
            float _FoamCutoff;
            float _FoamAmount;
            float _FoamAlpha;
            float4 _FoamColor;
            TEXTURE2D(_NoiseTexture);SAMPLER(sampler_NoiseTexture);
            float _NormalScale;
            float _shadowStrength;
            float _Roughness;
            float _Metallic;
            float _WaterSpeed;
            float _WaterNoiseSize;
            float _WaterStrength;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
                float4 tangent : TANGENT;
            };

            struct v2f
            {
                float3 normalWS : TEXCOORD0;
                float3 tangentWS : TANGENT;
                float3 bitangentWS : TEXCOORD1;
                float4 vertex : SV_POSITION;
                float3 vertex_world : TEXCOORD2;
                float2 uv : TEXCOORD3;
            };

            //-----------------Depth
            float4 ComputeScreenPos(float4 pos, float projectionSign)
            {
                float4 o = pos * 0.5f;
                o.xy = float2(o.x, o.y * projectionSign) + o.w;
                o.zw = pos.zw;
                return o;
            }

            float CustomSampleSceneDepth(float2 uv)
            {
                return SAMPLE_TEXTURE2D_X_LOD(_CameraDepthTexture, sampler_CameraDepthTexture, UnityStereoTransformScreenSpaceTex(uv),1.0).r;
            }

            float GetDepthFade(float3 WorldPos, float Distance)
            {

                float4 ScreenPosition = ComputeScreenPos(TransformWorldToHClip(WorldPos), _ProjectionParams.x);
                float EyeDepth = LinearEyeDepth(CustomSampleSceneDepth(ScreenPosition.xy / ScreenPosition.w), _ZBufferParams);
                return saturate((EyeDepth - ScreenPosition.a) / Distance);
            }
            //-----------------Depth

            //-----------------UVMovement
            float2 UVMovement(float2 uv, float Speed, float Scale)
            {
                float2 newUV = uv * Scale + (_Time.y * Speed).rr;
                return newUV;
            }
            //-----------------UVMovement

            //-----------------SceneColor
            float3 GetSceneColor(float3 WorldPos, float2 uv_move)
            {
                float4 ScreenPosition = ComputeScreenPos(TransformWorldToHClip(WorldPos), _ProjectionParams.x);
                return SampleSceneColor((ScreenPosition.xy + uv_move) / ScreenPosition.w);
            }
            //-----------------SceneColor
            //DGF
            float DistributionGGX(float NoH, float a)
            {
                float a2 = a * a;
                float NoH2 = NoH * NoH;

                float nom = a2;
                float denom = NoH2 * (a2 - 1) + 1;
                denom = denom * denom * PI;
                return nom / denom;
            }

            float GeometrySchlickGGX(float NoV, float k)
            {
                float nom = NoV;
                float denom = NoV * (1.0 - k) + k;
                return nom / denom;
            }

            float GeometrySmith(float NoV, float NoL, float k)
            {
                float ggx1 = GeometrySchlickGGX(NoV, k);
                float ggx2 = GeometrySchlickGGX(NoL, k);
                return ggx1 * ggx2;
            }

            float3 FresnelSchlick(float cosTheta, float3 F0)
            {
                return F0 + pow(1.0 - cosTheta, 5.0);
            }

            float3 IndirFresnelSchlick(float cosTheta, float3 F0, float roughness)
            {
                return F0 + (max(float3(1, 1, 1) * (1 - roughness), F0) - F0) * pow(1.0 - cosTheta, 5.0);
            }

            float3 SH_Process(float3 N)
            {
                float4 SH[7];
                SH[0] = unity_SHAr;
                SH[1] = unity_SHAg;
                SH[2] = unity_SHAb;
                SH[3] = unity_SHBr;
                SH[4] = unity_SHBr;
                SH[5] = unity_SHBr;
                SH[6] = unity_SHC;

                return max(0.0, SampleSH9(SH, N));
            }

            v2f vert(appdata v)
            {
                v2f o;

                float2 NoiseUV = v.uv * _WaterNoiseSize + (_WaterSpeed * _Time.y).rr;//噪音图基础UV
                o.vertex_world = TransformObjectToWorld(v.vertex.xyz);//世界空间的顶点
                float ReflactionSceneMoveDepth = GetDepthFade(o.vertex_world, _SceneMoveDepth);//获得边缘深度图
                float noise = (SAMPLE_TEXTURE2D_LOD(_NoiseTexture, sampler_NoiseTexture,NoiseUV,1.0).r - 0.5f )* saturate(1.0 - saturate(ReflactionSceneMoveDepth));//获得造影
                v.vertex.y += noise * _WaterStrength;//对顶点的Y坐标进行位移
                
                o.vertex = TransformObjectToHClip(v.vertex);
                o.normalWS = normalize(TransformObjectToWorldNormal(v.normal.xyz));
                o.tangentWS = normalize(TransformObjectToWorldDir(v.tangent.xyz));
                o.bitangentWS = cross(o.normalWS, o.tangentWS) * v.tangent.w * unity_WorldTransformParams.w;
                o.uv = v.uv;
                return o;
            }

            real4 frag(v2f i) : SV_Target
            {
                //Main Color
                float depthfade = GetDepthFade(i.vertex_world, _WaterDepth);
                float4 WaterColor = lerp(_ShallowWater, _DeepWater, depthfade);
                //Main Color

                //Scene Color
                float2 ReflactionMoveUV = UVMovement(i.uv, _RefractionSpeed, _RefractionScale);
                float ReflactionNoise = SAMPLE_TEXTURE2D(_NoiseTexture, sampler_NoiseTexture, ReflactionMoveUV).r;
                float ReflactionSceneMoveDepth = GetDepthFade(i.vertex_world, _SceneMoveDepth);
                float3 SceneColor = GetSceneColor(i.vertex_world, float2(ReflactionNoise * _NoiseInfluenceX, ReflactionNoise * _NoiseInfluenceZ) * ReflactionSceneMoveDepth);
                //Scene Color

                //Water Filter
                float2 FilterMoveUV = UVMovement(i.uv, _FoamSpeed, _FoamScale);
                float FilterNoise = SAMPLE_TEXTURE2D(_NoiseTexture, sampler_NoiseTexture, FilterMoveUV).r;
                float FilterSceneMoveDepth = GetDepthFade(i.vertex_world, _FoamAmount) * _FoamCutoff;
                float WaterFilter = step(FilterSceneMoveDepth, FilterNoise) * _FoamAlpha;
                //Water Filter

                //Light
                float4 SHADOW_COORDS = TransformWorldToShadowCoord(i.vertex_world);
                Light mainLight = GetMainLight(SHADOW_COORDS);
                half shadow = MainLightRealtimeShadow(SHADOW_COORDS);
                shadow = lerp(shadow, 1.0, _shadowStrength);
                //Shadow 阴影RT采样

                float4 NormalDataUnProcess = float4(ReflactionNoise * 0.5f, ReflactionNoise * 0.5f, 1.0f, 1.0f);
                float3 Normaldata = UnpackNormal(NormalDataUnProcess);
                
                float3 position = i.vertex_world;
                float3 N = normalize(Normaldata.x * i.tangentWS * _NormalScale + Normaldata.y * i.bitangentWS * _NormalScale + i.normalWS);
                float3 V = normalize(_WorldSpaceCameraPos - i.vertex_world);
                float3 L = mainLight.direction;
                float3 H = normalize(V + L);
                float3 R = reflect(-V, N);

                float3 IndirSpecularBaseColor = SAMPLE_TEXTURECUBE_LOD(unity_SpecCube0, samplerunity_SpecCube0, R, 1.0).rgb;
                float3 F0 = lerp(0.04, IndirSpecularBaseColor, _Metallic);

                float NoV = max(saturate(dot(N, V)), 0.000001);
                float NoL = max(saturate(dot(N, L)), 0.000001);
                float HoV = max(saturate(dot(H, V)), 0.000001);
                float NoH = max(saturate(dot(H, N)), 0.000001);
                float LoH = max(saturate(dot(H, L)), 0.000001);

                //DGF
                float D = DistributionGGX(NoH, _Roughness);
                float k = pow(1 + _Roughness, 2) / 8;
                float G = GeometrySmith(NoV, NoL, k);
                float3 F = FresnelSchlick(LoH, F0);

                //直接高光项
                float3 specular = D * G * F / (4 * NoV * NoL);

                //直接漫反射项
                float3 ks = F;
                float3 kd = (1 - ks) * (1 - _Metallic);
                float3 diffuse = kd * IndirSpecularBaseColor / PI;
                float3 DirectColor = (diffuse + specular) * NoL * PI * mainLight.color;
                //Light

                float4 final_color = lerp(WaterColor, _FoamColor, WaterFilter);
                final_color.rgb = lerp(SceneColor, final_color.rgb, final_color.a) + DirectColor;

                //return float4(specular.rgb, 1.0);
                return float4(shadow * final_color.rgb, 1.0);
            }
            ENDHLSL

        }
    }
}