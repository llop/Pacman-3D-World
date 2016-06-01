Shader "Unlit/BlackWithBorders"
{
Properties {
    _Color ("Main Color", Color) = (0,0,0,0)
    _MainTex ("Base (RGB)", 2D) = "white" {}
}
SubShader {
    Tags { "RenderType"="Opaque" }
    LOD 200
 
CGPROGRAM
#pragma surface surf Lambert

fixed4 _Color;
uniform float width;
uniform float height;
 
struct Input
{
    float2 uv_MainTex;
};
 
void surf (Input IN, inout SurfaceOutput o)
{


    float2 finalUV = IN.uv_MainTex;
 
    
    float4 mutedColor = float4(0,0.5,1,0);

    if (abs(o.Normal.z) < abs(o.Normal.x) && abs(o.Normal.x) > abs(o.Normal.y)) {
    	float tmp = width;
    	width = height;
    	height = tmp;
    }

    float borderAmountX = 0.1f / width;
    float borderAmountY = 0.1f / height;
 
    if( finalUV.x < borderAmountX || finalUV.x > (1.0f - borderAmountX) )
    {
        o.Albedo = mutedColor;
    }
    else if( finalUV.y < borderAmountY || finalUV.y > (1.0f - borderAmountY) )
    {
        o.Albedo = mutedColor;
    }
    else
    {
        o.Albedo = _Color;
    }
}
ENDCG
}
 
Fallback "VertexLit"
}