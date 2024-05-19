Shader "Custom/CutoutMask"
{
    Properties
    {
        _MainTex ("RGB Texture", 2D) = "white" {}
        _AlphaTex ("Alpha Texture", 2D) = "white" {}
        _Cutoff ("Alpha Cutoff", Range(0,1)) = 0.5
        _Color ("Tint Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 texcoord : TEXCOORD0;
                UNITY_FOG_COORDS(1)
            };

            sampler2D _MainTex;
            sampler2D _AlphaTex;
            float4 _MainTex_ST;
            float4 _AlphaTex_ST;
            float _Cutoff;
            float4 _Color;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                UNITY_TRANSFER_FOG(o, o.pos);
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                // Sample the RGB texture
                half4 rgb = tex2D(_MainTex, i.texcoord);

                // Sample the Alpha texture
                half alpha = tex2D(_AlphaTex, i.texcoord).r;

                // Discard the fragment if the alpha value is below the cutoff threshold
                clip(alpha - _Cutoff);

                // Combine RGB and Alpha, and apply the tint color
                half4 outputColor = half4(rgb.rgb * _Color.rgb, alpha);
                return outputColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}