// Shader created with Shader Forge v1.04 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.04;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,rprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:0,culm:0,dpts:2,wrdp:True,dith:2,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:33949,y:32712,varname:node_1,prsc:2|diff-6777-OUT,spec-142-OUT,normal-428-OUT,emission-131-OUT,olwid-69-OUT,olcol-52-RGB;n:type:ShaderForge.SFN_Tex2d,id:2,x:33338,y:32117,ptovrint:False,ptlb:Diffuse,ptin:_Diffuse,varname:node_6810,prsc:2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:3,x:33338,y:32569,ptovrint:False,ptlb:Normal,ptin:_Normal,varname:node_8778,prsc:2,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Tex2d,id:4,x:33338,y:32302,ptovrint:False,ptlb:Specular,ptin:_Specular,varname:node_4566,prsc:2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:5,x:33103,y:32848,ptovrint:False,ptlb:Emissive,ptin:_Emissive,varname:node_3834,prsc:2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Vector1,id:34,x:33103,y:32773,varname:node_34,prsc:2,v1:0;n:type:ShaderForge.SFN_Time,id:40,x:32890,y:33055,varname:node_40,prsc:2;n:type:ShaderForge.SFN_Color,id:52,x:33609,y:33335,ptovrint:False,ptlb:Outline_Color,ptin:_Outline_Color,varname:node_1433,prsc:2,glob:False,c1:1,c2:0.6,c3:0,c4:1;n:type:ShaderForge.SFN_ValueProperty,id:63,x:33259,y:33276,ptovrint:False,ptlb:Outline_Width,ptin:_Outline_Width,varname:node_3341,prsc:2,glob:False,v1:0.05;n:type:ShaderForge.SFN_Multiply,id:69,x:33609,y:33167,varname:node_69,prsc:2|A-101-OUT,B-63-OUT;n:type:ShaderForge.SFN_Sin,id:90,x:33259,y:33113,varname:node_90,prsc:2|IN-113-OUT;n:type:ShaderForge.SFN_Add,id:101,x:33446,y:33096,varname:node_101,prsc:2|A-102-OUT,B-90-OUT;n:type:ShaderForge.SFN_Vector1,id:102,x:33259,y:33054,varname:node_102,prsc:2,v1:1.3;n:type:ShaderForge.SFN_Multiply,id:113,x:33089,y:33113,varname:node_113,prsc:2|A-40-T,B-115-OUT;n:type:ShaderForge.SFN_ValueProperty,id:115,x:32890,y:33195,ptovrint:False,ptlb:Pulse_Speed,ptin:_Pulse_Speed,varname:node_8042,prsc:2,glob:False,v1:6;n:type:ShaderForge.SFN_SwitchProperty,id:131,x:33607,y:32869,ptovrint:False,ptlb:Emissive_On/Off,ptin:_Emissive_OnOff,varname:node_5694,prsc:2,on:False|A-34-OUT,B-2651-OUT;n:type:ShaderForge.SFN_Multiply,id:142,x:33537,y:32392,varname:node_142,prsc:2|A-4-RGB,B-143-OUT;n:type:ShaderForge.SFN_ValueProperty,id:143,x:33338,y:32479,ptovrint:False,ptlb:Specular_Power,ptin:_Specular_Power,varname:node_7249,prsc:2,glob:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:428,x:33574,y:32586,varname:node_428,prsc:2|A-3-RGB,B-429-OUT;n:type:ShaderForge.SFN_ValueProperty,id:429,x:33338,y:32746,ptovrint:False,ptlb:Normal_Intensity,ptin:_Normal_Intensity,varname:node_7008,prsc:2,glob:False,v1:1;n:type:ShaderForge.SFN_Add,id:6777,x:33865,y:32133,varname:node_6777,prsc:2|A-2-RGB,B-618-OUT;n:type:ShaderForge.SFN_Multiply,id:618,x:33670,y:32162,varname:node_618,prsc:2|A-2-RGB,B-6710-OUT;n:type:ShaderForge.SFN_ValueProperty,id:6710,x:33490,y:32227,ptovrint:False,ptlb:Corrective_Glow,ptin:_Corrective_Glow,varname:node_7740,prsc:2,glob:False,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:6839,x:33259,y:32942,ptovrint:False,ptlb:Emission_Intensity,ptin:_Emission_Intensity,varname:node_9052,prsc:2,glob:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:2651,x:33433,y:32908,varname:node_2651,prsc:2|A-5-RGB,B-6839-OUT;proporder:2-4-143-3-131-5-63-52-115-429-6710-6839;pass:END;sub:END;*/

