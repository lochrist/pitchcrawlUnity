Shader "Sprites/Overlay"
{  
    Properties
    {
       [PerRendererData] _MainTex ("Sprite Texture", 2D) = "red" {}
       _Color ("Color Key", Color) = (0,0,0,1)
       _BrightnessThreshold ("Brightness Threshold", Range (0,1)) = 0.3
       [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
    }
    SubShader
    {
        Tags 
        { 
            "RenderType" = "Transparent" 
            "IgnoreProjector"="True" 
            "Queue" = "Transparent+1" 
            "PreviewType"="Plane"
			"CanUseSpriteAtlas"="False"
        }

        Cull Off
		Lighting Off
		ZWrite Off
		Fog { Mode Off }
        Blend SrcAlpha OneMinusSrcAlpha 

        Pass
        { 
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma multi_compile DUMMY PIXELSNAP_ON
                #include "UnityCG.cginc"
 
                sampler2D _MainTex;
                float4 _Color;
                uniform float _BrightnessThreshold;

                struct Vertex
                {
                    float4 vertex : POSITION;
                    float4 color : COLOR;
                    float2 uv : TEXCOORD0;
                };
    
                struct Fragment
                {
                    float4 vertex : SV_POSITION;
                    fixed4 color : COLOR;
                    half2 uv : TEXCOORD0;
                };
 
                Fragment vert(Vertex v)
                {
                    Fragment o;
                    o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
                    o.color = v.color;
                    o.uv = v.uv;

                    #ifdef PIXELSNAP_ON
				    //o.vertex = UnityPixelSnap(o.vertex);
				    #endif

                    return o;
                }
                                                    
                fixed4 frag(Fragment IN) : COLOR
                {
                    fixed4 o = tex2D (_MainTex, IN.uv);

                    float keyBrightness = (_Color.r + _Color.g + _Color.b) / 3.0f;
                    float texBrightness = (o.r + o.g + o.b) / 3.0f;

                    float dt = smoothstep(0, _BrightnessThreshold, abs(texBrightness - keyBrightness));
                    float x = dt;
                    o.a = x * o.a;
                    
                    return o;
                }

            ENDCG
        }
    }
}