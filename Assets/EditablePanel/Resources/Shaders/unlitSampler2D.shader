Shader "EPlane/unlitSampler2D"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
        

		Pass
		{
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

sampler2D _MainTex;
float4 _MainTex_ST;

struct a2v
{
	float4 vertex:POSITION;
	float2 uv : TEXCOORD0;
};

struct v2f
{
	float4 vertex:SV_POSITION;
	float2 uv:TEXCOORD0;
};

v2f vert(a2v v)
{
	v2f o;
	o.vertex = UnityObjectToClipPos(v.vertex);
	o.uv = TRANSFORM_TEX(v.uv, _MainTex);;
	return o;
}

fixed4 frag(v2f i):SV_Target
{
	return tex2D(_MainTex,i.uv); 
}

ENDCG
		}
    }
    FallBack "Diffuse"
}
