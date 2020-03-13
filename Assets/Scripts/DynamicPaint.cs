using UnityEngine;

public class DynamicPaint : MonoBehaviour
{
    /// <summary>
    /// ペイント対象のレンダラー
    /// </summary>
    [SerializeField]
    private Renderer paintRenderer = null;
    /// <summary>
    /// ペイント対象のコライダ―
    /// </summary>
    [SerializeField]
    private Collider paintCollider = null;
    /// <summary>
    /// ペイント用マテリアル
    /// </summary>
    [SerializeField]
    private Material paintMat = null;
    /// <summary>
    /// プレイヤー1の塗り色
    /// </summary>
    [SerializeField]
    private Color color1 = Color.white;
    /// <summary>
    /// プレイヤー2の塗り色
    /// </summary>
    [SerializeField]
    private Color color2 = Color.white;
    /// <summary>
    /// ペイントする半径
    /// </summary>
    [SerializeField]
    private float brushRadius = 0.5f;
    /// <summary>
    /// ペイント対象のメッシュ
    /// </summary>
    [SerializeField]
    private Mesh mesh = null;
    /// <summary>
    /// 集計用シェーダー
    /// </summary>
    [SerializeField]
    private ComputeShader colorCountShader = null;
    /// <summary>
    /// 頂点マップ作成用のマテリアル
    /// </summary>
    [SerializeField]
    private Material vertexMapMat = null;
    /// <summary>
    /// ペイントするテクスチャ（メインテクスチャに設定される）
    /// </summary>
    private CustomRenderTexture paintTexture = null;
    /// <summary>
    /// 頂点マップ
    /// </summary>
    private RenderTexture vertexMap = null;

    public Renderer forDebug = null;

    // shader property id
    private int radiusId = Shader.PropertyToID("_Radius");
    private int paintColorId = Shader.PropertyToID("_PaintColor");
    private int paintWorldPositionId = Shader.PropertyToID("_PaintWorldPosition");
    private int objectToWorldMatId = Shader.PropertyToID("_ObjectToWorldMat");

    /// <summary>
    /// 集計用バッファ
    /// </summary>
    private ComputeBuffer buffer = null;
    /// <summary>
    /// 集計用テクスチャ
    /// </summary>
    private RenderTexture colorCountTexture = null;
    private int kernelId = 0;
    private int colorCountTexId = Shader.PropertyToID("_ColorCountTex");
    private int bufferId = Shader.PropertyToID("_Result");

    void Start()
    {
        Texture mainTexture = paintRenderer.material.mainTexture;

        // メインテクスチャの解像度が低い場合は固定値
        int width = mainTexture.width;
        int height = mainTexture.height;
        if (width < 256 || height < 256)
        {
            width = 256;
            height = 256;
        }

        // ペイントするテクスチャを作成
        paintTexture = new CustomRenderTexture(width, height, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default)
        {
            material = paintMat,
            initializationSource = CustomRenderTextureInitializationSource.TextureAndColor,
            initializationColor = Color.white,
            initializationTexture = mainTexture,
            initializationMode = CustomRenderTextureUpdateMode.OnDemand,
            updateMode = CustomRenderTextureUpdateMode.OnDemand,
            doubleBuffered = true
        };
        paintTexture.Create();

        // 頂点マップを作成
        vertexMap = new RenderTexture(width, height, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Default)
        {
            filterMode = FilterMode.Point
        };

        // 頂点マップの初期化
        SetVertexMap();

        // 集計用テクスチャの生成
        colorCountTexture = new RenderTexture(paintTexture.width, paintTexture.height, 0, RenderTextureFormat.ARGB32)
        {
            hideFlags = HideFlags.HideAndDontSave,
            enableRandomWrite = true,
            filterMode = FilterMode.Point
        };
        colorCountTexture.Create();

        forDebug.material.mainTexture = colorCountTexture;

        // 集計用シェーダーの設定
        kernelId = colorCountShader.FindKernel("CountColor");
        int color1Id = Shader.PropertyToID("_Color1");
        int color2Id = Shader.PropertyToID("_Color2");
        colorCountShader.SetVector(color1Id, color1);
        colorCountShader.SetVector(color2Id, color2);

        Clear(false);
    }

    void OnDestroy()
    {
        buffer.Release();
    }

    /// <summary>
    /// 頂点マップをセットする
    /// </summary>
    private void SetVertexMap()
    {
        // DrawMeshNow()で使用するシェーダーパスを指定
        vertexMapMat.SetPass(0);

        // 描画対象を頂点マップにする
        Graphics.SetRenderTarget(vertexMap);

        // RenderTextureに書き込み
        Graphics.DrawMeshNow(mesh, Matrix4x4.identity);

        int vertexMapId = Shader.PropertyToID("_VertexMap");
        paintMat.SetTexture(vertexMapId, vertexMap);
    }

