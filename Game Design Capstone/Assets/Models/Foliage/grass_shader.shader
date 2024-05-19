Shader "Custom/grass_shader"
{
    Properties
    {
        _MainTex ("Grass Texture", 2D) = "white" {}
        _Cutoff ("Alpha Cutoff", Range(0,1)) = 0.5
    }

    SubShader
    {
        Cull Off // Disable backface culling

        Tags { "RenderType" = "Opaque" }

        CGPROGRAM
        #pragma surface surf Standard //fullforwardshadows //alpha:blend

        struct Input
        {
            float2 uv_MainTex;
        };

        sampler2D _MainTex;
        half _Cutoff;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Sample the grass texture
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);

            // Apply alpha cutoff
            if (c.a < _Cutoff)
                discard;

            // Output the sampled color
            o.Albedo = c.rgb;
            // o.Alpha = 0;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
