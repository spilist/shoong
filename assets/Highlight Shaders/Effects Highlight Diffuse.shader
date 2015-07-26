// Shader created with Shader Forge v1.13 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.13;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,nrsp:0,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,rprd:False,enco:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,bsrc:0,bdst:1,culm:0,dpts:2,wrdp:True,dith:2,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:9123,x:32987,y:32692,varname:node_9123,prsc:2|diff-4020-OUT,emission-309-OUT,olwid-9071-OUT,olcol-4095-RGB;n:type:ShaderForge.SFN_Tex2d,id:1035,x:32417,y:32095,ptovrint:False,ptlb:Diffuse Map,ptin:_DiffuseMap,varname:node_1035,prsc:2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:8824,x:32667,y:32225,varname:node_8824,prsc:2|A-1035-RGB,B-9096-RGB;n:type:ShaderForge.SFN_Color,id:9096,x:32417,y:32279,ptovrint:False,ptlb:Diffuse Color,ptin:_DiffuseColor,varname:node_9096,prsc:2,glob:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Color,id:8717,x:32224,y:32437,ptovrint:False,ptlb:Highlight Color,ptin:_HighlightColor,varname:node_8717,prsc:2,glob:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Fresnel,id:8198,x:32224,y:32594,varname:node_8198,prsc:2|EXP-534-OUT;n:type:ShaderForge.SFN_Add,id:4020,x:32821,y:32490,varname:node_4020,prsc:2|A-8824-OUT,B-519-OUT;n:type:ShaderForge.SFN_Color,id:4095,x:32765,y:33454,ptovrint:False,ptlb:Outline Color,ptin:_OutlineColor,varname:node_7066,prsc:2,glob:False,c1:0,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_Slider,id:6759,x:32171,y:33293,ptovrint:False,ptlb:Outline Width,ptin:_OutlineWidth,varname:node_6759,prsc:2,min:0,cur:0.3,max:1;n:type:ShaderForge.SFN_Slider,id:1143,x:31654,y:32813,ptovrint:False,ptlb:Highlight Surface,ptin:_HighlightSurface,varname:node_1143,prsc:2,min:0,cur:0.5,max:1;n:type:ShaderForge.SFN_Multiply,id:519,x:32563,y:32513,varname:node_519,prsc:2|A-8717-RGB,B-8198-OUT,C-8198-OUT,D-8198-OUT;n:type:ShaderForge.SFN_Lerp,id:9066,x:32583,y:33186,varname:node_9066,prsc:2|A-5425-OUT,B-129-OUT,T-6759-OUT;n:type:ShaderForge.SFN_Vector1,id:5425,x:32311,y:33094,varname:node_5425,prsc:2,v1:0;n:type:ShaderForge.SFN_Vector1,id:129,x:32311,y:33187,varname:node_129,prsc:2,v1:0.05;n:type:ShaderForge.SFN_ConstantLerp,id:534,x:32036,y:32623,varname:node_534,prsc:2,a:1,b:0|IN-1143-OUT;n:type:ShaderForge.SFN_Slider,id:1106,x:32381,y:32830,ptovrint:False,ptlb:Emissive,ptin:_Emissive,varname:_Highlightsurface_copy_copy,prsc:2,min:0,cur:0.5,max:1;n:type:ShaderForge.SFN_Multiply,id:309,x:32736,y:32771,varname:node_309,prsc:2|A-519-OUT,B-1106-OUT;n:type:ShaderForge.SFN_SwitchProperty,id:9071,x:32774,y:33039,ptovrint:False,ptlb:Outline On/Off,ptin:_OutlineOnOff,varname:node_9071,prsc:2,on:False|A-5425-OUT,B-9066-OUT;proporder:9096-1035-8717-1143-1106-9071-4095-6759;pass:END;sub:END;*/

Shader "Ciconia Studio/Effects/Highlight/Diffuse" {
    Properties {
        _DiffuseColor ("Diffuse Color", Color) = (1,1,1,1)
        _DiffuseMap ("Diffuse Map", 2D) = "white" {}
        _HighlightColor ("Highlight Color", Color) = (1,1,1,1)
        _HighlightSurface ("Highlight Surface", Range(0, 1)) = 0.5
        _Emissive ("Emissive", Range(0, 1)) = 0.5
        [MaterialToggle] _OutlineOnOff ("Outline On/Off", Float ) = 0
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth ("Outline Width", Range(0, 1)) = 0.3
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
            #pragma multi_compile_fog
            #pragma exclude_renderers xbox360 ps3 
            #pragma target 3.0
            uniform float4 _OutlineColor;
            uniform float _OutlineWidth;
            uniform fixed _OutlineOnOff;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                UNITY_FOG_COORDS(0)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                float node_5425 = 0.0;
                o.pos = mul(UNITY_MATRIX_MVP, float4(v.vertex.xyz + v.normal*lerp( node_5425, lerp(node_5425,0.05,_OutlineWidth), _OutlineOnOff ),1));
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
/////// Vectors:
                return fixed4(_OutlineColor.rgb,0);
            }
            ENDCG
        }
        Pass {
            Name "FORWARD"
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
            #pragma multi_compile_fog
            #pragma exclude_renderers xbox360 ps3 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _DiffuseMap; uniform float4 _DiffuseMap_ST;
            uniform float4 _DiffuseColor;
            uniform float4 _HighlightColor;
            uniform float _HighlightSurface;
            uniform float _Emissive;
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
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(_Object2World, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
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
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float4 _DiffuseMap_var = tex2D(_DiffuseMap,TRANSFORM_TEX(i.uv0, _DiffuseMap));
                float node_8198 = pow(1.0-max(0,dot(normalDirection, viewDirection)),lerp(1,0,_HighlightSurface));
                float3 node_519 = (_HighlightColor.rgb*node_8198*node_8198*node_8198);
                float3 diffuseColor = ((_DiffuseMap_var.rgb*_DiffuseColor.rgb)+node_519);
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
////// Emissive:
                float3 emissive = (node_519*_Emissive);
/// Final Color:
                float3 finalColor = diffuse + emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers xbox360 ps3 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _DiffuseMap; uniform float4 _DiffuseMap_ST;
            uniform float4 _DiffuseColor;
            uniform float4 _HighlightColor;
            uniform float _HighlightSurface;
            uniform float _Emissive;
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
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(_Object2World, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
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
                float4 _DiffuseMap_var = tex2D(_DiffuseMap,TRANSFORM_TEX(i.uv0, _DiffuseMap));
                float node_8198 = pow(1.0-max(0,dot(normalDirection, viewDirection)),lerp(1,0,_HighlightSurface));
                float3 node_519 = (_HighlightColor.rgb*node_8198*node_8198*node_8198);
                float3 diffuseColor = ((_DiffuseMap_var.rgb*_DiffuseColor.rgb)+node_519);
                float3 diffuse = directDiffuse * diffuseColor;
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
