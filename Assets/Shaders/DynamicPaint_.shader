Shader "Custom/DynamicPaint_"
{
    Properties
    {
        _MainTex ("MainTex", 2D) = "white" {}
        _PaintTex ("PaintTexture", 2D) = "white" {}
        _PaintUV ("PaintUV", Vector) = (1, 1, 0, 0)
        _PaintColor ("PaintColor", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma target 5.0
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 worldPos : TEXCOORD1;
                float4 screenPos : TEXCOORD2;
                float4 worldDir : TEXCOORD3;
                // UNITY_FOG_COORDS(2)
                float4 vertex : SV_POSITION;
            };

            // 都度設定する値
            float4 _PaintWorldPosition; // ペイント位置
            float4 _PaintColor; // ペイントカラー
            float4 _PaintUV;

            sampler2D _MainTex;
            sampler2D _PaintTex;
            sampler2D _CameraDepthTexture;
            half _Radius;
            float4x4 _ViewProjInvMat;
            float4 _MainTex_ST;
            float4 _PaintTex_ST;
            float4 _CameraDepthTexture_ST;
            RWTexture2D<float4> _ColorCountTex : register(u2);   // 集計用のテクスチャ

            bool isPaintRange(float3 worldPos)
            {
                float dist = distance(_PaintWorldPosition.xyz, worldPos.xyz);

                return dist < _Radius;
            }

            bool isPaintRange(float2 uv)
            {
                // float radius = 0.5;
                // return _PaintUV.x - radius < uv.x
                //     && uv.x < _PaintUV.x + radius
                //     && _PaintUV.y - radius < uv.y
                //     && uv.y < _PaintUV.y + radius;

                float dist = distance(_PaintUV.xy, uv.xy);

                return dist <= _Radius;
            }

            float paintRange(float4 worldPos)
            {
                return distance(_PaintWorldPosition.xyz, worldPos.xyz);
            }

            float3 getScreenPos(float2 uv, float depth)
            {
                return float3(uv.x * 2.0 - 1.0, uv.y * 2.0 - 1.0, depth);
            }

            float3 getWorldPos(float3 screenPos)
            {
                float4 worldPos = mul(_ViewProjInvMat, float4(screenPos, 1));
                return worldPos.xyz / worldPos.w;
            }

            v2f vert (appdata v)
            {
                v2f o;
                // クリップ座標に変換
                o.vertex = UnityObjectToClipPos(v.vertex);

                // スクリーン座標(0 ~ w)
                o.screenPos = ComputeScreenPos(o.vertex);
                
                // ワールド座標に変換
                float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
                worldPos /= worldPos.w;

                o.worldPos = worldPos;
                o.uv = v.uv2;

                // UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 0 ~ 1の範囲にする
                float2 uvs = i.screenPos.xy / i.screenPos.w;
                float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uvs);

                // depthをテクスチャからカメラの深度に変換する
                depth = LinearEyeDepth(depth);

                float3 worldPos = getWorldPos(getScreenPos(uvs, depth));

                fixed4 col = fixed4(0, 0, 0, 0);
                if (isPaintRange(worldPos)) {
                    // ペイントする
                    float2 uv = TRANSFORM_TEX(i.uv, _PaintTex);
                    col = _PaintColor * tex2Dlod(_PaintTex, float4(uv, 0, 0));
                    col = _PaintColor;
                    // col = fixed4(1, 0, 0, 1);
                } else {
                    // ペイントしない
                    float2 uv = TRANSFORM_TEX(i.uv, _MainTex);
                    col = tex2Dlod(_MainTex, float4(uv, 0, 0));
                    // col = fixed4(0,1,0,1);
                }

                col = float4(worldPos, 1.0);

                // float2 paint = float2(_PaintUV.x, _PaintUV.y);
                // col = fixed4(paint, paint);
                // col = fixed4(uvs, uvs);

                // float xs, xy;
                // _ColorCountTex.GetDimensions(xs, xy);

                // int2 loc = int2(xs * i.uv.x, xy * i.uv.y);
                // _ColorCountTex[loc] = float4(col.x, col.y, col.z, mode);

                // apply fog
                // UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
    Fallback "Diffuse"
}
