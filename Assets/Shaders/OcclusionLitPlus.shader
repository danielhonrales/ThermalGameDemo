// URP PBR with normal map and emission, occluded by Meta environment depth so real
// people, hands and furniture correctly hide virtual drones and sudden-death cover.
Shader "ThermalGame/OcclusionLitPlus"
{
    Properties
    {
        _BaseColor ("Color", Color) = (1,1,1,1)
        _BaseMap ("Albedo", 2D) = "white" {}
        [Normal] _BumpMap ("Normal Map", 2D) = "bump" {}
        _BumpScale ("Normal Scale", Float) = 1
        _Metallic ("Metallic", Range(0,1)) = 0
        _Smoothness ("Smoothness", Range(0,1)) = 0.5
        _MetallicGlossMap ("Metallic (R) Smoothness (A)", 2D) = "white" {}
        [HDR] _EmissionColor ("Emission", Color) = (0,0,0,1)
        _EmissionMap ("Emission Map", 2D) = "white" {}
        _EnvironmentDepthBias ("Environment Depth Bias", Float) = 0.0
    }

    SubShader
    {
        PackageRequirements {"com.unity.render-pipelines.universal"}
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" "Queue"="Geometry" }

        Pass
        {
            Tags { "LightMode" = "UniversalForward" }
            Blend One OneMinusSrcAlpha, One OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile _ _CLUSTER_LIGHT_LOOP
            #pragma multi_compile_fragment _ _SHADOWS_SOFT
            #pragma multi_compile _ HARD_OCCLUSION SOFT_OCCLUSION
            #pragma multi_compile_instancing
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #include "Packages/com.meta.xr.sdk.core/Shaders/EnvironmentDepth/URP/EnvironmentOcclusionURP.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 posWorld : TEXCOORD1;
                float3 normalWS : TEXCOORD2;
                float4 tangentWS : TEXCOORD3;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

            TEXTURE2D(_BaseMap); SAMPLER(sampler_BaseMap);
            TEXTURE2D(_BumpMap); SAMPLER(sampler_BumpMap);
            TEXTURE2D(_MetallicGlossMap); SAMPLER(sampler_MetallicGlossMap);
            TEXTURE2D(_EmissionMap); SAMPLER(sampler_EmissionMap);

            CBUFFER_START(UnityPerMaterial)
            float4 _BaseColor;
            float4 _BaseMap_ST;
            float4 _EmissionColor;
            float _BumpScale;
            float _Metallic;
            float _Smoothness;
            float _EnvironmentDepthBias;
            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
                VertexPositionInputs vertexInputs = GetVertexPositionInputs(input.positionOS.xyz);
                VertexNormalInputs normalInputs = GetVertexNormalInputs(input.normalOS, input.tangentOS);
                output.positionCS = vertexInputs.positionCS;
                output.uv = TRANSFORM_TEX(input.uv, _BaseMap);
                output.normalWS = normalInputs.normalWS;
                output.tangentWS = float4(normalInputs.tangentWS, input.tangentOS.w * GetOddNegativeScale());
                output.posWorld = vertexInputs.positionWS;
                META_DEPTH_INITIALIZE_VERTEX_OUTPUT(output, input.positionOS.xyz);
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                float4 albedo = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv) * _BaseColor;
                float3 normalTS = UnpackNormalScale(SAMPLE_TEXTURE2D(_BumpMap, sampler_BumpMap, input.uv), _BumpScale);
                float3 bitangent = input.tangentWS.w * cross(input.normalWS, input.tangentWS.xyz);
                float3 normalWS = normalize(TransformTangentToWorld(normalTS,
                    half3x3(input.tangentWS.xyz, bitangent, input.normalWS)));

                InputData lighting = (InputData)0;
                lighting.positionWS = input.posWorld;
                lighting.normalWS = normalWS;
                lighting.viewDirectionWS = GetWorldSpaceNormalizeViewDir(input.posWorld);
                lighting.shadowCoord = TransformWorldToShadowCoord(input.posWorld);
                lighting.bakedGI = SampleSH(normalWS);
                lighting.positionCS = input.positionCS;
                lighting.normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(input.positionCS);

                SurfaceData surface = (SurfaceData)0;
                surface.albedo = albedo.rgb;
                surface.alpha = 1.0;
                float4 metalGloss = SAMPLE_TEXTURE2D(_MetallicGlossMap, sampler_MetallicGlossMap, input.uv);
                surface.metallic = _Metallic * metalGloss.r;
                surface.smoothness = _Smoothness * metalGloss.a;
                surface.emission = _EmissionColor.rgb * SAMPLE_TEXTURE2D(_EmissionMap, sampler_EmissionMap, input.uv).rgb;
                surface.occlusion = 1.0;

                half4 color = UniversalFragmentPBR(lighting, surface);
                META_DEPTH_OCCLUDE_OUTPUT_PREMULTIPLY(input, color, _EnvironmentDepthBias);
                return color;
            }
            ENDHLSL
        }
    }
    FallBack "Universal Render Pipeline/Lit"
}
