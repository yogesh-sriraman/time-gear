// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.30 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.30;sub:START;pass:START;ps:flbk:Particles/Additive,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:7,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:1085,x:35661,y:32526,varname:node_1085,prsc:2|emission-3221-OUT,alpha-2557-OUT;n:type:ShaderForge.SFN_Tex2d,id:6236,x:31918,y:32741,varname:node_6236,prsc:2,ntxv:0,isnm:False|UVIN-2975-OUT,TEX-6733-TEX;n:type:ShaderForge.SFN_Tex2dAsset,id:6733,x:31760,y:32741,ptovrint:False,ptlb:MainTexture,ptin:_MainTexture,varname:node_6733,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:529,x:32139,y:32741,varname:node_529,prsc:2|A-6236-RGB,B-4974-RGB,C-3623-RGB,D-3623-A;n:type:ShaderForge.SFN_Color,id:4974,x:31918,y:32898,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_4974,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_VertexColor,id:3623,x:31918,y:33050,varname:node_3623,prsc:2;n:type:ShaderForge.SFN_Multiply,id:8336,x:32139,y:32898,varname:node_8336,prsc:2|A-6236-R,B-4974-A,C-3623-A;n:type:ShaderForge.SFN_Multiply,id:7726,x:32487,y:32745,varname:node_7726,prsc:2|A-1637-OUT,B-529-OUT;n:type:ShaderForge.SFN_ValueProperty,id:1637,x:32253,y:32598,ptovrint:False,ptlb:EmissiveIntensity,ptin:_EmissiveIntensity,varname:node_1637,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:4;n:type:ShaderForge.SFN_Multiply,id:5136,x:32487,y:32928,varname:node_5136,prsc:2|A-8336-OUT,B-1906-OUT;n:type:ShaderForge.SFN_ValueProperty,id:1906,x:32245,y:33141,ptovrint:False,ptlb:OpacityIntensity,ptin:_OpacityIntensity,varname:node_1906,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:4;n:type:ShaderForge.SFN_SwitchProperty,id:251,x:32787,y:32652,ptovrint:False,ptlb:HasNoiseEmissive?,ptin:_HasNoiseEmissive,varname:node_251,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False|A-7726-OUT,B-5083-OUT;n:type:ShaderForge.SFN_Tex2d,id:5766,x:32568,y:32434,varname:node_5766,prsc:2,ntxv:0,isnm:False|UVIN-1800-OUT,TEX-5958-TEX;n:type:ShaderForge.SFN_Tex2dAsset,id:5958,x:32408,y:32434,ptovrint:False,ptlb:Noise,ptin:_Noise,varname:node_5958,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:5083,x:32568,y:32565,varname:node_5083,prsc:2|A-2541-OUT,B-7726-OUT;n:type:ShaderForge.SFN_ValueProperty,id:7197,x:32021,y:32072,ptovrint:False,ptlb:U_Speed,ptin:_U_Speed,varname:node_8820,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.1;n:type:ShaderForge.SFN_ValueProperty,id:7662,x:32021,y:32167,ptovrint:False,ptlb:V_Speed,ptin:_V_Speed,varname:node_3475,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.15;n:type:ShaderForge.SFN_Append,id:7346,x:32231,y:32108,varname:node_7346,prsc:2|A-7197-OUT,B-7662-OUT;n:type:ShaderForge.SFN_Multiply,id:1706,x:32402,y:32108,varname:node_1706,prsc:2|A-7346-OUT,B-3262-T;n:type:ShaderForge.SFN_Time,id:3262,x:32231,y:32262,varname:node_3262,prsc:2;n:type:ShaderForge.SFN_Add,id:1800,x:32402,y:32262,varname:node_1800,prsc:2|A-1706-OUT,B-8600-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:8600,x:32231,y:32403,varname:node_8600,prsc:2,uv:0;n:type:ShaderForge.SFN_Power,id:2541,x:32806,y:32434,varname:node_2541,prsc:2|VAL-5766-RGB,EXP-2891-OUT;n:type:ShaderForge.SFN_ValueProperty,id:2891,x:32568,y:32364,ptovrint:False,ptlb:NoisePower,ptin:_NoisePower,varname:node_2891,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:3;n:type:ShaderForge.SFN_Multiply,id:7204,x:32688,y:33006,varname:node_7204,prsc:2|A-2189-OUT,B-5136-OUT;n:type:ShaderForge.SFN_ComponentMask,id:2189,x:32987,y:32434,varname:node_2189,prsc:2,cc1:0,cc2:-1,cc3:-1,cc4:-1|IN-2541-OUT;n:type:ShaderForge.SFN_SwitchProperty,id:6806,x:32849,y:32874,ptovrint:False,ptlb:HasNoiseOpacity?,ptin:_HasNoiseOpacity,varname:node_6806,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False|A-5136-OUT,B-7204-OUT;n:type:ShaderForge.SFN_Multiply,id:5785,x:33052,y:32915,varname:node_5785,prsc:2|A-6806-OUT,B-7176-OUT;n:type:ShaderForge.SFN_DepthBlend,id:7176,x:33067,y:33085,varname:node_7176,prsc:2|DIST-8558-OUT;n:type:ShaderForge.SFN_ValueProperty,id:8558,x:32913,y:33173,ptovrint:False,ptlb:Depth,ptin:_Depth,varname:node_8558,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.2;n:type:ShaderForge.SFN_SwitchProperty,id:5291,x:33227,y:32825,ptovrint:False,ptlb:HasOpacityDepth?,ptin:_HasOpacityDepth,varname:node_5291,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False|A-6806-OUT,B-5785-OUT;n:type:ShaderForge.SFN_SwitchProperty,id:1427,x:33288,y:32588,ptovrint:False,ptlb:HasEmissiveDepth?,ptin:_HasEmissiveDepth,varname:node_1427,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False|A-251-OUT,B-5834-OUT;n:type:ShaderForge.SFN_Multiply,id:5834,x:33115,y:32642,varname:node_5834,prsc:2|A-251-OUT,B-7176-OUT;n:type:ShaderForge.SFN_Multiply,id:7135,x:33823,y:32775,varname:node_7135,prsc:2|A-5291-OUT,B-5421-OUT;n:type:ShaderForge.SFN_ValueProperty,id:5421,x:33565,y:32944,ptovrint:False,ptlb:FinalOpacity,ptin:_FinalOpacity,varname:node_5421,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:1253,x:33944,y:32604,varname:node_1253,prsc:2|A-413-OUT,B-1974-OUT;n:type:ShaderForge.SFN_ValueProperty,id:413,x:33594,y:32551,ptovrint:False,ptlb:FinalEmissive,ptin:_FinalEmissive,varname:node_413,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:5215,x:34195,y:32630,varname:node_5215,prsc:2|A-1253-OUT,B-3559-OUT;n:type:ShaderForge.SFN_ValueProperty,id:3559,x:34010,y:32885,ptovrint:False,ptlb:FinalMix,ptin:_FinalMix,varname:node_3559,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:7114,x:34216,y:32823,varname:node_7114,prsc:2|A-7135-OUT,B-3559-OUT;n:type:ShaderForge.SFN_Multiply,id:7658,x:34808,y:32796,varname:node_7658,prsc:2|A-7114-OUT,B-787-OUT;n:type:ShaderForge.SFN_Fresnel,id:5436,x:34359,y:33012,varname:node_5436,prsc:2|EXP-8715-OUT;n:type:ShaderForge.SFN_ValueProperty,id:8715,x:33914,y:33021,ptovrint:False,ptlb:FresnelExp,ptin:_FresnelExp,varname:node_8715,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_OneMinus,id:787,x:34555,y:33012,varname:node_787,prsc:2|IN-5436-OUT;n:type:ShaderForge.SFN_Multiply,id:965,x:34687,y:32545,varname:node_965,prsc:2|A-5215-OUT,B-787-OUT;n:type:ShaderForge.SFN_Multiply,id:3221,x:35036,y:32579,varname:node_3221,prsc:2|A-965-OUT,B-2397-RGB;n:type:ShaderForge.SFN_Tex2d,id:2397,x:34882,y:33216,varname:node_2397,prsc:2,tex:e0d8caa53aafb3c45a56ad6afb0400ea,ntxv:0,isnm:False|TEX-573-TEX;n:type:ShaderForge.SFN_Tex2dAsset,id:573,x:34593,y:33220,ptovrint:False,ptlb:MainMask,ptin:_MainMask,varname:node_573,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:e0d8caa53aafb3c45a56ad6afb0400ea,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:2557,x:35036,y:32786,varname:node_2557,prsc:2|A-7658-OUT,B-2397-R;n:type:ShaderForge.SFN_Multiply,id:1974,x:33451,y:32381,varname:node_1974,prsc:2|A-8757-RGB,B-1427-OUT;n:type:ShaderForge.SFN_Tex2d,id:8757,x:33225,y:32230,varname:node_8757,prsc:2,ntxv:0,isnm:False|TEX-5899-TEX;n:type:ShaderForge.SFN_Tex2dAsset,id:5899,x:33048,y:32169,ptovrint:False,ptlb:GradientMap,ptin:_GradientMap,varname:node_5899,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Append,id:4198,x:31309,y:32703,varname:node_4198,prsc:2|A-5323-OUT,B-8906-OUT;n:type:ShaderForge.SFN_Multiply,id:1153,x:31480,y:32703,varname:node_1153,prsc:2|A-4198-OUT,B-8825-T;n:type:ShaderForge.SFN_Time,id:8825,x:31309,y:32857,varname:node_8825,prsc:2;n:type:ShaderForge.SFN_Add,id:2975,x:31480,y:32857,varname:node_2975,prsc:2|A-1153-OUT,B-2444-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:2444,x:31309,y:32998,varname:node_2444,prsc:2,uv:0;n:type:ShaderForge.SFN_ValueProperty,id:5323,x:31096,y:32685,ptovrint:False,ptlb:U_Speed_Main,ptin:_U_Speed_Main,varname:node_5323,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:8906,x:31096,y:32786,ptovrint:False,ptlb:V_Speed_Main,ptin:_V_Speed_Main,varname:node_8906,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:6274,x:35245,y:32943,varname:node_6274,prsc:2|A-5215-OUT,B-1370-OUT,C-3623-A;n:type:ShaderForge.SFN_ComponentMask,id:2615,x:35421,y:32943,varname:node_2615,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-6274-OUT;n:type:ShaderForge.SFN_ValueProperty,id:1370,x:34989,y:33041,ptovrint:False,ptlb:DistortionValue,ptin:_DistortionValue,varname:node_1370,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.05;proporder:6733-4974-1637-1906-251-5958-7197-7662-2891-6806-8558-5291-1427-5421-413-3559-8715-573-5899-5323-8906-1370;pass:END;sub:END;*/

