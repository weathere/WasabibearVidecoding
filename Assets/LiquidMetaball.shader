Shader "Custom/LiquidMetaball"
{
    Properties
    {
        _MetaballTex ("轉播畫布 (Render Texture)", 2D) = "white" {}
        _Cutoff ("融合門檻 (Threshold)", Range(0.01, 1)) = 0.5
        _LiquidTex ("液體貼圖 (Liquid Texture)", 2D) = "white" {}
        _Color ("染色微調 (Color Tint)", Color) = (1, 1, 1, 1)
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
            sampler2D _LiquidTex;
            float4 _LiquidTex_ST; // 用來支援 Tiling 和 Offset
            
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
                // 1. 讀取轉播攝影機的融球光暈
                fixed4 meta = tex2D(_MetaballTex, i.uv);
                
                // 2. 如果透明度小於門檻，就隱藏（捨棄）
                if (meta.a < _Cutoff) discard;
                
                // 3. 計算液體貼圖的 UV (支援 Inspector 中的 Tiling 縮放)
                float2 liquidUV = i.uv * _LiquidTex_ST.xy + _LiquidTex_ST.zw;
                
                // 4. 讀取液體貼圖，並乘上顏色作為微調
                fixed4 finalCol = tex2D(_LiquidTex, liquidUV) * _Color;
                
                // 保持原本融球的邊緣抗鋸齒 (如果有的話)
                finalCol.a = 1.0; 
                return finalCol;
            }
            ENDCG
        }
    }
}
