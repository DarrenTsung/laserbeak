// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Sonic Ether/Particles/AdditiveCircle" {
Properties {
	_Color ("Color", Color) = (0.5,0.5,0.5,0.5)
	_DissolveTex ("Dissolve Texture", 2D) = "white" {}
	_DissolveFactor ("Dissolve Factor", Range(0, 1)) = 1.0
	_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
	_EmissionGain ("Emission Gain", Range(0, 1)) = 0.3

	_Radius ("Radius", Range(0, 0.5)) = 0.5
	_Width ("Width", Range(0, 0.5)) = 0.01
	_SegmentLength ("Segment Length", Range(0, 1)) = 1.0
	_AngleOffset ("Angle Offset", Range(0, 1)) = 0.0
}

Category {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	Blend SrcAlpha One
	AlphaTest Greater .01
	ColorMask RGB
	Cull Off Lighting Off ZWrite Off Fog { Color (0,0,0,0) }

	SubShader {
		Pass {

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_particles

			#include "UnityCG.cginc"

			float _Radius;
			float _Width;
			float _SegmentLength;
			float _AngleOffset;

			sampler2D _DissolveTex;
			fixed4 _Color;

			struct appdata_t {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				#ifdef SOFTPARTICLES_ON
				float4 projPos : TEXCOORD1;
				#endif
			};

			float4 _DissolveTex_ST;

			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				#ifdef SOFTPARTICLES_ON
				o.projPos = ComputeScreenPos (o.vertex);
				COMPUTE_EYEDEPTH(o.projPos.z);
				#endif
				o.color = v.color;
				o.texcoord = TRANSFORM_TEX(v.texcoord, _DissolveTex);
				return o;
			}

			#if SHADER_API_METAL
			sampler2D_float _CameraDepthTexture;
			#else
			sampler2D _CameraDepthTexture;
			#endif
			float _InvFade;
			float _DissolveFactor;
			float _EmissionGain;

			fixed4 frag (v2f i) : SV_Target
			{
				#ifdef SOFTPARTICLES_ON
				float sceneZ = LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos))));
				float partZ = i.projPos.z;
				float fade = saturate (_InvFade * (sceneZ-partZ));
				i.color.a *= fade;
				#endif

				// grayscale texture but using red channel right now
				i.color.a *= step(_DissolveFactor, tex2D(_DissolveTex, i.texcoord).r);

				const float ALIAS = 0.03f;

				// check if inside circle
				float2 circlecoord = i.texcoord - float2(0.5, 0.5);
				// 0 if outside _Radius, 1 otherwise
				float coordSqrdLength =  dot(circlecoord, circlecoord);
				i.color.a *= smoothstep(coordSqrdLength, coordSqrdLength + ALIAS, pow(_Radius, 2));
				// 0 if inside (_Radius - _Width), 1 otherwise
				float inner = pow(max(0.0, _Radius - _Width), 2);
				i.color.a *= smoothstep(inner, inner + ALIAS, coordSqrdLength);

				// Circle Segment Logic
				const float PI = 3.14159265359;
				float x = (atan2(circlecoord.y, circlecoord.x) - PI/2.0);
				float y = 2.0*PI;
				float angle = (x - y * floor(x/y))/(2.0*PI);

				// center angle based on segment length
				angle += _SegmentLength / 2.0f;

				angle = (angle + _AngleOffset) % 1.0f;

				// 0 if outside angle, 1 otherwise
				i.color.a *= step(angle, _SegmentLength);

				return 2.0f * i.color * _Color * (exp(_EmissionGain * 5.0f));
			}
			ENDCG
		}
	}
}
}