    /// <summary>
    /// コライダーから一番近い点を返す
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public Vector3 ClosestPoint(Vector3 position)
    {
        return paintCollider.ClosestPoint(position);
    }

    /// <summary>
    /// ペイントする
    /// </summary>
    /// <param name="position">ペイントする位置</param>
    /// <param name="type">プレイヤータイプ</param>
    public void Paint(Vector3 position, PlayerType type)
    {
        Vector4 point4 = position;

        if (type == PlayerType.Human)
        {
            paintMat.SetColor(paintColorId, color1);
        }
        else if (type == PlayerType.Ai)
        {
            paintMat.SetColor(paintColorId, color2);
        }

        paintMat.SetFloat(radiusId, brushRadius);
        paintMat.SetVector(paintWorldPositionId, point4);
        paintMat.SetMatrix(objectToWorldMatId, paintRenderer.localToWorldMatrix);

        // RWTextureに書き込むための設定
        Graphics.ClearRandomWriteTargets();
        Graphics.SetRandomWriteTarget(1, colorCountTexture);

        paintTexture.Update(1);
    }

    /// <summary>
    /// ペイント用テクスチャをリセット
    /// </summary>
    /// <param name="recreate">テクスチャを再生成するか</param>
    private void ResetPaintTexture(bool recreate)
    {
        if (recreate && paintTexture != null)
        {
            // CustomRenderTexture.Initialize()では_SelfTexture2Dが初期化されないため，新たにCustomRenderTextureを作成する
            var tmpTexture = new CustomRenderTexture(paintTexture.width, paintTexture.height, paintTexture.format, RenderTextureReadWrite.Default)
            {
                material = paintTexture.material,
                initializationSource = paintTexture.initializationSource,
                initializationColor = paintTexture.initializationColor,
                initializationTexture = paintTexture.initializationTexture,
                initializationMode = paintTexture.initializationMode,
                updateMode = paintTexture.updateMode,
                doubleBuffered = paintTexture.doubleBuffered
            };
            tmpTexture.Create();
            paintTexture.Release();
            paintTexture = tmpTexture;
        }

        paintTexture.Initialize();

        // メインテクスチャにセット
        paintRenderer.material.mainTexture = paintTexture;
    }

    /// <summary>
    /// コンピュートバッファーをリセット
    /// </summary>
    private void ResetComputeBuffer()
    {
        // バッファの初期化
        if (buffer != null) buffer.Release();
        buffer = new ComputeBuffer(3, sizeof(int));

        // 集計用シェーダーに値をセット
        colorCountShader.SetBuffer(kernelId, bufferId, buffer);
    }

    /// <summary>
    /// 色集計用のテクスチャをリセット
    /// </summary>
    /// <param name="initialize">テクスチャの初期化</param>
    private void ResetColorCountTexture(bool initialize)
    {
        if (initialize && colorCountTexture != null)
        {
            // 集計用テクスチャの初期化
            var tmp = RenderTexture.GetTemporary(colorCountTexture.descriptor);
            Graphics.Blit(tmp, colorCountTexture);
            RenderTexture.ReleaseTemporary(tmp);
        }

        // 集計用シェーダーに値をセット
        colorCountShader.SetTexture(kernelId, "_Source", colorCountTexture);
    }

    /// <summary>
    /// ペイントされたテクスチャをリセットする
    /// </summary>
    public void Clear(bool recreate = true)
    {
        ResetPaintTexture(recreate);
        ResetComputeBuffer();
        ResetColorCountTexture(recreate);
    }

    /// <summary>
    /// 色が塗られたピクセルをカウントする
    /// </summary>
    /// <returns>各色毎の集計結果配列（配列インデックスは(int)PlayerType）</returns>
    private int[] GetColorCount()
    {
        buffer.SetData(new int[3]);
        colorCountShader.Dispatch(kernelId, 16, 16, 1);

        int[] count = new int[3];
        buffer.GetData(count);

        return count;
    }

    /// <summary>
    /// 色の割合を計算
    /// </summary>
    /// <param name="type">取得する色のプレイヤータイプ</param>
    /// <returns>色の割合</returns>
    public float ComputeColorRatio(PlayerType type)
    {
        int[] count = GetColorCount();
        float sum = count[(int)PlayerType.Human] + count[(int)PlayerType.Ai];
        float ratio = count[(int)type] / sum;

        return ratio;
    }
}