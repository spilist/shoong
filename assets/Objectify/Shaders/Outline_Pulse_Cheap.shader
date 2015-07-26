// Shader created with Shader Forge v1.04 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.04;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,rprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:0,culm:0,dpts:2,wrdp:True,dith:2,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:33985,y:32712,varname:node_1,prsc:2|diff-6273-OUT,emission-131-OUT,olwid-69-OUT,olcol-52-RGB;n:type:ShaderForge.SFN_Tex2d,id:2,x:33373,y:32576,ptovrint:False,ptlb:Diffuse,ptin:_Diffuse,varname:node_819,prsc:2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:5,x:33059,y:32866,ptovrint:False,ptlb:Emissive,ptin:_Emissive,varname:node_8237,prsc:2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Vector1,id:34,x:33059,y:32791,varname:node_34,prsc:2,v1:0;n:type:ShaderForge.SFN_Time,id:40,x:32925,y:33055,varname:node_40,prsc:2;n:type:ShaderForge.SFN_Color,id:52,x:33644,y:33335,ptovrint:False,ptlb:Outline_Color,ptin:_Outline_Color,varname:node_4662,prsc:2,glob:False,c1:1,c2:0.6,c3:0,c4:1;n:type:ShaderForge.SFN_ValueProperty,id:63,x:33294,y:33276,ptovrint:False,ptlb:Outline_Width,ptin:_Outline_Width,varname:node_4657,prsc:2,glob:False,v1:0.05;n:type:ShaderForge.SFN_Multiply,id:69,x:33644,y:33167,varname:node_69,prsc:2|A-101-OUT,B-63-OUT;n:type:ShaderForge.SFN_Sin,id:90,x:33294,y:33113,varname:node_90,prsc:2|IN-113-OUT;n:type:ShaderForge.SFN_Add,id:101,x:33481,y:33096,varname:node_101,prsc:2|A-102-OUT,B-90-OUT;n:type:ShaderForge.SFN_Vector1,id:102,x:33294,y:33054,varname:node_102,prsc:2,v1:1.3;n:type:ShaderForge.SFN_Multiply,id:113,x:33124,y:33113,varname:node_113,prsc:2|A-40-T,B-115-OUT;n:type:ShaderForge.SFN_ValueProperty,id:115,x:32925,y:33195,ptovrint:False,ptlb:Pulse_Speed,ptin:_Pulse_Speed,varname:node_5497,prsc:2,glob:False,v1:6;n:type:ShaderForge.SFN_SwitchProperty,id:131,x:33642,y:32869,ptovrint:False,ptlb:Emissive_On/Off,ptin:_Emissive_OnOff,varname:node_3642,prsc:2,on:False|A-34-OUT,B-6857-OUT;n:type:ShaderForge.SFN_Add,id:6273,x:33749,y:32658,varname:node_6273,prsc:2|A-2-RGB,B-997-OUT;n:type:ShaderForge.SFN_Multiply,id:997,x:33554,y:32687,varname:node_997,prsc:2|A-2-RGB,B-9130-OUT;n:type:ShaderForge.SFN_ValueProperty,id:9130,x:33373,y:32751,ptovrint:False,ptlb:Corrective_Glow,ptin:_Corrective_Glow,varname:node_7740,prsc:2,glob:False,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:8962,x:33294,y:32991,ptovrint:False,ptlb:Emission_Intensity,ptin:_Emission_Intensity,varname:node_9052,prsc:2,glob:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:6857,x:33463,y:32891,varname:node_6857,prsc:2|A-5-RGB,B-8962-OUT;proporder:2-131-5-63-52-115-9130-8962;pass:END;sub:END;*/

Shader "Objectify/Outline_Pulse_Cheap" {
    Properties {
        _Diffuse ("Diffuse", 2D) = "white" {}
        [MaterialToggle] _Emissive_OnOff ("Emissive_On/Off", Float ) = 0
        _Emissive ("Emissive", 2D) = "white" {}
        _Outline_Width ("Outline_Width", Float ) = 0.05
        _Outline_Color ("Outline_Color", Color) = (1,0.6,0,1)
        _Pulse_Speed ("Pulse_Speed", Float ) = 6
        _Corrective_Glow ("Corrective_Glow", Float ) = 1
        _Emission_Intensity ("Emission_Intensity", Float ) = 1
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "Outline"
            Tags {
            }
            Cull Front
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform float4 _Outline_Color;
            uniform float _Outline_Width;
            uniform float _Pulse_Speed;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                float4 node_40 = _Time + _TimeEditor;
                o.pos = mul(UNITY_MATRIX_MVP, float4(v.vertex.xyz + v.normal*((1.3+sin((node_40.g*_Pulse_Speed)))*_Outline_Width),1));
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
/////// Vectors:
                return fixed4(_Outline_Color.rgb,0);
            }
            ENDCG
        }
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform sampler2D _Emissive; uniform float4 _Emissive_ST;
            uniform fixed _Emissive_OnOff;
            uniform float _Corrective_Glow;
            uniform float _Emission_Intensity;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = mul(_Object2World, float4(v.normal,0)).xyz;
                o.posWorld = mul(_Object2World, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
/////// Vectors:
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 indirectDiffuse = float3(0,0,0);
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                float3 diffuse = (directDiffuse + indirectDiffuse) * (_Diffuse_var.rgb+(_Diffuse_var.rgb*_Corrective_Glow));
////// Emissive:
                float4 _Emissive_var = tex2D(_Emissive,TRANSFORM_TEX(i.uv0, _Emissive));
                float3 emissive = lerp( 0.0, (_Emissive_var.rgb*_Emission_Intensity), _Emissive_OnOff );
/// Final Color:
                float3 finalColor = diffuse + emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
        Pass {
            Name "ForwardAdd"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            Fog { Color (0,0,0,0) }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform sampler2D _Emissive; uniform float4 _Emissive_ST;
            uniform fixed _Emissive_OnOff;
            uniform float _Corrective_Glow;
            uniform float _Emission_Intensity;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = mul(_Object2World, float4(v.normal,0)).xyz;
                o.posWorld = mul(_Object2World, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
/////// Vectors:
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                float3 diffuse = directDiffuse * (_Diffuse_var.rgb+(_Diffuse_var.rgb*_Corrective_Glow));
/// Final Color:
                float3 finalColor = diffuse;
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
