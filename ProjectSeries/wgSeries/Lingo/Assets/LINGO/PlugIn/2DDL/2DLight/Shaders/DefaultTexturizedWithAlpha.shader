Shader "2D Dynamic Lights/Texturized/DefaultTexturizedAlphaBlend" {
    Properties 
	{
        _MainTex ("SelfIllum Color (RGB) Alpha (A)", 2D) = "white" {}
    }
    
    Category {
		ZTest always
       Lighting Off
       ZWrite Off
       Fog { Mode Off }
       Blend One One
       Offset -1, -1

       
       
	   BindChannels {
              Bind "Color", color
              Bind "Vertex", vertex
              Bind "TexCoord", texcoord
       }
       
	   SubShader 
	   {
		  Tags { "Queue"="Transparent" "RenderType" = "Opaque" }
           
           
           
             Pass 
			{
				ColorMask RGBA	
				Blend SrcAlpha OneMinusSrcAlpha
			
         		SetTexture [_MainTex] {
             	constantColor [_AlphaColor]
                combine previous lerp(texture) constant DOUBLE, previous lerp(texture) constant
        	}
         
	         // Multiply in texture
	          SetTexture [_MainTex] {
	             combine texture * previous
	          }
			  	
			  	
            }
             
             Pass {
	            Color (1,0,1,0)
	            Cull front
       		}
       
        } 
    }
}