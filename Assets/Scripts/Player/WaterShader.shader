Shader "Custom/WaterShader"
{
    Properties
    {
        //파도 노말 텍스쳐
        _BumpTex("BumpTex", 2D) = "Bump"{}
        //메인 텍스쳐
        _MainTex ("tex", 2D) = "white" {}
        //하늘이 반사되는걸 구현할때 사용할 큐브맵, 현재 스카이 박스와 같은 텍스쳐를 넣어준다.
        _CUBE("Cubemap",CUBE) = ""{}
    }

    SubShader
    {
        //Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        Tags{"RenderType" = "Opaque"}
        LOD 200

        GrabPass{}

        CGPROGRAM
        #pragma surface surf water /*alpha:blend*/ vertex:vert//surface함수 serf, CustomLight함수 water, vertex함수 vert
        #pragma target 3.0

        sampler2D _GrabTexture;
        sampler2D _MainTex;
        samplerCUBE _CUBE;

        //파도 텍스쳐
        sampler2D _BumpTex;

        struct Input
        {
            //메인 텍스쳐 UV
            float2 uv_MainTex;
            //월드 반사 벡터
            float3 worldRefl;
            //파도 노말 텍스쳐 UV
            float2 uv_BumpTex;
            //림 연산을 위해 사용
            float3 viewDir;
            //반사 또는 스크린 공간 효과를 위한 스크린 UV
            float4 screenPos;


            INTERNAL_DATA//반사벡터 노멀처리, WorldReflectionVector 함수 사용을 위한 메크로 선언
        };

        void vert(inout appdata_full v)
        {
            v.vertex.z += cos(abs(v.texcoord.x * 2 - 2) * 10/*파도간격*/ + _Time.y/*파도속도*/) * 0.1/*1.5*//*파도높이*/;//하프렘버트 역공식과 삼각함수 적용
        }


        void surf (Input IN, inout /*SurfaceOutputStandard*/SurfaceOutput o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);

            /*잔물결 구현 부분*/
            //잔물결을 구현하기 위해 노말맵의 UV좌표를 시간에따라 양 방향에서 가운데로 모이도록 흘러가게 해준다.
            float3 normal1 = UnpackNormal(tex2D(_BumpTex, IN.uv_BumpTex + _Time.y * 0.01));
            float3 normal2 = UnpackNormal(tex2D(_BumpTex, IN.uv_BumpTex - _Time.y * 0.01));
            //0.5를 곱해주는 이유는 합 연산 으로 인해 최대값이 2로 늘었기 때문에 해준것이다.
            o.Normal = (normal1 + normal2) * 0.5;

            //WorldReflectionVector 함수를 이용해 픽셀당 노멀을 기반으로 반사 벡터를 얻어온 다음에
            //texCUBE함수를 이용해 반사되어 보이는 최종 색상을 알아와서 적용해준다.
            float4 reflection = texCUBE(_CUBE, WorldReflectionVector(IN, o.Normal));

 
            /*프레넬 반사 구현 부분*/
            //표면의 노말과 뷰벡터를 내적해서 두개의 벡터가 이루는 각의 cos값을 알아낸다.
            float rim = saturate(dot(o.Normal, IN.viewDir));
            //이렇게 만들어진 rim1값은 이루는각이 0도에 가까울수록 0에 가까워지고 90도에 가까워질수록 1에 가까워진다.
            //따라서 바로 발 아래 부분을 보면 투명하게 보이고 먼곳을 바라볼수록 하늘이 반사된 수면이 보이게 된다. 
            float rim1 = pow(1 - rim, 20);
            //이 값을 알파로 적용함으로써 가까운곳은 Rim이 연하게 들어가고 멀리있는것은 쎄게 들어간다.
            float rim2 = pow(1 - rim, 2);


            /*수면 아래 굴절 구현 부분*/
            //스크린 좌표는 기본적으로 Perspective이기 때문에 Vector의 w성분을 나눠줌으로써 Orthographic으로 바꿔준다.
            float3 ScreenUV = IN.screenPos.rgb / IN.screenPos.a;

            //GrabPass를 이용해서 캡쳐한 화면을 tex2D를 이용해서 뽑아오고
            float4 grabtex = tex2D(_GrabTexture, ScreenUV + o.Normal.xy * 0.03) * 0.5;

            //물에의한 외곡, 림라이트, 잔물결 노말 등을 모두 종합해서 적용시켜 준다.
            o.Emission = lerp(grabtex, reflection, rim2) + (rim1 * _LightColor0);


            //o.Emission = reflection;

            //float3 reflection = WorldReflectionVector(IN, o.Normal).xyz;
            //o.Emission = UNITY_SAMPLE_TEXCUBE(unity_SpecCube0, reflection).rgb * unity_SpecCube0_HDR.r;
            

            o.Alpha = 1;
        }


        //커스텀 라이트
        
        float4 Lightingwater(SurfaceOutput s, float3 lightDir, float3 viewDir, float atten)
        {
            /*Speculer적용부분*/

            //speculer는 Biln-Phong Shading에서의 speculer구하는 공식을 이용한다.
            //빛의 방향과 view방향을 더해서 HalfVector를 구하고
            float3 H = normalize(lightDir + viewDir);
            //해당 벡터와 노말벡터를 내적해서 Speculer값을 구한다.
            float spec = saturate(dot(s.Normal, H));
            //강도조절
            spec = pow(spec, 1050) * 10;

            ///*프레넬 반사 적용부분*/
            ////표면의 노말과 뷰벡터를 내적해서 두개의 벡터가 이루는 각의 cos값을 알아낸다.
            //float rim = saturate(dot(s.Normal, viewDir));
            ////이렇게 만들어진 rim1값은 이루는각이 0도에 가까울수록 0에 가까워지고 90도에 가까워질수록 1에 가까워진다.
            ////따라서 바로 발 아래 부분을 보면 투명하게 보이고 먼곳을 바라볼수록 하늘이 반사된 수면이 보이게 된다. 
            //float rim1 = pow(1 - rim, 20);
            ////이 값을 알파로 적용함으로써 가까운곳은 Rim이 연하게 들어가고 멀리있는것은 쎄게 들어간다.
            //float rim2 = pow(1 - rim, 2);

            //float4 final = saturate(rim1 + spec) * _LightColor0;

            //return float4(final.rgb, saturate(rim2+spec));

            float4 final;
            final.rgb = spec * _LightColor0;
            final.a = s.Alpha + spec;
            return final;
        }
        
        ENDCG
    }
    FallBack "Diffuse"
}
