Shader "ThermalGame/MixedRealityEnvironment"
{
    Properties
    {
        [MainTexture] _BaseMap("Base Map", 2D) = "white" {}
        [MainColor] _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        _EnvironmentDepthBias("Environment Depth Bias", Float) = 0
        _UseEnvironmentDepth("Use Environment Depth", Float) = 1
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }
        // Match Meta's reference URP occlusion shader. Its visibility result is
        // carried through alpha so passthrough under the eye buffer is revealed.
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZWrite On

        Pass
        {
            Name "MixedRealityForward"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma target 3.0
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma multi_compile _ HARD_OCCLUSION SOFT_OCCLUSION
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.meta.xr.sdk.core/Shaders/EnvironmentDepth/URP/EnvironmentOcclusionURP.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 positionWS : TEXCOORD1;
                META_DEPTH_VERTEX_OUTPUT(2)
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float4 _BaseMap_ST;
                float _EnvironmentDepthBias;
                float _UseEnvironmentDepth;
            CBUFFER_END

            float4x4 _MRPlayfieldWorldToLocal;
            float3 _MRPlayfieldHalfExtents;
            float _MREnablePlayfieldCutout;

            Varyings Vert(Attributes input)
            {
                Varyings output;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                VertexPositionInputs positions = GetVertexPositionInputs(input.positionOS.xyz);
                output.positionCS = positions.positionCS;
                output.positionWS = positions.positionWS;
                output.uv = TRANSFORM_TEX(input.uv, _BaseMap);
                META_DEPTH_INITIALIZE_VERTEX_OUTPUT(output, input.positionOS.xyz);
                return output;
            }

            half4 Frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                if (_MREnablePlayfieldCutout > 0.5)
                {
                    float3 playfieldPosition = mul(_MRPlayfieldWorldToLocal, float4(input.positionWS, 1.0)).xyz;
                    bool insidePlayfield = all(abs(playfieldPosition) < _MRPlayfieldHalfExtents);
                    if (insidePlayfield)
                    {
                        discard;
                    }
                }

                half4 color = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv) * _BaseColor;
                if (_UseEnvironmentDepth > 0.5)
                {
                    META_DEPTH_OCCLUDE_OUTPUT_PREMULTIPLY(input, color, _EnvironmentDepthBias);
                }
                return color;
            }
            ENDHLSL
        }
    }

    FallBack Off
}
