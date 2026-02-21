Shader "MonsterTamer/UI/Transitions/BattleFlash"
{
     Properties
    {
        _Color ("Flash Color", Color) = (1,1,1,1)
        _FlashStrength ("Flash Strength", Range(0,1)) = 0
    }
    SubShader
    {
        Tags { "Queue"="Overlay" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off ZWrite Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float4 _Color;
            float _FlashStrength;

            struct appdata { float4 vertex : POSITION; float2 uv : TEXCOORD0; };
            struct v2f { float4 vertex : SV_POSITION; };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Output plain flash color with variable intensity
                fixed4 col = _Color;
                col.a *= _FlashStrength;
                return col;
            }
            ENDHLSL
        }
    }
}
