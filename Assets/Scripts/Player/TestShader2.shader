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
        //Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        Tags{"RenderType" = "Opaque"}

        LOD 200

        GrabPass{}

        CGPROGRAM
        //#pragma surface surf Standard fullforwardshadows
        #pragma surface surf water /*alpha:blend*/ noambient vertex:vert
        #pragma target 3.0



        sampler2D _GrabTexture;

        sampler2D _BumpTex;
        sampler2D _MainTex;
        samplerCUBE _CUBE;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpTex;
            float3 worldRefl;
            float3 viewDir;
            float4 screenPos;

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
            v.vertex.z += cos(abs(v.texcoord.x * 2 - 1) * 10/*파도간격*/ + _Time.y/*파도속도*/) * 0.2/*파도높이*/;//하프렘버트 역공식과 삼각함수 적용
        }

        void surf(Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);

            float3 normal1 = UnpackNormal(tex2D(_BumpTex, IN.uv_BumpTex + _Time.y * 0.01));
            float3 normal2 = UnpackNormal(tex2D(_BumpTex, IN.uv_BumpTex - _Time.y * 0.01));
            o.Normal = (normal1 + normal2) * 0.5;

            //o.Normal *= float3(0.5, 0.5, 1);


            float4 reflection = texCUBE(_CUBE, WorldReflectionVector(IN, o.Normal));
            //o.Emission = reflection * 1.05;

            //추가

            float rim = saturate(dot(o.Normal, IN.viewDir));

            float rim1 = pow(1 - rim, 50) * 0.8;//기울어지면 밝아짐
            float rim2 = pow(1 - rim, 4);//프레넬 마스킹용(알파)

            float3 ScreenUV = IN.screenPos.rgb / IN.screenPos.a;

            //o.Emission = IN.screenPos.rgb / IN.screenPos.a;
            //o.Emission = tex2D(_GrabTexture, ScreenUV+o.Normal.xy*0.03)*0.5;

            float4 grabtex = tex2D(_GrabTexture, ScreenUV + o.Normal.xy * 0.03) * 0.5;
            o.Emission = lerp(grabtex, reflection, rim2) + (rim1 * _LightColor0);

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
            float3 H = normalize(lightDir + viewDir);
            float spec = saturate(dot(s.Normal, H));
            spec = pow(spec, 1050) * 10;

            float rim = saturate(dot(s.Normal, viewDir));

            float rim1 = pow(1 - rim, 20);//기울어지면 밝아짐
            float rim2 = pow(1 - rim, 2);//프레넬 마스킹용(알파)

            //float4 final = rim1 * _LightColor0;//라이트받아오기
            //float4 final = saturate(rim1 + spec) * _LightColor0;//라이트받아오기

            float4 final;
            final.rgb = spec * _LightColor0;
            final.a = s.Alpha + spec;
            //final.a = saturate(rim2 + spec);
            return final;
            //return final;
            //return float4(final.rgb, rim2);
            //return float4(final.rgb, saturate(rim2 + spec));
        }
        ENDCG
    }
        FallBack "Diffuse"
}