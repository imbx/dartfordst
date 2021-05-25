#pragma surface surf StandardTranslucent fullforwardshadows
#pragma target 3.0

sampler2D _MainTex;

struct Input {
    float2 uv_MainTex;
};

half _Glossiness;
half _Metallic;
fixed4 _Color;

float thickness;

#include "UnityPBSLighting.cginc"
inline fixed4 LightingStandardTranslucent(SurfaceOutputStandard s, fixed3 viewDir, UnityGI gi)
{
    // Original colour
    fixed4 pbr = LightingStandard(s, viewDir, gi);
    
    // --- Translucency ---
    float3 L = gi.light.dir;
    float3 V = viewDir;
    float3 N = s.Normal;
    float3 H = normalize(L + N * _Distortion);
    float VdotH = pow(saturate(dot(V, -H)), _Power) * _Scale;
    float3 I = _Attenuation * (VdotH + _Ambient) * thickness;
    // Final add
    pbr.rgb = pbr.rgb + gi.light.color * I;
    return pbr;
}

void LightingStandardTranslucent_GI(SurfaceOutputStandard s, UnityGIInput data, inout UnityGI gi)
{
    LightingStandard_GI(s, data, gi);        
}

void surf (Input IN, inout SurfaceOutputStandard o)
{
    // Albedo comes from a texture tinted by color
    fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
    o.Albedo = c.rgb;
    // Metallic and smoothness come from slider variables
    o.Metallic = _Metallic;
    o.Smoothness = _Glossiness;
    o.Alpha = c.a;
    thickness = tex2D (_LocalThickness, IN.uv_MainTex).r;
}

