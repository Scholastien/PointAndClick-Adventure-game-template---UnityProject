// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:4013,x:33443,y:32816,varname:node_4013,prsc:2|emission-6383-OUT;n:type:ShaderForge.SFN_Tex2d,id:7272,x:32407,y:32784,ptovrint:False,ptlb:Clovers,ptin:_Clovers,varname:node_7272,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-2400-OUT;n:type:ShaderForge.SFN_TexCoord,id:5116,x:32017,y:32697,varname:node_5116,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Add,id:2400,x:32219,y:32784,varname:node_2400,prsc:2|A-9430-OUT,B-5116-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:103,x:30988,y:32757,varname:node_103,prsc:2,ntxv:0,isnm:False|UVIN-8866-UVOUT,TEX-9187-TEX;n:type:ShaderForge.SFN_Tex2dAsset,id:9187,x:30988,y:32583,ptovrint:False,ptlb:Wind,ptin:_Wind,varname:node_9187,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Panner,id:8866,x:30749,y:32678,varname:node_8866,prsc:2,spu:0.124,spv:0.1|UVIN-6403-OUT;n:type:ShaderForge.SFN_TexCoord,id:3689,x:30304,y:32723,varname:node_3689,prsc:2,uv:0,uaff:True;n:type:ShaderForge.SFN_RemapRange,id:7901,x:31684,y:32761,varname:node_7901,prsc:2,frmn:0,frmx:1,tomn:-1,tomx:1|IN-7498-OUT;n:type:ShaderForge.SFN_TexCoord,id:356,x:31684,y:33359,varname:node_356,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_RemapRange,id:2284,x:31684,y:33195,varname:node_2284,prsc:2,frmn:0,frmx:1,tomn:-1,tomx:2|IN-356-V;n:type:ShaderForge.SFN_Clamp01,id:7132,x:31684,y:33071,varname:node_7132,prsc:2|IN-2284-OUT;n:type:ShaderForge.SFN_Multiply,id:9430,x:32017,y:32837,varname:node_9430,prsc:2|A-7901-OUT,B-4745-OUT;n:type:ShaderForge.SFN_Multiply,id:4745,x:31684,y:32946,varname:node_4745,prsc:2|A-7132-OUT,B-1566-OUT;n:type:ShaderForge.SFN_SceneColor,id:2205,x:32664,y:32955,varname:node_2205,prsc:2;n:type:ShaderForge.SFN_Tex2d,id:5976,x:32911,y:33038,ptovrint:False,ptlb:Clovers Glow,ptin:_CloversGlow,varname:node_5976,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-2400-OUT;n:type:ShaderForge.SFN_Lerp,id:6973,x:32911,y:32890,varname:node_6973,prsc:2|A-2205-RGB,B-7272-RGB,T-8380-OUT;n:type:ShaderForge.SFN_Blend,id:6383,x:33164,y:32972,varname:node_6383,prsc:2,blmd:6,clmp:True|SRC-6973-OUT,DST-2816-OUT;n:type:ShaderForge.SFN_Multiply,id:2816,x:33164,y:33138,varname:node_2816,prsc:2|A-5976-RGB,B-3352-OUT,C-6156-OUT,D-7596-OUT;n:type:ShaderForge.SFN_RemapRange,id:3352,x:32911,y:33203,varname:node_3352,prsc:2,frmn:0,frmx:1,tomn:0,tomx:1|IN-7628-R;n:type:ShaderForge.SFN_Vector3,id:6156,x:33164,y:33260,varname:node_6156,prsc:2,v1:0.3529412,v2:0.8661259,v3:1;n:type:ShaderForge.SFN_Vector1,id:7596,x:33164,y:33354,varname:node_7596,prsc:2,v1:2;n:type:ShaderForge.SFN_Add,id:6403,x:30539,y:32791,varname:node_6403,prsc:2|A-3689-UVOUT,B-2272-OUT;n:type:ShaderForge.SFN_ObjectPosition,id:1479,x:30121,y:32872,varname:node_1479,prsc:2;n:type:ShaderForge.SFN_Add,id:2272,x:30304,y:32872,varname:node_2272,prsc:2|A-1479-X,B-1479-Y,C-1479-Z;n:type:ShaderForge.SFN_Tex2d,id:5704,x:30988,y:32905,varname:node_5704,prsc:2,ntxv:0,isnm:False|UVIN-955-UVOUT,TEX-9187-TEX;n:type:ShaderForge.SFN_Append,id:7498,x:31263,y:32846,varname:node_7498,prsc:2|A-103-R,B-5704-G;n:type:ShaderForge.SFN_Panner,id:955,x:30749,y:32895,varname:node_955,prsc:2,spu:0.163,spv:0.1323|UVIN-6403-OUT;n:type:ShaderForge.SFN_Vector2,id:1566,x:31508,y:32964,varname:node_1566,prsc:2,v1:0.01,v2:0.04;n:type:ShaderForge.SFN_Tex2d,id:7628,x:30988,y:33077,varname:node_7628,prsc:2,ntxv:0,isnm:False|UVIN-78-OUT,TEX-9187-TEX;n:type:ShaderForge.SFN_RemapRange,id:78,x:30749,y:33094,varname:node_78,prsc:2,frmn:0,frmx:1,tomn:0,tomx:3|IN-1853-UVOUT;n:type:ShaderForge.SFN_Panner,id:1853,x:30569,y:33094,varname:node_1853,prsc:2,spu:0.0453,spv:0.0339|UVIN-6403-OUT;n:type:ShaderForge.SFN_RemapRange,id:4066,x:32361,y:33083,varname:node_4066,prsc:2,frmn:0,frmx:1,tomn:0,tomx:1.3|IN-7272-A;n:type:ShaderForge.SFN_Clamp01,id:8380,x:32533,y:33083,varname:node_8380,prsc:2|IN-4066-OUT;proporder:9187-7272-5976;pass:END;sub:END;*/

