Shader "Custom/InitializeByTextureAndColor"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex CustomRenderTextureVertexShader
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCustomRenderTexture.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;

            fixed4 frag (v2f_customrendertexture i) : SV_Target
            {
                float2 uv = i.globalTexcoord;
                fixed4 col = tex2D(_MainTex, uv) * _Color;
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
