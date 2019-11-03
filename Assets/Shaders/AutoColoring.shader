Shader "Unlit/AutoColoring"
{
    Properties
    {
        _MainTex ("Texture1", 2D) = "white" {}
        _DefaultTex ("DefaultTexture", 2D) = "white" {}
        _Color1 ("Color1", Color) = (1, 1, 1, 1)
        _Color2 ("Color2", Color) = (1, 1, 1, 1)
        _DefaultColor ("DefaultColor", Color) = (1, 1, 1, 1)
        _Radius ("Radius", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 worldPos : TEXCOORD1;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _DefaultTex;
            float4 _Color1;
            float4 _Color2;
            float4 _DefaultColor;
            half _Radius;
            float4 _DrawWorldPosition1[2000];
            float4 _DrawWorldPosition2[2000];
            int _CurrentWorldPosition1;
            int _CurrentWorldPosition2;
            float4 _MainTex_ST;
            float4 _DefaultTex_ST;

            int findIndex(float4 worldPosAry[2000], int currentPos, float4 worldPos)
            {
                for(int i=0; i<currentPos; i++) {
                    half dist = distance(worldPosAry[i].xyz, worldPos.xyz);
                    if (dist < _Radius) return i;
                }

                return -1;
            }

            bool equals(float4 a, float4 b) {
                return a.x == b.x && a.y == b.y && a.z == b.z && a.w == b.w;
            }

            v2f vert (appdata v)
            {
                v2f o;
                // クリップ座標に変換
                o.vertex = UnityObjectToClipPos(v.vertex);
                
                // ワールド座標に変換
                float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
                worldPos /= worldPos.w;

                o.worldPos = worldPos;
                o.uv = v.uv;

                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                int index1 = findIndex(_DrawWorldPosition1, _CurrentWorldPosition1, i.worldPos);
                int index2 = findIndex(_DrawWorldPosition2, _CurrentWorldPosition2, i.worldPos);

                fixed4 col = fixed4(0, 0, 0, 0);
                if (index1 < 0 && index2 < 0) {
                    // 上塗りしない場合
                    float2 uv = TRANSFORM_TEX(i.uv, _DefaultTex);
                    col = _DefaultColor * tex2D(_DefaultTex, uv);
                } else if (index1 > index2) {
                    // _MainTex1が上になる
                    float2 uv = TRANSFORM_TEX(i.uv, _MainTex);
                    col = _Color1 * tex2D(_MainTex, uv);
                } else {
                    // _MainTex2が上になる
                    float2 uv = TRANSFORM_TEX(i.uv, _MainTex);  
                    col = _Color2 * tex2D(_MainTex, uv);
                }
                
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
    // Fallback "Diffuse"
}
