Shader "Custom RP/Unlit"
{
    Properties
    {
        _BaseColor("Color", color) = (1.0,1.0,1.0,1.0)
    }
    
    SubShader
    {
        Pass
        {
            HLSLPROGRAM
            #pragma vertex UnlitPassVertex
            #pragma fragment UnlitPassFragment
            #include "UnlitPass.hlsl"

            ENDHLSL
        }
    }
}