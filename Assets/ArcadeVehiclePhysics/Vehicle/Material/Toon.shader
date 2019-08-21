Shader "Custom/Toon"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Face texture", 2D) = "white" {}
        _LightStrength ("Light brightness", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Cartoon fullforwardshadows addshadow vertex:vert
        #pragma target 3.0

        sampler2D _MainTex, _BodyTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        float _LightStrength;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void vert (inout appdata_full v, out Input o) 
        {
            UNITY_INITIALIZE_OUTPUT(Input,o);
        }

        half4 LightingCartoon (SurfaceOutput s, half3 lightDir, half atten) 
        {
            half NdotL = dot (s.Normal, lightDir);
            half diff = NdotL * 0.5 + 0.5;
            diff = smoothstep(0.49,0.51,diff);
            atten = smoothstep(0.59,0.61,atten);
            half4 c;
            c.rgb = s.Albedo * saturate(_LightColor0.rgb * diff * atten * _LightStrength);
            c.a = s.Alpha;
            return c;
        }

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 body = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            
            o.Albedo = body.rgb;            
        }
        ENDCG
    }
    FallBack "Diffuse"
}
