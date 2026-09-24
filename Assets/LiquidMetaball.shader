Shader "Custom/LiquidMetaball"
{
    Properties
    {
        _MetaballTex ("Render Texture", 2D) = "white" {}
        _Cutoff ("融合門檻 (Threshold)", Range(0.01, 1)) = 0.5
        _Color ("液體顏色 (Liquid Color)", Color) = (0, 0.4, 0.1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t { float4 vertex : POSITION; float2 uv : TEXCOORD0; };
            struct v2f { float2 uv : TEXCOORD0; float4 vertex : SV_POSITION; };

            sampler2D _MetaballTex;
            float _Cutoff;
            fixed4 _Color;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MetaballTex, i.uv);
                // 魔法核心：只要光暈重疊的透明度大於門檻，就變成實心顏色，否則直接隱藏
                if (col.a < _Cutoff) discard;
                return _Color;
            }
            ENDCG
        }
    }
}
