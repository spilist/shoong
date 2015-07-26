// Shader created with Shader Forge v1.04 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.04;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,rprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:0,culm:0,dpts:2,wrdp:True,dith:2,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:34377,y:32712,varname:node_1,prsc:2|diff-4460-OUT,emission-163-OUT;n:type:ShaderForge.SFN_Tex2d,id:3,x:33700,y:32537,ptovrint:False,ptlb:Diffuse,ptin:_Diffuse,varname:node_2830,prsc:2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:9,x:33542,y:32733,ptovrint:False,ptlb:Emissive,ptin:_Emissive,varname:node_9334,prsc:2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Vector1,id:35,x:33542,y:32658,varname:node_35,prsc:2,v1:0;n:type:ShaderForge.SFN_Time,id:41,x:32890,y:33043,varname:node_41,prsc:2;n:type:ShaderForge.SFN_Color,id:53,x:33700,y:33121,ptovrint:False,ptlb:Fresnel_Color,ptin:_Fresnel_Color,varname:node_67,prsc:2,glob:False,c1:1,c2:0.6,c3:0,c4:1;n:type:ShaderForge.SFN_Sin,id:91,x:33295,y:33107,varname:node_91,prsc:2|IN-114-OUT;n:type:ShaderForge.SFN_Add,id:102,x:33490,y:33060,varname:node_102,prsc:2|A-331-OUT,B-91-OUT;n:type:ShaderForge.SFN_Multiply,id:114,x:33120,y:33107,varname:node_114,prsc:2|A-41-T,B-116-OUT;n:type:ShaderForge.SFN_ValueProperty,id:116,x:32890,y:33183,ptovrint:False,ptlb:Pulse_Speed,ptin:_Pulse_Speed,varname:node_1124,prsc:2,glob:False,v1:6;n:type:ShaderForge.SFN_SwitchProperty,id:132,x:33910,y:32757,ptovrint:False,ptlb:Emissive_On/Off,ptin:_Emissive_OnOff,varname:node_5679,prsc:2,on:False|A-35-OUT,B-3551-OUT;n:type:ShaderForge.SFN_Add,id:163,x:34109,y:32787,varname:node_163,prsc:2|A-132-OUT,B-363-OUT;n:type:ShaderForge.SFN_Fresnel,id:225,x:33700,y:32975,varname:node_225,prsc:2|EXP-102-OUT;n:type:ShaderForge.SFN_Slider,id:331,x:33138,y:33037,ptovrint:False,ptlb:Fresnel_Width,ptin:_Fresnel_Width,varname:node_1261,prsc:2,min:1.5,cur:1.5,max:5;n:type:ShaderForge.SFN_Multiply,id:363,x:33911,y:33048,varname:node_363,prsc:2|A-225-OUT,B-53-RGB;n:type:ShaderForge.SFN_Add,id:4460,x:34226,y:32563,varname:node_4460,prsc:2|A-3-RGB,B-7562-OUT;n:type:ShaderForge.SFN_Multiply,id:7562,x:34031,y:32592,varname:node_7562,prsc:2|A-3-RGB,B-5000-OUT;n:type:ShaderForge.SFN_ValueProperty,id:5000,x:33850,y:32656,ptovrint:False,ptlb:Corrective_Glow,ptin:_Corrective_Glow,varname:node_7740,prsc:2,glob:False,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:7834,x:33557,y:32918,ptovrint:False,ptlb:Emission_Intensity,ptin:_Emission_Intensity,varname:node_9052,prsc:2,glob:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:3551,x:33740,y:32792,varname:node_3551,prsc:2|A-9-RGB,B-7834-OUT;proporder:3-132-9-116-331-53-5000-7834;pass:END;sub:END;*/

Shader "Objectify/Fresnel_Pulse_Cheap" {
    Properties {
        _Diffuse ("Diffuse", 2D) = "white" {}
        [MaterialToggle] _Emissive_OnOff ("Emissive_On/Off", Float ) = 0
        _Emissive ("Emissive", 2D) = "white" {}
        _Pulse_Speed ("Pulse_Speed", Float ) = 6
        _Fresnel_Width ("Fresnel_Width", Range(1.5, 5)) = 1.5
        _Fresnel_Color ("Fresnel_Color", Color) = (1,0.6,0,1)
        _Corrective_Glow ("Corrective_Glow", Float ) = 1
        _Emission_Intensity ("Emission_Intensity", Float ) = 1
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
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
            uniform float4 _TimeEditor;
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform sampler2D _Emissive; uniform float4 _Emissive_ST;
            uniform float4 _Fresnel_Color;
            uniform float _Pulse_Speed;
            uniform fixed _Emissive_OnOff;
            uniform float _Fresnel_Width;
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
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
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
                float4 node_41 = _Time + _TimeEditor;
                float3 emissive = (lerp( 0.0, (_Emissive_var.rgb*_Emission_Intensity), _Emissive_OnOff )+(pow(1.0-max(0,dot(normalDirection, viewDirection)),(_Fresnel_Width+sin((node_41.g*_Pulse_Speed))))*_Fresnel_Color.rgb));
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
            uniform float4 _TimeEditor;
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform sampler2D _Emissive; uniform float4 _Emissive_ST;
            uniform float4 _Fresnel_Color;
            uniform float _Pulse_Speed;
            uniform fixed _Emissive_OnOff;
            uniform float _Fresnel_Width;
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
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
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
