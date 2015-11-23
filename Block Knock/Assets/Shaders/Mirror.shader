Shader "Example/ScreenPos" {
    Properties {
		_Ref ("Reflection", 2D) = "white" {}     
		_RefMask ("RefMask", 2D) = "white" {}     
		_OutlineColor ("Outline Color", Color) = (0,0,0,1)
		_Outline ("Outline width", Range (.002, 0.03)) = .005
		_MainTex ("Base (RGB)", 2D) = "white" {} 
		
    }
    SubShader {
      Tags { "RenderType" = "Additive" }
      CGPROGRAM
      #include "UnityCG.cginc"
      #pragma surface surf Lambert
      
      //Blend One One
      
      struct Input {     
      	float2 uv_MainTex;     
        float4 screenPos;
      };
      sampler2D _Ref;   
      sampler2D _RefMask;  
      sampler2D _MainTex;
      void surf (Input IN, inout SurfaceOutput o) {
                              
          float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
          screenUV.x -= 0.5;
          screenUV *= float2((_ScreenParams.x / _ScreenParams.y),-1);
          screenUV.x += 0.5;
                    
          o.Albedo = tex2D (_Ref, screenUV).rgb;  
          o.Albedo = lerp(tex2D (_MainTex, IN.uv_MainTex).rgb, o.Albedo, (clamp(IN.screenPos.y / 25 - 0.1, 0, 1)) * tex2D (_RefMask, IN.uv_MainTex).rgb);
          
                  
      }
      ENDCG
    } 
    Fallback "Diffuse"
  }