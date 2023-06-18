Shader "Custom/Chess" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _ColorA ("Color A", Color) = (1, 1, 1, 1)
        _ColorB ("Color B", Color) = (0, 0, 0, 1)

        [IntRange]
        _TilesPerSide ("Tiles Per Side", Range(1, 100)) = 10
    }

    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 100

        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D _MainTex;
        fixed4 _ColorA;
        fixed4 _ColorB;
        float _TilesPerSide;

        struct Input {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutput o) {
            float2 uv = IN.uv_MainTex * _TilesPerSide * 2.0;
            float2 tileUV = floor(uv) + 0.5;
            float2 tileIndex = floor(uv * 0.5);

            fixed4 color;

            if (fmod(tileIndex.x + tileIndex.y, 2) < 1.0) {
                color = _ColorA;
            } else {
                color = _ColorB;
            }

            o.Albedo = color.rgb;
            o.Alpha = color.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}