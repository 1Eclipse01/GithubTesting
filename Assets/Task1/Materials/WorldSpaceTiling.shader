Shader "Custom/WorldSpaceTiling"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}   // default to white texture
        _Tiling   ("Tiling",   Float) = 1       // default to normal tile scale
        _Color ("Color", Color) = (1,1,1,1)     // colour tint to apply
    }

    SubShader
    {
        Tags
        {
            "RenderType"      = "Opaque"
            "RenderPipeline"  = "UniversalPipeline"  // the setting for a URP shader
            "Queue"           = "Geometry"
        }

        Pass            // PASS 1 — Forward lit pass (lighting + shadow receiving)
        {
            Name "ForwardLit" 
            Tags { "LightMode" = "UniversalForward" } 

            HLSLPROGRAM                                 // URP uses HLSL, not CGPROGRAM
            #pragma vertex   vert                       // define which function is used for vertex shading
            #pragma fragment frag                       // define which function is used for fragment (pixel) shading

            // Generates various URP keywords we need
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _SHADOWS_SOFT

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f      // vertex to fragment interpolation data
            {
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
            };

            TEXTURE2D(_MainTex);        // declare main texture
            SAMPLER(sampler_MainTex);   // declare texture sampler

            CBUFFER_START(UnityPerMaterial) // for batching in URP
                float4 _MainTex_ST;
                float _Tiling;
                half4 _Color;
            CBUFFER_END

            v2f vert(appdata v) // vertex shader
            {
                v2f o;
                o.worldPos    = TransformObjectToWorld(v.vertex.xyz);       // get world space location of vertex
                o.pos         = TransformObjectToHClip(v.vertex.xyz);       // get clip space (world / MVP matrix) location
                o.worldNormal = TransformObjectToWorldNormal(v.normal);     // set up normals in world space
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                // Triplanar blend weights
                float3 blend = abs(i.worldNormal);
                blend /= max(blend.x + blend.y + blend.z, 0.0001);

                float3 texX = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.worldPos.yz * _Tiling * _MainTex_ST.xy).rgb;
                float3 texY = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.worldPos.xz * _Tiling * _MainTex_ST.xy).rgb;
                float3 texZ = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.worldPos.xy * _Tiling * _MainTex_ST.xy).rgb;

                float3 col = texX * blend.x + texY * blend.y + texZ * blend.z;

                //______ Lighting ________
                float4 shadowCoord = TransformWorldToShadowCoord(i.worldPos);
                Light mainLight = GetMainLight(shadowCoord);
                half ndotl = max(0, dot(normalize(i.worldNormal), mainLight.direction));
                half3 lighting = mainLight.color * (ndotl * mainLight.shadowAttenuation);  // shadowAttenuation is scaled 0-1
                half3 ambient = half3(unity_AmbientSky.rgb);    // Apply Unity's ambient lighting settings

                col = col * (lighting + ambient);
                col *= _Color.rgb; // apply the tint
                return half4(col, 1);
            }
            ENDHLSL
        }

        Pass            // PASS 2 — Shadow casting onto other objects
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" } 
            ZWrite On
            ZTest LEqual
            ColorMask 0          // shadow pass only writes depth, not colour

            HLSLPROGRAM
            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment
            #pragma multi_compile_shadowcaster

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl"
            ENDHLSL
        }
    }

    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}