Shader "Objectify/Outline_Pulse" {
    Properties {
        _Diffuse ("Diffuse", 2D) = "white" {}
        _Specular ("Specular", 2D) = "white" {}
        _Specular_Power ("Specular_Power", Float ) = 1
        _Normal ("Normal", 2D) = "bump" {}
        [MaterialToggle] _Emissive_OnOff ("Emissive_On/Off", Float ) = 0
        _Emissive ("Emissive", 2D) = "white" {}
        _Outline_Width ("Outline_Width", Float ) = 0.05
        _Outline_Color ("Outline_Color", Color) = (1,0.6,0,1)
        _Pulse_Speed ("Pulse_Speed", Float ) = 6
        _Normal_Intensity ("Normal_Intensity", Float ) = 1
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
            uniform sampler2D _Normal; uniform float4 _Normal_ST;
            uniform sampler2D _Specular; uniform float4 _Specular_ST;
            uniform sampler2D _Emissive; uniform float4 _Emissive_ST;
            uniform fixed _Emissive_OnOff;
            uniform float _Specular_Power;
            uniform float _Normal_Intensity;
            uniform float _Corrective_Glow;
            uniform float _Emission_Intensity;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 binormalDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = mul(_Object2World, float4(v.normal,0)).xyz;
                o.tangentDir = normalize( mul( _Object2World, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.binormalDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(_Object2World, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.binormalDir, i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _Normal_var = UnpackNormal(tex2D(_Normal,TRANSFORM_TEX(i.uv0, _Normal)));
                float3 normalLocal = (_Normal_var.rgb*_Normal_Intensity);
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                float gloss = 0.5;
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float4 _Specular_var = tex2D(_Specular,TRANSFORM_TEX(i.uv0, _Specular));
                float3 specularColor = (_Specular_var.rgb*_Specular_Power);
                float3 directSpecular = (floor(attenuation) * _LightColor0.xyz) * pow(max(0,dot(halfDirection,normalDirection)),specPow);
                float3 specular = directSpecular * specularColor;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 indirectDiffuse = float3(0,0,0);
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                float3 diffuse = (directDiffuse + indirectDiffuse) * (_Diffuse_var.rgb+(_Diffuse_var.rgb*_Corrective_Glow));
////// Emissive:
                float4 _Emissive_var = tex2D(_Emissive,TRANSFORM_TEX(i.uv0, _Emissive));
                float3 emissive = lerp( 0.0, (_Emissive_var.rgb*_Emission_Intensity), _Emissive_OnOff );
/// Final Color:
                float3 finalColor = diffuse + specular + emissive;
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
            uniform sampler2D _Normal; uniform float4 _Normal_ST;
            uniform sampler2D _Specular; uniform float4 _Specular_ST;
            uniform sampler2D _Emissive; uniform float4 _Emissive_ST;
            uniform fixed _Emissive_OnOff;
            uniform float _Specular_Power;
            uniform float _Normal_Intensity;
            uniform float _Corrective_Glow;
            uniform float _Emission_Intensity;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 binormalDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = mul(_Object2World, float4(v.normal,0)).xyz;
                o.tangentDir = normalize( mul( _Object2World, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.binormalDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(_Object2World, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.binormalDir, i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _Normal_var = UnpackNormal(tex2D(_Normal,TRANSFORM_TEX(i.uv0, _Normal)));
                float3 normalLocal = (_Normal_var.rgb*_Normal_Intensity);
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                float gloss = 0.5;
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float4 _Specular_var = tex2D(_Specular,TRANSFORM_TEX(i.uv0, _Specular));
                float3 specularColor = (_Specular_var.rgb*_Specular_Power);
                float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow);
                float3 specular = directSpecular * specularColor;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                float3 diffuse = directDiffuse * (_Diffuse_var.rgb+(_Diffuse_var.rgb*_Corrective_Glow));
/// Final Color:
                float3 finalColor = diffuse + specular;
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
