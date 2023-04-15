Shader "Custom/ZWriteOff"
{
    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"    
        }
        Pass
        {
            ZWrite off
        }
    }
}
