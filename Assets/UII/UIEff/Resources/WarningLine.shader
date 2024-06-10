Shader "WarningLineMask"
{
	Properties
	{
		_Color1("Color1", Color) = (1, 1, 1, 0)
		_Color2("Color2", Color) = (1, 0.8391529, 0, 0)
		_Tilling("Tilling", Vector) = (100, 100, 0, 0)
		_TillingSize("TillingSize", Float) = 4
		_OffsetSpeed("OffsetSpeed", Float) = 1
		_Rotation("Rotation", Float) = -15.7




		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		 _StencilReadMask("Stencil Read Mask", Float) = 255
		_ColorMask("Color Mask", Float) = 15
	}
		SubShader
	{
		Tags
		{

			"RenderType" = "Opaque"
		
		}
		LOD 100
		Pass
		{
			Name "Sprite Unlit"

			// Render State
			Cull Back
		Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
		ZTest[unity_GUIZTestMode]
		ZWrite Off

			Stencil
		{
			Ref[_Stencil]
			Comp[_StencilComp]
			Pass[_StencilOp]
			ReadMask[_StencilReadMask]
			WriteMask[_StencilWriteMask]
		}
		ColorMask[_ColorMask]

		// Debug
		// <None>

		// --------------------------------------------------
		// Pass

		HLSLPROGRAM

		// Pragmas
		#pragma target 2.0
	#pragma exclude_renderers d3d11_9x
	#pragma vertex vert
	#pragma fragment frag

		// DotsInstancingOptions: <None>
		// HybridV1InjectedBuiltinProperties: <None>

		// Keywords
		#pragma multi_compile_fragment _ DEBUG_DISPLAY
		// GraphKeywords: <None>

		// Defines
		#define _SURFACE_TYPE_TRANSPARENT 1
		#define ATTRIBUTES_NEED_NORMAL
		#define ATTRIBUTES_NEED_TANGENT
		#define ATTRIBUTES_NEED_TEXCOORD0
		#define ATTRIBUTES_NEED_COLOR
		#define VARYINGS_NEED_POSITION_WS
		#define VARYINGS_NEED_TEXCOORD0
		#define VARYINGS_NEED_COLOR
		#define FEATURES_GRAPH_VERTEX
		/* WARNING: $splice Could not find named fragment 'PassInstancing' */
		#define SHADERPASS SHADERPASS_SPRITEUNLIT
		/* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

		// Includes
		/* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreInclude' */

		#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
	#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

		// --------------------------------------------------
		// Structs and Packing

		/* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */

		struct Attributes
	{
		 float3 positionOS : POSITION;
		 float3 normalOS : NORMAL;
		 float4 tangentOS : TANGENT;
		 float4 uv0 : TEXCOORD0;
		 float4 color : COLOR;
		#if UNITY_ANY_INSTANCING_ENABLED
		 uint instanceID : INSTANCEID_SEMANTIC;
		#endif
	};
	struct Varyings
	{
		 float4 positionCS : SV_POSITION;
		 float3 positionWS;
		 float4 texCoord0;
		 float4 color;
		#if UNITY_ANY_INSTANCING_ENABLED
		 uint instanceID : CUSTOM_INSTANCE_ID;
		#endif
		#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		 uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
		#endif
		#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		 uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
		#endif
		#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		 FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
		#endif
	};
	struct SurfaceDescriptionInputs
	{
		 float4 uv0;
		 float3 TimeParameters;
	};
	struct VertexDescriptionInputs
	{
		 float3 ObjectSpaceNormal;
		 float3 ObjectSpaceTangent;
		 float3 ObjectSpacePosition;
	};
	struct PackedVaryings
	{
		 float4 positionCS : SV_POSITION;
		 float3 interp0 : INTERP0;
		 float4 interp1 : INTERP1;
		 float4 interp2 : INTERP2;
		#if UNITY_ANY_INSTANCING_ENABLED
		 uint instanceID : CUSTOM_INSTANCE_ID;
		#endif
		#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		 uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
		#endif
		#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		 uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
		#endif
		#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		 FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
		#endif
	};

		PackedVaryings PackVaryings(Varyings input)
	{
		PackedVaryings output;
		ZERO_INITIALIZE(PackedVaryings, output);
		output.positionCS = input.positionCS;
		output.interp0.xyz = input.positionWS;
		output.interp1.xyzw = input.texCoord0;
		output.interp2.xyzw = input.color;
		#if UNITY_ANY_INSTANCING_ENABLED
		output.instanceID = input.instanceID;
		#endif
		#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
		#endif
		#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
		#endif
		#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		output.cullFace = input.cullFace;
		#endif
		return output;
	}

	Varyings UnpackVaryings(PackedVaryings input)
	{
		Varyings output;
		output.positionCS = input.positionCS;
		output.positionWS = input.interp0.xyz;
		output.texCoord0 = input.interp1.xyzw;
		output.color = input.interp2.xyzw;
		#if UNITY_ANY_INSTANCING_ENABLED
		output.instanceID = input.instanceID;
		#endif
		#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
		#endif
		#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
		#endif
		#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		output.cullFace = input.cullFace;
		#endif
		return output;
	}


	// --------------------------------------------------
	// Graph

	// Graph Properties
	CBUFFER_START(UnityPerMaterial)
float4 _Color2;
float4 _Color1;
float2 _Tilling;
float _TillingSize;
float _OffsetSpeed;
float _Rotation;
CBUFFER_END

// Object and Global properties

	// Graph Includes
	// GraphIncludes: <None>

	// -- Property used by ScenePickingPass
	#ifdef SCENEPICKINGPASS
	float4 _SelectionID;
	#endif

// -- Properties used by SceneSelectionPass
#ifdef SCENESELECTIONPASS
int _ObjectId;
int _PassValue;
#endif

// Graph Functions

void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
{
	Out = A * B;
}

void Unity_Multiply_float_float(float A, float B, out float Out)
{
	Out = A * B;
}

void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
{
	Out = UV * Tiling + Offset;
}

void Unity_Rotate_Degrees_float(float2 UV, float2 Center, float Rotation, out float2 Out)
{
	//rotation matrix
	Rotation = Rotation * (3.1415926f / 180.0f);
	UV -= Center;
	float s = sin(Rotation);
	float c = cos(Rotation);

	//center rotation matrix
	float2x2 rMatrix = float2x2(c, -s, s, c);
	rMatrix *= 0.5;
	rMatrix += 0.5;
	rMatrix = rMatrix * 2 - 1;

	//multiply the UVs by the rotation matrix
	UV.xy = mul(UV.xy, rMatrix);
	UV += Center;

	Out = UV;
}

void Unity_Fraction_float(float In, out float Out)
{
	Out = frac(In);
}

void Unity_Step_float(float Edge, float In, out float Out)
{
	Out = step(Edge, In);
}

void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
{
	Out = A * B;
}

void Unity_Add_float4(float4 A, float4 B, out float4 Out)
{
	Out = A + B;
}

/* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */

// Graph Vertex
struct VertexDescription
{
	float3 Position;
	float3 Normal;
	float3 Tangent;
};

VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
{
	VertexDescription description = (VertexDescription)0;
	description.Position = IN.ObjectSpacePosition;
	description.Normal = IN.ObjectSpaceNormal;
	description.Tangent = IN.ObjectSpaceTangent;
	return description;
}

	#ifdef FEATURES_GRAPH_VERTEX
Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
{
return output;
}
#define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
#endif

// Graph Pixel
struct SurfaceDescription
{
	float3 BaseColor;
	float Alpha;
};

SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
{
	SurfaceDescription surface = (SurfaceDescription)0;
	float4 _Property_09c41c511fa944ca9e64d282255c6394_Out_0 = _Color2;
	float4 _Property_60c6534eed9b40b88f7671260c1b1efb_Out_0 = _Color1;
	float4 _UV_e431f7d4f13f4986ad0ed1697fdcdb64_Out_0 = IN.uv0;
	float2 _Property_7ebf8344b3624b1787d3e375b37e114d_Out_0 = _Tilling;
	float2 _Multiply_438a1e3cef5c484b82845d5fc6a1646d_Out_2;
	Unity_Multiply_float2_float2(_Property_7ebf8344b3624b1787d3e375b37e114d_Out_0, float2(0.01, 0.01), _Multiply_438a1e3cef5c484b82845d5fc6a1646d_Out_2);
	float _Property_a5409d3cd27a4b76964416ed0ca817c5_Out_0 = _TillingSize;
	float2 _Multiply_7dfa063e34254ceeb38d1fc0c702ae14_Out_2;
	Unity_Multiply_float2_float2(_Multiply_438a1e3cef5c484b82845d5fc6a1646d_Out_2, (_Property_a5409d3cd27a4b76964416ed0ca817c5_Out_0.xx), _Multiply_7dfa063e34254ceeb38d1fc0c702ae14_Out_2);
	float _Property_4b99a5d030794652bb4e658b9cc5e831_Out_0 = _OffsetSpeed;
	float _Multiply_83cc244aeb6f4768aca89ead20241cb3_Out_2;
	Unity_Multiply_float_float(IN.TimeParameters.x, _Property_4b99a5d030794652bb4e658b9cc5e831_Out_0, _Multiply_83cc244aeb6f4768aca89ead20241cb3_Out_2);
	float2 _Vector2_47fd82e29cdc446db758d476a4263dcd_Out_0 = float2(_Multiply_83cc244aeb6f4768aca89ead20241cb3_Out_2, 0);
	float2 _TilingAndOffset_6d8b377774e64bb1b2ed6a249b61d549_Out_3;
	Unity_TilingAndOffset_float((_UV_e431f7d4f13f4986ad0ed1697fdcdb64_Out_0.xy), _Multiply_7dfa063e34254ceeb38d1fc0c702ae14_Out_2, _Vector2_47fd82e29cdc446db758d476a4263dcd_Out_0, _TilingAndOffset_6d8b377774e64bb1b2ed6a249b61d549_Out_3);
	float _Property_7e55f4de81854d95983ba4e6d8de8aae_Out_0 = _Rotation;
	float2 _Rotate_e3982d3739ae430dbdbaa4f340096d68_Out_3;
	Unity_Rotate_Degrees_float(_TilingAndOffset_6d8b377774e64bb1b2ed6a249b61d549_Out_3, float2 (0.5, 0.5), _Property_7e55f4de81854d95983ba4e6d8de8aae_Out_0, _Rotate_e3982d3739ae430dbdbaa4f340096d68_Out_3);
	float _Split_f5722a2e57fc4ba7b65428d51b49743f_R_1 = _Rotate_e3982d3739ae430dbdbaa4f340096d68_Out_3[0];
	float _Split_f5722a2e57fc4ba7b65428d51b49743f_G_2 = _Rotate_e3982d3739ae430dbdbaa4f340096d68_Out_3[1];
	float _Split_f5722a2e57fc4ba7b65428d51b49743f_B_3 = 0;
	float _Split_f5722a2e57fc4ba7b65428d51b49743f_A_4 = 0;
	float _Fraction_cf9003c2fdf946e4a74f9ca3f744792b_Out_1;
	Unity_Fraction_float(_Split_f5722a2e57fc4ba7b65428d51b49743f_R_1, _Fraction_cf9003c2fdf946e4a74f9ca3f744792b_Out_1);
	float _Step_c10190ae190740c882de08f2853ac255_Out_2;
	Unity_Step_float(_Fraction_cf9003c2fdf946e4a74f9ca3f744792b_Out_1, 0.5, _Step_c10190ae190740c882de08f2853ac255_Out_2);
	float4 _Multiply_7c393bd901144b48b2454107524cbdb9_Out_2;
	Unity_Multiply_float4_float4(_Property_60c6534eed9b40b88f7671260c1b1efb_Out_0, (_Step_c10190ae190740c882de08f2853ac255_Out_2.xxxx), _Multiply_7c393bd901144b48b2454107524cbdb9_Out_2);
	float4 _Add_d844641a9fdb464caac8914eee217ab9_Out_2;
	Unity_Add_float4(_Property_09c41c511fa944ca9e64d282255c6394_Out_0, _Multiply_7c393bd901144b48b2454107524cbdb9_Out_2, _Add_d844641a9fdb464caac8914eee217ab9_Out_2);
	surface.BaseColor = (_Add_d844641a9fdb464caac8914eee217ab9_Out_2.xyz);
	surface.Alpha = 1;
	return surface;
}

// --------------------------------------------------
// Build Graph Inputs

VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
{
	VertexDescriptionInputs output;
	ZERO_INITIALIZE(VertexDescriptionInputs, output);

	output.ObjectSpaceNormal = input.normalOS;
	output.ObjectSpaceTangent = input.tangentOS.xyz;
	output.ObjectSpacePosition = input.positionOS;

	return output;
}
	SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
{
	SurfaceDescriptionInputs output;
	ZERO_INITIALIZE(SurfaceDescriptionInputs, output);







	output.uv0 = input.texCoord0;
	output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN                output.FaceSign =                                   IS_FRONT_VFACE(input.cullFace, true, false);
#else
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
#endif
#undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

	return output;
}

	// --------------------------------------------------
	// Main

	#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/2D/ShaderGraph/Includes/SpriteUnlitPass.hlsl"

	ENDHLSL
}
Pass
{
	Name "SceneSelectionPass"
	Tags
	{
		"LightMode" = "SceneSelectionPass"
	}

		// Render State
		Cull Off

		// Debug
		// <None>

		// --------------------------------------------------
		// Pass

		HLSLPROGRAM

		// Pragmas
		#pragma target 2.0
	#pragma exclude_renderers d3d11_9x
	#pragma vertex vert
	#pragma fragment frag

		// DotsInstancingOptions: <None>
		// HybridV1InjectedBuiltinProperties: <None>

		// Keywords
		// PassKeywords: <None>
		// GraphKeywords: <None>

		// Defines
		#define _SURFACE_TYPE_TRANSPARENT 1
		#define ATTRIBUTES_NEED_NORMAL
		#define ATTRIBUTES_NEED_TANGENT
		#define FEATURES_GRAPH_VERTEX
		/* WARNING: $splice Could not find named fragment 'PassInstancing' */
		#define SHADERPASS SHADERPASS_DEPTHONLY
	#define SCENESELECTIONPASS 1

		/* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

		// Includes
		/* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreInclude' */

		#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
	#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

		// --------------------------------------------------
		// Structs and Packing

		/* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */

		struct Attributes
	{
		 float3 positionOS : POSITION;
		 float3 normalOS : NORMAL;
		 float4 tangentOS : TANGENT;
		#if UNITY_ANY_INSTANCING_ENABLED
		 uint instanceID : INSTANCEID_SEMANTIC;
		#endif
	};
	struct Varyings
	{
		 float4 positionCS : SV_POSITION;
		#if UNITY_ANY_INSTANCING_ENABLED
		 uint instanceID : CUSTOM_INSTANCE_ID;
		#endif
		#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		 uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
		#endif
		#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		 uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
		#endif
		#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		 FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
		#endif
	};
	struct SurfaceDescriptionInputs
	{
	};
	struct VertexDescriptionInputs
	{
		 float3 ObjectSpaceNormal;
		 float3 ObjectSpaceTangent;
		 float3 ObjectSpacePosition;
	};
	struct PackedVaryings
	{
		 float4 positionCS : SV_POSITION;
		#if UNITY_ANY_INSTANCING_ENABLED
		 uint instanceID : CUSTOM_INSTANCE_ID;
		#endif
		#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		 uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
		#endif
		#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		 uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
		#endif
		#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		 FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
		#endif
	};

		PackedVaryings PackVaryings(Varyings input)
	{
		PackedVaryings output;
		ZERO_INITIALIZE(PackedVaryings, output);
		output.positionCS = input.positionCS;
		#if UNITY_ANY_INSTANCING_ENABLED
		output.instanceID = input.instanceID;
		#endif
		#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
		#endif
		#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
		#endif
		#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		output.cullFace = input.cullFace;
		#endif
		return output;
	}

	Varyings UnpackVaryings(PackedVaryings input)
	{
		Varyings output;
		output.positionCS = input.positionCS;
		#if UNITY_ANY_INSTANCING_ENABLED
		output.instanceID = input.instanceID;
		#endif
		#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
		#endif
		#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
		#endif
		#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		output.cullFace = input.cullFace;
		#endif
		return output;
	}


	// --------------------------------------------------
	// Graph

	// Graph Properties
	CBUFFER_START(UnityPerMaterial)
float4 _Color2;
float4 _Color1;
float2 _Tilling;
float _TillingSize;
float _OffsetSpeed;
float _Rotation;
CBUFFER_END

// Object and Global properties

	// Graph Includes
	// GraphIncludes: <None>

	// -- Property used by ScenePickingPass
	#ifdef SCENEPICKINGPASS
	float4 _SelectionID;
	#endif

// -- Properties used by SceneSelectionPass
#ifdef SCENESELECTIONPASS
int _ObjectId;
int _PassValue;
#endif

// Graph Functions
// GraphFunctions: <None>

/* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */

// Graph Vertex
struct VertexDescription
{
	float3 Position;
	float3 Normal;
	float3 Tangent;
};

VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
{
	VertexDescription description = (VertexDescription)0;
	description.Position = IN.ObjectSpacePosition;
	description.Normal = IN.ObjectSpaceNormal;
	description.Tangent = IN.ObjectSpaceTangent;
	return description;
}

	#ifdef FEATURES_GRAPH_VERTEX
Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
{
return output;
}
#define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
#endif

// Graph Pixel
struct SurfaceDescription
{
	float Alpha;
};

SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
{
	SurfaceDescription surface = (SurfaceDescription)0;
	surface.Alpha = 1;
	return surface;
}

// --------------------------------------------------
// Build Graph Inputs

VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
{
	VertexDescriptionInputs output;
	ZERO_INITIALIZE(VertexDescriptionInputs, output);

	output.ObjectSpaceNormal = input.normalOS;
	output.ObjectSpaceTangent = input.tangentOS.xyz;
	output.ObjectSpacePosition = input.positionOS;

	return output;
}
	SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
{
	SurfaceDescriptionInputs output;
	ZERO_INITIALIZE(SurfaceDescriptionInputs, output);







#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN                output.FaceSign =                                   IS_FRONT_VFACE(input.cullFace, true, false);
#else
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
#endif
#undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

	return output;
}

	// --------------------------------------------------
	// Main

	#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"

	ENDHLSL
}
Pass
{
	Name "ScenePickingPass"
	Tags
	{
		"LightMode" = "Picking"
	}

		// Render State
		Cull Back

		// Debug
		// <None>

		// --------------------------------------------------
		// Pass

		HLSLPROGRAM

		// Pragmas
		#pragma target 2.0
	#pragma exclude_renderers d3d11_9x
	#pragma vertex vert
	#pragma fragment frag

		// DotsInstancingOptions: <None>
		// HybridV1InjectedBuiltinProperties: <None>

		// Keywords
		// PassKeywords: <None>
		// GraphKeywords: <None>

		// Defines
		#define _SURFACE_TYPE_TRANSPARENT 1
		#define ATTRIBUTES_NEED_NORMAL
		#define ATTRIBUTES_NEED_TANGENT
		#define FEATURES_GRAPH_VERTEX
		/* WARNING: $splice Could not find named fragment 'PassInstancing' */
		#define SHADERPASS SHADERPASS_DEPTHONLY
	#define SCENEPICKINGPASS 1

		/* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

		// Includes
		/* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreInclude' */

		#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
	#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

		// --------------------------------------------------
		// Structs and Packing

		/* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */

		struct Attributes
	{
		 float3 positionOS : POSITION;
		 float3 normalOS : NORMAL;
		 float4 tangentOS : TANGENT;
		#if UNITY_ANY_INSTANCING_ENABLED
		 uint instanceID : INSTANCEID_SEMANTIC;
		#endif
	};
	struct Varyings
	{
		 float4 positionCS : SV_POSITION;
		#if UNITY_ANY_INSTANCING_ENABLED
		 uint instanceID : CUSTOM_INSTANCE_ID;
		#endif
		#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		 uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
		#endif
		#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		 uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
		#endif
		#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		 FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
		#endif
	};
	struct SurfaceDescriptionInputs
	{
	};
	struct VertexDescriptionInputs
	{
		 float3 ObjectSpaceNormal;
		 float3 ObjectSpaceTangent;
		 float3 ObjectSpacePosition;
	};
	struct PackedVaryings
	{
		 float4 positionCS : SV_POSITION;
		#if UNITY_ANY_INSTANCING_ENABLED
		 uint instanceID : CUSTOM_INSTANCE_ID;
		#endif
		#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		 uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
		#endif
		#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		 uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
		#endif
		#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		 FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
		#endif
	};

		PackedVaryings PackVaryings(Varyings input)
	{
		PackedVaryings output;
		ZERO_INITIALIZE(PackedVaryings, output);
		output.positionCS = input.positionCS;
		#if UNITY_ANY_INSTANCING_ENABLED
		output.instanceID = input.instanceID;
		#endif
		#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
		#endif
		#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
		#endif
		#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		output.cullFace = input.cullFace;
		#endif
		return output;
	}

	Varyings UnpackVaryings(PackedVaryings input)
	{
		Varyings output;
		output.positionCS = input.positionCS;
		#if UNITY_ANY_INSTANCING_ENABLED
		output.instanceID = input.instanceID;
		#endif
		#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
		#endif
		#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
		#endif
		#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		output.cullFace = input.cullFace;
		#endif
		return output;
	}


	// --------------------------------------------------
	// Graph

	// Graph Properties
	CBUFFER_START(UnityPerMaterial)
float4 _Color2;
float4 _Color1;
float2 _Tilling;
float _TillingSize;
float _OffsetSpeed;
float _Rotation;
CBUFFER_END

// Object and Global properties

	// Graph Includes
	// GraphIncludes: <None>

	// -- Property used by ScenePickingPass
	#ifdef SCENEPICKINGPASS
	float4 _SelectionID;
	#endif

// -- Properties used by SceneSelectionPass
#ifdef SCENESELECTIONPASS
int _ObjectId;
int _PassValue;
#endif

// Graph Functions
// GraphFunctions: <None>

/* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */

// Graph Vertex
struct VertexDescription
{
	float3 Position;
	float3 Normal;
	float3 Tangent;
};

VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
{
	VertexDescription description = (VertexDescription)0;
	description.Position = IN.ObjectSpacePosition;
	description.Normal = IN.ObjectSpaceNormal;
	description.Tangent = IN.ObjectSpaceTangent;
	return description;
}

	#ifdef FEATURES_GRAPH_VERTEX
Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
{
return output;
}
#define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
#endif

// Graph Pixel
struct SurfaceDescription
{
	float Alpha;
};

SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
{
	SurfaceDescription surface = (SurfaceDescription)0;
	surface.Alpha = 1;
	return surface;
}

// --------------------------------------------------
// Build Graph Inputs

VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
{
	VertexDescriptionInputs output;
	ZERO_INITIALIZE(VertexDescriptionInputs, output);

	output.ObjectSpaceNormal = input.normalOS;
	output.ObjectSpaceTangent = input.tangentOS.xyz;
	output.ObjectSpacePosition = input.positionOS;

	return output;
}
	SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
{
	SurfaceDescriptionInputs output;
	ZERO_INITIALIZE(SurfaceDescriptionInputs, output);







#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN                output.FaceSign =                                   IS_FRONT_VFACE(input.cullFace, true, false);
#else
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
#endif
#undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

	return output;
}

	// --------------------------------------------------
	// Main

	#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"

	ENDHLSL
}
Pass
{
	Name "Sprite Unlit"
	Tags
	{
		"LightMode" = "UniversalForward"
	}

		// Render State
		Cull Off
	Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
	ZTest LEqual
	ZWrite Off

		// Debug
		// <None>

		// --------------------------------------------------
		// Pass

		HLSLPROGRAM

		// Pragmas
		#pragma target 2.0
	#pragma exclude_renderers d3d11_9x
	#pragma vertex vert
	#pragma fragment frag

		// DotsInstancingOptions: <None>
		// HybridV1InjectedBuiltinProperties: <None>

		// Keywords
		#pragma multi_compile_fragment _ DEBUG_DISPLAY
		// GraphKeywords: <None>

		// Defines
		#define _SURFACE_TYPE_TRANSPARENT 1
		#define ATTRIBUTES_NEED_NORMAL
		#define ATTRIBUTES_NEED_TANGENT
		#define ATTRIBUTES_NEED_TEXCOORD0
		#define ATTRIBUTES_NEED_COLOR
		#define VARYINGS_NEED_POSITION_WS
		#define VARYINGS_NEED_TEXCOORD0
		#define VARYINGS_NEED_COLOR
		#define FEATURES_GRAPH_VERTEX
		/* WARNING: $splice Could not find named fragment 'PassInstancing' */
		#define SHADERPASS SHADERPASS_SPRITEFORWARD
		/* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

		// Includes
		/* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreInclude' */

		#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
	#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

		// --------------------------------------------------
		// Structs and Packing

		/* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */

		struct Attributes
	{
		 float3 positionOS : POSITION;
		 float3 normalOS : NORMAL;
		 float4 tangentOS : TANGENT;
		 float4 uv0 : TEXCOORD0;
		 float4 color : COLOR;
		#if UNITY_ANY_INSTANCING_ENABLED
		 uint instanceID : INSTANCEID_SEMANTIC;
		#endif
	};
	struct Varyings
	{
		 float4 positionCS : SV_POSITION;
		 float3 positionWS;
		 float4 texCoord0;
		 float4 color;
		#if UNITY_ANY_INSTANCING_ENABLED
		 uint instanceID : CUSTOM_INSTANCE_ID;
		#endif
		#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		 uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
		#endif
		#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		 uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
		#endif
		#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		 FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
		#endif
	};
	struct SurfaceDescriptionInputs
	{
		 float4 uv0;
		 float3 TimeParameters;
	};
	struct VertexDescriptionInputs
	{
		 float3 ObjectSpaceNormal;
		 float3 ObjectSpaceTangent;
		 float3 ObjectSpacePosition;
	};
	struct PackedVaryings
	{
		 float4 positionCS : SV_POSITION;
		 float3 interp0 : INTERP0;
		 float4 interp1 : INTERP1;
		 float4 interp2 : INTERP2;
		#if UNITY_ANY_INSTANCING_ENABLED
		 uint instanceID : CUSTOM_INSTANCE_ID;
		#endif
		#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		 uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
		#endif
		#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		 uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
		#endif
		#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		 FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
		#endif
	};

		PackedVaryings PackVaryings(Varyings input)
	{
		PackedVaryings output;
		ZERO_INITIALIZE(PackedVaryings, output);
		output.positionCS = input.positionCS;
		output.interp0.xyz = input.positionWS;
		output.interp1.xyzw = input.texCoord0;
		output.interp2.xyzw = input.color;
		#if UNITY_ANY_INSTANCING_ENABLED
		output.instanceID = input.instanceID;
		#endif
		#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
		#endif
		#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
		#endif
		#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		output.cullFace = input.cullFace;
		#endif
		return output;
	}

	Varyings UnpackVaryings(PackedVaryings input)
	{
		Varyings output;
		output.positionCS = input.positionCS;
		output.positionWS = input.interp0.xyz;
		output.texCoord0 = input.interp1.xyzw;
		output.color = input.interp2.xyzw;
		#if UNITY_ANY_INSTANCING_ENABLED
		output.instanceID = input.instanceID;
		#endif
		#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
		#endif
		#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
		#endif
		#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		output.cullFace = input.cullFace;
		#endif
		return output;
	}


	// --------------------------------------------------
	// Graph

	// Graph Properties
	CBUFFER_START(UnityPerMaterial)
float4 _Color2;
float4 _Color1;
float2 _Tilling;
float _TillingSize;
float _OffsetSpeed;
float _Rotation;
CBUFFER_END

// Object and Global properties

	// Graph Includes
	// GraphIncludes: <None>

	// -- Property used by ScenePickingPass
	#ifdef SCENEPICKINGPASS
	float4 _SelectionID;
	#endif

// -- Properties used by SceneSelectionPass
#ifdef SCENESELECTIONPASS
int _ObjectId;
int _PassValue;
#endif

// Graph Functions

void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
{
	Out = A * B;
}

void Unity_Multiply_float_float(float A, float B, out float Out)
{
	Out = A * B;
}

void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
{
	Out = UV * Tiling + Offset;
}

void Unity_Rotate_Degrees_float(float2 UV, float2 Center, float Rotation, out float2 Out)
{
	//rotation matrix
	Rotation = Rotation * (3.1415926f / 180.0f);
	UV -= Center;
	float s = sin(Rotation);
	float c = cos(Rotation);

	//center rotation matrix
	float2x2 rMatrix = float2x2(c, -s, s, c);
	rMatrix *= 0.5;
	rMatrix += 0.5;
	rMatrix = rMatrix * 2 - 1;

	//multiply the UVs by the rotation matrix
	UV.xy = mul(UV.xy, rMatrix);
	UV += Center;

	Out = UV;
}

void Unity_Fraction_float(float In, out float Out)
{
	Out = frac(In);
}

void Unity_Step_float(float Edge, float In, out float Out)
{
	Out = step(Edge, In);
}

void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
{
	Out = A * B;
}

void Unity_Add_float4(float4 A, float4 B, out float4 Out)
{
	Out = A + B;
}

/* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */

// Graph Vertex
struct VertexDescription
{
	float3 Position;
	float3 Normal;
	float3 Tangent;
};

VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
{
	VertexDescription description = (VertexDescription)0;
	description.Position = IN.ObjectSpacePosition;
	description.Normal = IN.ObjectSpaceNormal;
	description.Tangent = IN.ObjectSpaceTangent;
	return description;
}

	#ifdef FEATURES_GRAPH_VERTEX
Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
{
return output;
}
#define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
#endif

// Graph Pixel
struct SurfaceDescription
{
	float3 BaseColor;
	float Alpha;
};

SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
{
	SurfaceDescription surface = (SurfaceDescription)0;
	float4 _Property_09c41c511fa944ca9e64d282255c6394_Out_0 = _Color2;
	float4 _Property_60c6534eed9b40b88f7671260c1b1efb_Out_0 = _Color1;
	float4 _UV_e431f7d4f13f4986ad0ed1697fdcdb64_Out_0 = IN.uv0;
	float2 _Property_7ebf8344b3624b1787d3e375b37e114d_Out_0 = _Tilling;
	float2 _Multiply_438a1e3cef5c484b82845d5fc6a1646d_Out_2;
	Unity_Multiply_float2_float2(_Property_7ebf8344b3624b1787d3e375b37e114d_Out_0, float2(0.01, 0.01), _Multiply_438a1e3cef5c484b82845d5fc6a1646d_Out_2);
	float _Property_a5409d3cd27a4b76964416ed0ca817c5_Out_0 = _TillingSize;
	float2 _Multiply_7dfa063e34254ceeb38d1fc0c702ae14_Out_2;
	Unity_Multiply_float2_float2(_Multiply_438a1e3cef5c484b82845d5fc6a1646d_Out_2, (_Property_a5409d3cd27a4b76964416ed0ca817c5_Out_0.xx), _Multiply_7dfa063e34254ceeb38d1fc0c702ae14_Out_2);
	float _Property_4b99a5d030794652bb4e658b9cc5e831_Out_0 = _OffsetSpeed;
	float _Multiply_83cc244aeb6f4768aca89ead20241cb3_Out_2;
	Unity_Multiply_float_float(IN.TimeParameters.x, _Property_4b99a5d030794652bb4e658b9cc5e831_Out_0, _Multiply_83cc244aeb6f4768aca89ead20241cb3_Out_2);
	float2 _Vector2_47fd82e29cdc446db758d476a4263dcd_Out_0 = float2(_Multiply_83cc244aeb6f4768aca89ead20241cb3_Out_2, 0);
	float2 _TilingAndOffset_6d8b377774e64bb1b2ed6a249b61d549_Out_3;
	Unity_TilingAndOffset_float((_UV_e431f7d4f13f4986ad0ed1697fdcdb64_Out_0.xy), _Multiply_7dfa063e34254ceeb38d1fc0c702ae14_Out_2, _Vector2_47fd82e29cdc446db758d476a4263dcd_Out_0, _TilingAndOffset_6d8b377774e64bb1b2ed6a249b61d549_Out_3);
	float _Property_7e55f4de81854d95983ba4e6d8de8aae_Out_0 = _Rotation;
	float2 _Rotate_e3982d3739ae430dbdbaa4f340096d68_Out_3;
	Unity_Rotate_Degrees_float(_TilingAndOffset_6d8b377774e64bb1b2ed6a249b61d549_Out_3, float2 (0.5, 0.5), _Property_7e55f4de81854d95983ba4e6d8de8aae_Out_0, _Rotate_e3982d3739ae430dbdbaa4f340096d68_Out_3);
	float _Split_f5722a2e57fc4ba7b65428d51b49743f_R_1 = _Rotate_e3982d3739ae430dbdbaa4f340096d68_Out_3[0];
	float _Split_f5722a2e57fc4ba7b65428d51b49743f_G_2 = _Rotate_e3982d3739ae430dbdbaa4f340096d68_Out_3[1];
	float _Split_f5722a2e57fc4ba7b65428d51b49743f_B_3 = 0;
	float _Split_f5722a2e57fc4ba7b65428d51b49743f_A_4 = 0;
	float _Fraction_cf9003c2fdf946e4a74f9ca3f744792b_Out_1;
	Unity_Fraction_float(_Split_f5722a2e57fc4ba7b65428d51b49743f_R_1, _Fraction_cf9003c2fdf946e4a74f9ca3f744792b_Out_1);
	float _Step_c10190ae190740c882de08f2853ac255_Out_2;
	Unity_Step_float(_Fraction_cf9003c2fdf946e4a74f9ca3f744792b_Out_1, 0.5, _Step_c10190ae190740c882de08f2853ac255_Out_2);
	float4 _Multiply_7c393bd901144b48b2454107524cbdb9_Out_2;
	Unity_Multiply_float4_float4(_Property_60c6534eed9b40b88f7671260c1b1efb_Out_0, (_Step_c10190ae190740c882de08f2853ac255_Out_2.xxxx), _Multiply_7c393bd901144b48b2454107524cbdb9_Out_2);
	float4 _Add_d844641a9fdb464caac8914eee217ab9_Out_2;
	Unity_Add_float4(_Property_09c41c511fa944ca9e64d282255c6394_Out_0, _Multiply_7c393bd901144b48b2454107524cbdb9_Out_2, _Add_d844641a9fdb464caac8914eee217ab9_Out_2);
	surface.BaseColor = (_Add_d844641a9fdb464caac8914eee217ab9_Out_2.xyz);
	surface.Alpha = 1;
	return surface;
}

// --------------------------------------------------
// Build Graph Inputs

VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
{
	VertexDescriptionInputs output;
	ZERO_INITIALIZE(VertexDescriptionInputs, output);

	output.ObjectSpaceNormal = input.normalOS;
	output.ObjectSpaceTangent = input.tangentOS.xyz;
	output.ObjectSpacePosition = input.positionOS;

	return output;
}
	SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
{
	SurfaceDescriptionInputs output;
	ZERO_INITIALIZE(SurfaceDescriptionInputs, output);







	output.uv0 = input.texCoord0;
	output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN                output.FaceSign =                                   IS_FRONT_VFACE(input.cullFace, true, false);
#else
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
#endif
#undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

	return output;
}

	// --------------------------------------------------
	// Main

	#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/2D/ShaderGraph/Includes/SpriteUnlitPass.hlsl"

	ENDHLSL
}
	}
		CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
		FallBack "Hidden/Shader Graph/FallbackError"
}