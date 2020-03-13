Shader "Custom/DynamicPaint"
{
    Properties
    {
        _PaintTex ("PaintTexture", 2D) = "white" {}
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
            #pragma vertex CustomRenderTextureVertexShader // vertex shaderは定義されているものを使う
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCustomRenderTexture.cginc"

            // 都度設定する値
            float4 _PaintWorldPosition; // ペイント位置
            float4 _PaintColor; // ペイントカラー
            float4x4 _ObjectToWorldMat; // ローカルからワールド座標系への変換行列

            sampler2D _PaintTex;
            float4 _PaintTex_ST;
            half _Radius;
            sampler2D _VertexMap;   // 頂点マップ
            RWTexture2D<float4> _ColorCountTex : register(u1);   // 集計用のテクスチャ

            bool isPaintRange(float3 worldPos)
            {
                float dist = distance(_PaintWorldPosition.xyz, worldPos);

                return dist < _Radius;
            }

            float2 calcPaintUV(float3 worldPos)
            {
                float dist = distance(_PaintWorldPosition.xyz, worldPos);
                float ratio = dist / _Radius;

                // 向きを計算（XY平面）
                fixed2 dir = worldPos.xy / (worldPos.z + 1e-4) - _PaintWorldPosition.xy / (_PaintWorldPosition.z + 1e-4);

                // UV座標を計算
                float2 uv = sign(dir) * ratio;
                uv = (uv + 1.0) * 0.5;

                return uv;                
            }

            fixed4 frag (v2f_customrendertexture i) : SV_Target
            {
                // 頂点座標からワールド座標を取得
                float2 uv = i.globalTexcoord;
                float4 vertex = float4(tex2D(_VertexMap, uv));
                float3 worldPos = mul(_ObjectToWorldMat, vertex);

                // 前フレームのテクスチャ
                fixed4 col = tex2D(_SelfTexture2D, uv);
                if (isPaintRange(worldPos)) {
                    // ペイントする
                    float2 paintUV = calcPaintUV(worldPos);
                    paintUV = TRANSFORM_TEX(paintUV, _PaintTex);
                    fixed4 paintCol = _PaintColor * tex2D(_PaintTex, paintUV);

                    col = lerp(paintCol, col, paintCol.a);

                    // ペイントした色を記録する
                    float xs, ys;
                    _ColorCountTex.GetDimensions(xs, ys);

                    int2 loc = int2(xs * uv.x, ys * uv.y);
                    _ColorCountTex[loc] = _PaintColor;
                }

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
    Fallback "Diffuse"
}
