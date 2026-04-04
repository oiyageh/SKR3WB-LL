Shader "Custom/SkyboxBlend"
{
    Properties
    {
        _TexA ("Sky A", CUBE) = "" {}
        _TexB ("Sky B", CUBE) = "" {}
        _Blend ("Blend", Range(0,1)) = 0
    }

    SubShader
    {
        Tags { "Queue"="Background" "RenderType"="Opaque" }
        Cull Off ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            samplerCUBE _TexA;
            samplerCUBE _TexB;
            float _Blend;

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 texcoord : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.vertex.xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 colA = texCUBE(_TexA, i.texcoord);
                fixed4 colB = texCUBE(_TexB, i.texcoord);
                return lerp(colA, colB, _Blend);
            }
            ENDCG
        }
    }
}