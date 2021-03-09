// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Shader1"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ColorOverlay ("_ColorOverlay", Color) = (1,1,1,1)
        _TextureMultiplier ("TextureMultiplier", float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue" = "Transparent"}
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _ColorOverlay;
            float _TextureMultiplier;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = _TextureMultiplier * v.uv;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float4 col = tex2D(_MainTex, i.uv);

                col *= float4(i.uv.x, i.uv.y, 3 * i.uv.g, 1);
                return col * _ColorOverlay;
            }
            ENDCG
        }
    }
}
