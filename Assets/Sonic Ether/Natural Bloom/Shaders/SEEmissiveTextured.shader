Shader "Sonic Ether/Emissive/Textured" {
Properties {
	_EmissionColor ("Emission Color", Color) = (1,1,1,1)
	_DiffuseColor ("Diffuse Color", Color) = (1, 1, 1, 1)
	_MainTex ("Diffuse Texture", 2D) = "white" {}
	_Illum ("Emission Texture", 2D) = "white" {}
	_EmissionGain ("Emission Gain", Range(0, 1)) = 0.5
	_EmissionTextureContrast ("Emission Texture Contrast", Range(1, 3)) = 1.0

	_Glossiness("Smoothness", Range(0,1)) = 0.5
	_Metallic("Metallic", Range(0,1)) = 0.0

	// START SMEAR ADD-ON
	_Position("Position", Vector) = (0, 0, 0, 0)
	_PrevPosition("Prev Position", Vector) = (0, 0, 0, 0)

	_NoiseScale("Noise Scale", Float) = 15
	_NoiseHeight("Noise Height", Float) = 1.3
	// END SMEAR ADD-ON
}

SubShader {
	Tags { "RenderType"="Transparent" "Queue"="Transparent" }
	LOD 200

CGPROGRAM
#pragma surface surf Standard vertex:vert alpha
#pragma target 3.0

sampler2D _MainTex;
sampler2D _Illum;
fixed4 _DiffuseColor;
fixed4 _EmissionColor;
float _EmissionGain;
float _EmissionTextureContrast;

struct Input {
	float2 uv_MainTex;
	float2 uv_Illum;
};

half _Glossiness;
half _Metallic;

// START SMEAR ADD-ON
fixed4 _PrevPosition;
fixed4 _Position;

half _NoiseScale;
half _NoiseHeight;

float hash(float n)
{
	return frac(sin(n)*43758.5453);
}

float noise(float3 x)
{
	// The noise function returns a value in the range -1.0f -> 1.0f

	float3 p = floor(x);
	float3 f = frac(x);

	f = f*f*(3.0 - 2.0*f);
	float n = p.x + p.y*57.0 + 113.0*p.z;

	return lerp(lerp(lerp(hash(n + 0.0), hash(n + 1.0), f.x),
	lerp(hash(n + 57.0), hash(n + 58.0), f.x), f.y),
	lerp(lerp(hash(n + 113.0), hash(n + 114.0), f.x),
	lerp(hash(n + 170.0), hash(n + 171.0), f.x), f.y), f.z);
}

void vert(inout appdata_full v, out Input o)
{
	UNITY_INITIALIZE_OUTPUT(Input, o);
	fixed4 worldPos = mul(unity_ObjectToWorld, v.vertex);

	fixed3 worldOffset = _Position.xyz - _PrevPosition.xyz; // -5
	fixed3 localOffset = worldPos.xyz - _Position.xyz; // -5

	// World offset should only be behind swing
	float dirDot = dot(normalize(worldOffset), normalize(localOffset));
	fixed3 unitVec = fixed3(1, 1, 1) * _NoiseHeight;
	worldOffset = clamp(worldOffset, unitVec * -1, unitVec);
	worldOffset *= -clamp(dirDot, -1, 0) * lerp(1, 0, step(length(worldOffset), 0));

	fixed3 smearOffset = -worldOffset.xyz * lerp(1, noise(worldPos * _NoiseScale), step(0, _NoiseScale));
	worldPos.xyz += smearOffset;
	v.vertex = mul(unity_WorldToObject, worldPos);
}
// END SMEAR ADD-ON

void surf (Input IN, inout SurfaceOutputStandard o) {
	fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
	fixed4 c = tex * _DiffuseColor;
	o.Albedo = c.rgb * c.a;
	fixed3 emissTex = tex2D(_Illum, IN.uv_Illum).rgb;
	float emissL = max(max(emissTex.r, emissTex.g), emissTex.b);
	fixed3 emissN = emissTex / (emissL + 0.0001);
	emissL = pow(emissL, _EmissionTextureContrast);
	emissTex = emissN * emissL;

	// Metallic and smoothness come from slider variables
	o.Metallic = _Metallic;
	o.Smoothness = _Glossiness;

	o.Emission = _EmissionColor * emissTex * (exp(_EmissionGain * 10.0f));
	o.Alpha = c.a;
}
ENDCG
}
FallBack "Self-Illumin/VertexLit"
}
