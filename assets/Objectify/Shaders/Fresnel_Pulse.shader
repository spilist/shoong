// Shader created with Shader Forge v1.04 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.04;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,rprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:0,culm:0,dpts:2,wrdp:True,dith:2,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:34610,y:32683,varname:node_1,prsc:2|diff-9942-OUT,spec-143-OUT,normal-271-OUT,emission-163-OUT;n:type:ShaderForge.SFN_Tex2d,id:3,x:33581,y:32181,ptovrint:False,ptlb:Diffuse,ptin:_Diffuse,varname:node_8776,prsc:2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:5,x:33581,y:32624,ptovrint:False,ptlb:Normal,ptin:_Normal,varname:node_4560,prsc:2,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Tex2d,id:7,x:33581,y:32366,ptovrint:False,ptlb:Specular,ptin:_Specular,varname:node_2922,prsc:2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:9,x:33448,y:32928,ptovrint:False,ptlb:Emissive,ptin:_Emissive,varname:node_8816,prsc:2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Vector1,id:35,x:33448,y:32853,varname:node_35,prsc:2,v1:0;n:type:ShaderForge.SFN_Time,id:41,x:32771,y:33218,varname:node_41,prsc:2;n:type:ShaderForge.SFN_Color,id:53,x:33581,y:33296,ptovrint:False,ptlb:Fresnel_Color,ptin:_Fresnel_Color,varname:node_1464,prsc:2,glob:False,c1:1,c2:0.6,c3:0,c4:1;n:type:ShaderForge.SFN_Sin,id:91,x:33176,y:33282,varname:node_91,prsc:2|IN-114-OUT;n:type:ShaderForge.SFN_Add,id:102,x:33371,y:33235,varname:node_102,prsc:2|A-331-OUT,B-91-OUT;n:type:ShaderForge.SFN_Multiply,id:114,x:33001,y:33282,varname:node_114,prsc:2|A-41-T,B-116-OUT;n:type:ShaderForge.SFN_ValueProperty,id:116,x:32771,y:33358,ptovrint:False,ptlb:Pulse_Speed,ptin:_Pulse_Speed,varname:node_9728,prsc:2,glob:False,v1:6;n:type:ShaderForge.SFN_SwitchProperty,id:132,x:33791,y:32932,ptovrint:False,ptlb:Emissive_On/Off,ptin:_Emissive_OnOff,varname:node_3757,prsc:2,on:False|A-35-OUT,B-5013-OUT;n:type:ShaderForge.SFN_Multiply,id:143,x:33780,y:32456,varname:node_143,prsc:2|A-7-RGB,B-145-OUT;n:type:ShaderForge.SFN_ValueProperty,id:145,x:33581,y:32543,ptovrint:False,ptlb:Specular_Power,ptin:_Specular_Power,varname:node_2067,prsc:2,glob:False,v1:1;n:type:ShaderForge.SFN_Add,id:163,x:33990,y:32962,varname:node_163,prsc:2|A-132-OUT,B-363-OUT;n:type:ShaderForge.SFN_Fresnel,id:225,x:33581,y:33150,varname:node_225,prsc:2|EXP-102-OUT;n:type:ShaderForge.SFN_Multiply,id:271,x:33905,y:32708,varname:node_271,prsc:2|A-5-RGB,B-273-OUT;n:type:ShaderForge.SFN_ValueProperty,id:273,x:33581,y:32806,ptovrint:False,ptlb:Normal_Intensity,ptin:_Normal_Intensity,varname:node_7283,prsc:2,glob:False,v1:1;n:type:ShaderForge.SFN_Slider,id:331,x:33019,y:33212,ptovrint:False,ptlb:Fresnel_Width,ptin:_Fresnel_Width,varname:node_8559,prsc:2,min:1.5,cur:1.5,max:5;n:type:ShaderForge.SFN_Multiply,id:363,x:33792,y:33223,varname:node_363,prsc:2|A-225-OUT,B-53-RGB;n:type:ShaderForge.SFN_Add,id:9942,x:34138,y:32255,varname:node_9942,prsc:2|A-3-RGB,B-4941-OUT;n:type:ShaderForge.SFN_Multiply,id:4941,x:33943,y:32284,varname:node_4941,prsc:2|A-3-RGB,B-1964-OUT;n:type:ShaderForge.SFN_ValueProperty,id:1964,x:33762,y:32348,ptovrint:False,ptlb:Corrective_Glow,ptin:_Corrective_Glow,varname:node_7740,prsc:2,glob:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:5013,x:33631,y:32981,varname:node_5013,prsc:2|A-9-RGB,B-9052-OUT;n:type:ShaderForge.SFN_ValueProperty,id:9052,x:33467,y:33107,ptovrint:False,ptlb:Emission_Intensity,ptin:_Emission_Intensity,varname:node_9052,prsc:2,glob:False,v1:1;proporder:3-7-145-5-273-132-9-116-331-53-1964-9052;pass:END;sub:END;*/

Shader "Objectify/Fresnel_Pulse" {
    Properties {
        _Diffuse ("Diffuse", 2D) = "white" {}
        _Specular ("Specular", 2D) = "white" {}
        _Specular_Power ("Specular_Power", Float ) = 1
        _Normal ("Normal", 2D) = "bump" {}
        _Normal_Intensity ("Normal_Intensity", Float ) = 1
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
            uniform sampler2D _Normal; uniform float4 _Normal_ST;
            uniform sampler2D _Specular; uniform float4 _Specular_ST;
            uniform sampler2D _Emissive; uniform float4 _Emissive_ST;
            uniform float4 _Fresnel_Color;
            uniform float _Pulse_Speed;
            uniform fixed _Emissive_OnOff;
            uniform float _Specular_Power;
            uniform float _Normal_Intensity;
            uniform float _Fresnel_Width;
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
                float4 node_41 = _Time + _TimeEditor;
                float3 emissive = (lerp( 0.0, (_Emissive_var.rgb*_Emission_Intensity), _Emissive_OnOff )+(pow(1.0-max(0,dot(normalDirection, viewDirection)),(_Fresnel_Width+sin((node_41.g*_Pulse_Speed))))*_Fresnel_Color.rgb));
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
            uniform float4 _TimeEditor;
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform sampler2D _Normal; uniform float4 _Normal_ST;
            uniform sampler2D _Specular; uniform float4 _Specular_ST;
            uniform sampler2D _Emissive; uniform float4 _Emissive_ST;
            uniform float4 _Fresnel_Color;
            uniform float _Pulse_Speed;
            uniform fixed _Emissive_OnOff;
            uniform float _Specular_Power;
            uniform float _Normal_Intensity;
            uniform float _Fresnel_Width;
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
