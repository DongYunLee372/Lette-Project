Shader "Custom/TestShader2"
{
    Properties
    {
        _BumpTex("BumpTex", 2D) = "Bump"{}
        _MainTex("tex",2D) = "white"{}
        _CUBE("Cubemap",CUBE) = ""{}
    }

        SubShader
        {
            Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
            LOD 200

            CGPROGRAM
            //#pragma surface surf Standard fullforwardshadows
            #pragma surface surf water alpha:blend noambient vertex:vert
            #pragma target 3.0
            
            sampler2D _BumpTex;
            sampler2D _MainTex;
            samplerCUBE _CUBE;

            struct Input
            {
                float2 uv_MainTex;
                float2 uv_BumpTex;
                float3 worldRefl;
                INTERNAL_DATA
            };

            /*void surf(Input IN, inout SurfaceOutput o)
            {
                fixed4 c = tex2D(_MainTex, IN.uv_MainTex);

                float4 reflection = texCUBE(_CUBE, WorldReflectionVector(IN, o.Normal));
                o.Emission = reflection;
                o.Alpha = c.a;
            }*/

            void vert(inout appdata_full v)
            {
                //v.vertex.z += v.texcoord.x;
                v.vertex.z += cos(abs(v.texcoord.x * 2 - 1)*10/*파도간격*/ +_Time.y/*파도속도*/)*0.5/*파도높이*/;//하프렘버트 역공식과 삼각함수 적용
            }

            void surf(Input IN, inout SurfaceOutput o)
            {
                fixed4 c = tex2D(_MainTex, IN.uv_MainTex);

                float3 normal1 = UnpackNormal(tex2D(_BumpTex, IN.uv_BumpTex + _Time.y * 0.01));
                float3 normal2 = UnpackNormal(tex2D(_BumpTex, IN.uv_BumpTex - _Time.y * 0.01));
                o.Normal = (normal1 + normal2) * 0.5;
                //o.Normal *= float3(0.5, 0.5, 1);


                float4 reflection = texCUBE(_CUBE, WorldReflectionVector(IN, o.Normal));
                o.Emission = reflection*1.05;
                o.Alpha = 1;
            }

            /*float4 Lightingwater(SurfaceOutput s, float3 lightDir, float3 viewDir, float atten)
            {
                float rim = saturate(dot(s.Normal, viewDir));
                rim = pow(1 - rim, 3);
                return float4(rim, rim, rim, 1);
            }*/

            float4 Lightingwater(SurfaceOutput s, float3 lightDir, float3 viewDir, float atten)
            {
                float rim = saturate(dot(s.Normal, viewDir));

                float rim1 = pow(1 - rim, 20);//기울어지면 밝아짐
                float rim2 = pow(1 - rim, 2);//프레넬 마스킹용(알파)

                float4 final = rim1 * _LightColor0;//라이트받아오기

                //return final;
                return float4(final.rgb, rim2);
            }
            ENDCG
        }
    FallBack "Diffuse"
}