Shader "Custom/Tunnel_Front" {
    Properties {
        _MainTexture ("MainTexture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _EmissiveIntensity ("EmissiveIntensity", Float ) = 4
        _OpacityIntensity ("OpacityIntensity", Float ) = 4
        [MaterialToggle] _HasNoiseEmissive ("HasNoiseEmissive?", Float ) = 0
        _Noise ("Noise", 2D) = "white" {}
        _U_Speed ("U_Speed", Float ) = 0.1
        _V_Speed ("V_Speed", Float ) = 0.15
        _NoisePower ("NoisePower", Float ) = 3
        [MaterialToggle] _HasNoiseOpacity ("HasNoiseOpacity?", Float ) = 0
        _Depth ("Depth", Float ) = 0.2
        [MaterialToggle] _HasOpacityDepth ("HasOpacityDepth?", Float ) = 0
        [MaterialToggle] _HasEmissiveDepth ("HasEmissiveDepth?", Float ) = 0
        _FinalOpacity ("FinalOpacity", Float ) = 1
        _FinalEmissive ("FinalEmissive", Float ) = 1
        _FinalMix ("FinalMix", Float ) = 1
        _FresnelExp ("FresnelExp", Float ) = 1
        _MainMask ("MainMask", 2D) = "white" {}
        _GradientMap ("GradientMap", 2D) = "white" {}
        _U_Speed_Main ("U_Speed_Main", Float ) = 0
        _V_Speed_Main ("V_Speed_Main", Float ) = 0
        _DistortionValue ("DistortionValue", Float ) = 0.05
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        LOD 200
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform sampler2D _CameraDepthTexture;
            uniform float4 _TimeEditor;
            uniform sampler2D _MainTexture; uniform float4 _MainTexture_ST;
            uniform float4 _Color;
            uniform float _EmissiveIntensity;
            uniform float _OpacityIntensity;
            uniform fixed _HasNoiseEmissive;
            uniform sampler2D _Noise; uniform float4 _Noise_ST;
            uniform float _U_Speed;
            uniform float _V_Speed;
            uniform float _NoisePower;
            uniform fixed _HasNoiseOpacity;
            uniform float _Depth;
            uniform fixed _HasOpacityDepth;
            uniform fixed _HasEmissiveDepth;
            uniform float _FinalOpacity;
            uniform float _FinalEmissive;
            uniform float _FinalMix;
            uniform float _FresnelExp;
            uniform sampler2D _MainMask; uniform float4 _MainMask_ST;
            uniform sampler2D _GradientMap; uniform float4 _GradientMap_ST;
            uniform float _U_Speed_Main;
            uniform float _V_Speed_Main;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float4 vertexColor : COLOR;
                float4 projPos : TEXCOORD3;
                UNITY_FOG_COORDS(4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float sceneZ = max(0,LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))) - _ProjectionParams.g);
                float partZ = max(0,i.projPos.z - _ProjectionParams.g);
////// Lighting:
////// Emissive:
                float4 node_8757 = tex2D(_GradientMap,TRANSFORM_TEX(i.uv0, _GradientMap));
                float4 node_8825 = _Time + _TimeEditor;
                float2 node_2975 = ((float2(_U_Speed_Main,_V_Speed_Main)*node_8825.g)+i.uv0);
                float4 node_6236 = tex2D(_MainTexture,TRANSFORM_TEX(node_2975, _MainTexture));
                float3 node_7726 = (_EmissiveIntensity*(node_6236.rgb*_Color.rgb*i.vertexColor.rgb*i.vertexColor.a));
                float4 node_3262 = _Time + _TimeEditor;
                float2 node_1800 = ((float2(_U_Speed,_V_Speed)*node_3262.g)+i.uv0);
                float4 node_5766 = tex2D(_Noise,TRANSFORM_TEX(node_1800, _Noise));
                float3 node_2541 = pow(node_5766.rgb,_NoisePower);
                float3 _HasNoiseEmissive_var = lerp( node_7726, (node_2541*node_7726), _HasNoiseEmissive );
                float node_7176 = saturate((sceneZ-partZ)/_Depth);
                float3 node_5215 = ((_FinalEmissive*(node_8757.rgb*lerp( _HasNoiseEmissive_var, (_HasNoiseEmissive_var*node_7176), _HasEmissiveDepth )))*_FinalMix);
                float node_787 = (1.0 - pow(1.0-max(0,dot(normalDirection, viewDirection)),_FresnelExp));
                float4 node_2397 = tex2D(_MainMask,TRANSFORM_TEX(i.uv0, _MainMask));
                float3 emissive = ((node_5215*node_787)*node_2397.rgb);
                float3 finalColor = emissive;
                float node_5136 = ((node_6236.r*_Color.a*i.vertexColor.a)*_OpacityIntensity);
                float _HasNoiseOpacity_var = lerp( node_5136, (node_2541.r*node_5136), _HasNoiseOpacity );
                fixed4 finalRGBA = fixed4(finalColor,((((lerp( _HasNoiseOpacity_var, (_HasNoiseOpacity_var*node_7176), _HasOpacityDepth )*_FinalOpacity)*_FinalMix)*node_787)*node_2397.r));
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Particles/Additive"
    CustomEditor "ShaderForgeMaterialInspector"
}
