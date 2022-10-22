Shader "Custom/NewSurfaceShader"
{
    Properties
    {
        
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _CUBE("Cubemap",CUBE) = ""{}
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows
        //#pragma surface surf water alpha:blend   
        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        samplerCUBE _CUBE;
        struct Input
        {
            float2 uv_MainTex;
            float3 worldRefl;
            INTERNAL_DATA
        };

       

        

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);

            float4 reflection = texCUBE(_CUBE, WorldReflectionVector(IN, o.Normal));
            o.Emission = reflection;
            o.Alpha = c.a;
        }
        float4 Lightingwater(SurfaceOutput s, float3 lightDir, float3 viewDir, float atten)
        {
            // Rim
            float rim = saturate(dot(s.Normal, viewDir));
            rim = pow(1 - rim, 3);

            float4 final = rim * _LightColor0;

            return final;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