Shader "Shader Forge/Clovers" {
    Properties {
        _Wind ("Wind", 2D) = "white" {}
        _Clovers ("Clovers", 2D) = "white" {}
        _CloversGlow ("Clovers Glow", 2D) = "white" {}
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        GrabPass{ }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _GrabTexture;
            uniform sampler2D _Clovers; uniform float4 _Clovers_ST;
            uniform sampler2D _Wind; uniform float4 _Wind_ST;
            uniform sampler2D _CloversGlow; uniform float4 _CloversGlow_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float4 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 uv0 : TEXCOORD0;
                float4 projPos : TEXCOORD1;
                UNITY_FOG_COORDS(2)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                float2 sceneUVs = (i.projPos.xy / i.projPos.w);
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
////// Lighting:
////// Emissive:
                float4 node_4230 = _Time;
                float2 node_6403 = (i.uv0+(objPos.r+objPos.g+objPos.b));
                float2 node_8866 = (node_6403+node_4230.g*float2(0.124,0.1));
                float4 node_103 = tex2D(_Wind,TRANSFORM_TEX(node_8866, _Wind));
                float2 node_955 = (node_6403+node_4230.g*float2(0.163,0.1323));
                float4 node_5704 = tex2D(_Wind,TRANSFORM_TEX(node_955, _Wind));
                float2 node_2400 = (((float2(node_103.r,node_5704.g)*2.0+-1.0)*(saturate((i.uv0.g*3.0+-1.0))*float2(0.01,0.04)))+i.uv0);
                float4 _Clovers_var = tex2D(_Clovers,TRANSFORM_TEX(node_2400, _Clovers));
                float4 _CloversGlow_var = tex2D(_CloversGlow,TRANSFORM_TEX(node_2400, _CloversGlow));
                float2 node_78 = ((node_6403+node_4230.g*float2(0.0453,0.0339))*3.0+0.0);
                float4 node_7628 = tex2D(_Wind,TRANSFORM_TEX(node_78, _Wind));
                float3 emissive = saturate((1.0-(1.0-lerp(sceneColor.rgb,_Clovers_var.rgb,saturate((_Clovers_var.a*1.3+0.0))))*(1.0-(_CloversGlow_var.rgb*(node_7628.r*1.0+0.0)*float3(0.3529412,0.8661259,1)*2.0))));
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
