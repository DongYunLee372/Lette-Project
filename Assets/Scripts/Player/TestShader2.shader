Shader "Custom/TestShader2"
{
    //Properties
    //{
    //    _MainTex("tex",2D) = "white"{}
    //    _CUBE("Cubemap",CUBE) = ""{}
    //}

    //    SubShader
    //{
    //    /*Tags { "RenderType" = "Transparent" "Opaque" = "Transparent" }
    //    LOD 200*/

    //    CGPROGRAM
    //    #pragma surface surf water alpha:blend
    //    #pragma target 3.0

    //    sampler2D _MainTex;
    //    samplerCUBE _CUBE;

    //    struct Input
    //    {
    //        float2 uv_MainTex;
    //        float3 worldRefl;
    //        INTERNAL_DATA
    //    };

    //    void surf(Input IN, inout SurfaceOutput o)
    //    {
    //        fixed4 c = tex2D(_MainTex, IN.uv_MainTex);

    //        float4 reflection = texCUBE(_CUBE, WorldReflectionVector(IN, o.Normal));
    //        o.Emission = reflection;
    //        o.Alpha = c.a;
    //    }
    //    ENDCG
    //}
    //FallBack "Diffuse"
}
