Shader "Custom/TestShader"
{
    Properties
    {
        _BumpMap("BumpMap", 2D) = "Bump"{}
        _WaveSpeed("Wave Speed", float) = 0.05
        _WavePower("Wave Power", float) = 0.2
        _WaveTilling("Wave Tilling", float) = 25

        _CubeMap("CubeMap", Cube) = ""{}

        _SpacPow("Spacular Power", float) = 2
    }

        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 200

            GrabPass{}

            CGPROGRAM

            //버텍스를 건드리기 위해서는 vertex : 함수이름 으로 추가로 작성을 해주어야 한다
            #pragma surface surf WLight vertex:vert noambient noshadow 

            #pragma target 3.0

            sampler2D _BumpMap;
            float _WaveSpeed;
            float _WavePower;
            float _WaveTilling;

            samplerCUBE _CubeMap;

            sampler2D _GrabTexture;
            float _SpacPow;

            float dotData;

            struct Input
            {
                float2 uv_BumpMap;
                float3 worldRefl;
                float4 screenPos;
                float3 viewDir;
                INTERNAL_DATA
            };

            void vert(inout appdata_full v)
            {
                v.vertex.y = sin(abs(v.texcoord.x * 2 - 1) * _WaveTilling + _Time.y) * _WavePower;
            }

            void surf(Input IN, inout SurfaceOutput o)
            {
                //노말맵의 UV를 서로 반대방향으로 움직여서 파도가 치는것처럼 보이게 해준다.
                float4 nor1 = tex2D(_BumpMap, IN.uv_BumpMap + float2(_Time.y * _WaveSpeed, 0));
                float4 nor2 = tex2D(_BumpMap, IN.uv_BumpMap - float2(_Time.y * _WaveSpeed, 0));
                o.Normal = UnpackNormal((nor1 + nor2) * 0.5);

                float4 sky = texCUBE(_CubeMap, WorldReflectionVector(IN, o.Normal));
                float4 refrection = tex2D(_GrabTexture, (IN.screenPos / IN.screenPos.a).xy + o.Normal.xy * 0.03);

                dotData = pow(saturate(1 - dot(o.Normal, IN.viewDir)), 0.6);
                float3 water = lerp(refrection, sky, dotData).rgb;

                o.Albedo = water;
            }

            float4 LightingWLight(SurfaceOutput s, float3 lightDIr, float3 viewDir, float atten)
            {
                float3 refVec = s.Normal * dot(s.Normal, viewDir) * 2 - viewDir;
                refVec = normalize(refVec);

                float spcr = lerp(0, pow(saturate(dot(refVec, lightDIr)),256), dotData) * _SpacPow;

                return float4(s.Albedo + spcr.rrr,1);
            }
            ENDCG
        }
            FallBack "Diffuse"
}
//Properties
   //{
   //    _Color ("Color", Color) = (1,1,1,1)
   //    _MainTex ("Albedo (RGB)", 2D) = "white" {}
   //    _Glossiness ("Smoothness", Range(0,1)) = 0.5
   //    _Metallic ("Metallic", Range(0,1)) = 0.0
   //}
   //SubShader
   //{
   //    Tags { "RenderType"="Opaque" }
   //    LOD 200

   //    CGPROGRAM
   //    // Physically based Standard lighting model, and enable shadows on all light types
   //    #pragma surface surf Standard fullforwardshadows

   //    // Use shader model 3.0 target, to get nicer looking lighting
   //    #pragma target 3.0

   //    sampler2D _MainTex;

   //    struct Input
   //    {
   //        float2 uv_MainTex;
   //    };

   //    half _Glossiness;
   //    half _Metallic;
   //    fixed4 _Color;

   //    // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
   //    // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
   //    // #pragma instancing_options assumeuniformscaling
   //    UNITY_INSTANCING_BUFFER_START(Props)
   //        // put more per-instance properties here
   //    UNITY_INSTANCING_BUFFER_END(Props)

   //    void surf (Input IN, inout SurfaceOutputStandard o)
   //    {
   //        // Albedo comes from a texture tinted by color
   //        fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
   //        o.Albedo = c.rgb;
   //        // Metallic and smoothness come from slider variables
   //        o.Metallic = _Metallic;
   //        o.Smoothness = _Glossiness;
   //        o.Alpha = c.a;
   //    }
   //    ENDCG
   //}
   //FallBack "Diffuse"