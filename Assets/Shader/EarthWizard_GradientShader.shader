Shader "Custom/GradientShaderWithMetallic"
{
    Properties
    {
        _ColorTop ("Top Color", Color) = (1, 1, 1, 1)
        _ColorMiddle ("Middle Color", Color) = (0.5, 0.5, 0.5, 1)
        _ColorBottom ("Bottom Color", Color) = (0, 0, 0, 1)
        _TopHeight ("Top Height", Float) = 1
        _MiddleHeight ("Middle Height", Float) = 0.5
        _BottomHeight ("Bottom Height", Float) = 0
        _Metallic ("Metallic", Range(0, 1)) = 0.5
        _Smoothness ("Smoothness", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #include "UnityCG.cginc"

        struct Input
        {
            float3 worldPos;
        };

        fixed4 _ColorTop;
        fixed4 _ColorMiddle;
        fixed4 _ColorBottom;
        float _TopHeight;
        float _MiddleHeight;
        float _BottomHeight;
        half _Metallic;
        half _Smoothness;

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            // Calculate height for gradient interpolation
            float height = saturate((IN.worldPos.y - _BottomHeight) / (_TopHeight - _BottomHeight));

            // Interpolate between three colors
            fixed4 finalColor;
            if (height <= (_MiddleHeight - _BottomHeight) / (_TopHeight - _BottomHeight))
            {
                float t = saturate((height * (_TopHeight - _BottomHeight) - _BottomHeight) / (_MiddleHeight - _BottomHeight));
                finalColor = lerp(_ColorBottom, _ColorMiddle, t);
            }
            else
            {
                float t = saturate((height * (_TopHeight - _BottomHeight) - _MiddleHeight) / (_TopHeight - _MiddleHeight));
                finalColor = lerp(_ColorMiddle, _ColorTop, t);
            }

            // Assign interpolated color to the Albedo
            o.Albedo = finalColor.rgb;

            // Assign Metallic and Smoothness
            o.Metallic = _Metallic;
            o.Smoothness = _Smoothness;
        }
        ENDCG
    }
    FallBack "Diffuse"
}