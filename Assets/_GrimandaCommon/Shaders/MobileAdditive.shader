Shader "Mobile/ColorAdditiveTexture"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Additive Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" }

        //Blend One Zero
        Cull Back
        Lighting On
            
        CGPROGRAM
        #pragma surface surf Lambert
           
        sampler2D _MainTex;
        float4 _Color;
           
        struct Input {
            float2 uv_MainTex;
            float4 color : Color;
        };

        void vert (inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input,o);
            o.color = _Color;
        }
       
        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb + _Color + _Color;
            o.Alpha = _Color;
        }
           
        ENDCG
    }
}