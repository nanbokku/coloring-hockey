using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class DynamicPaint : MonoBehaviour
{
    [SerializeField]
    private Renderer paintRenderer = null;
    [SerializeField]
    private Collider paintCollider = null;
    [SerializeField]
    private Color color1 = Color.white;
    [SerializeField]
    private Color color2 = Color.white;
    [SerializeField]
    private Color defaultColor = Color.white;
    [SerializeField]
    private Texture mainTexture = null;
    [SerializeField]
    private Texture defaultTexture = null;
    private MaterialPropertyBlock materialBlock = null;
    [SerializeField]
    private SpriteRenderer sprite;
    [SerializeField]
    private ComputeShader colorCountShader = null;

    /// <summary>
    /// ワールド座標(xyz)とカラーナンバー(w)を格納する配列
    /// </summary>
    private Vector4[] drawWorldPositionsAndColorNumbers = new Vector4[1023];
    /// <summary>
    /// drawWorldPositionAndColorNumbersにおける現在のインデックス
    /// </summary>
    private int currentIndex = 0;
    /// <summary>
    /// drawWorldpositionsAndColorNumbersの長さ
    /// </summary>
    private int positionAndColorAryLength = 0;

    // shader property id
    private int drawWorldPositionsAndColorNumbersId = Shader.PropertyToID("_DrawWorldPositionsAndColorNumbers");
    private int currentIndexId = Shader.PropertyToID("_CurrentIndex");
    private int positionAndColorAryLengthId = Shader.PropertyToID("_PositionAndColorAryLength");
    // 色が塗られた距離を保存
    private Dictionary<PlayerType, float> colorRecord = new Dictionary<PlayerType, float>();

    // private ComputeBuffer buffer = null;
    // private RenderTexture colorCountTexture = null;
    private ColorInfo lastColorInfo = new ColorInfo();
    // private int kernelId = 0;
    // private int colorCountTexId = Shader.PropertyToID("_ColorCountTex");
    // private int bufferId = Shader.PropertyToID("_Result");

    private class ColorInfo
    {
        public PlayerType Color { get; set; } = PlayerType.None;
        public Vector3 Position { get; set; } = Vector3.zero;
    }

    void Awake()
    {
        materialBlock = new MaterialPropertyBlock();

        // インスペクターから指定した値をシェーダーにセット
        int color1Id = Shader.PropertyToID("_Color1");
        int color2Id = Shader.PropertyToID("_Color2");
        int defaultColorId = Shader.PropertyToID("_DefaultColor");
        int mainTextureId = Shader.PropertyToID("_MainTex");
        int defaultTextureId = Shader.PropertyToID("_DefaultTex");

        materialBlock.SetColor(color1Id, color1);
        materialBlock.SetColor(color2Id, color2);
        materialBlock.SetColor(defaultColorId, defaultColor);
        materialBlock.SetTexture(mainTextureId, mainTexture);
        materialBlock.SetTexture(defaultTextureId, defaultTexture);

        // // 集計用シェーダーの設定
        // kernelId = colorCountShader.FindKernel("CountColor");
        // colorCountShader.SetVector("_Color1", color1);
        // colorCountShader.SetVector("_Color2", color2);

        Clear();
    }

    public Vector3 ClosestPoint(Vector3 position)
    {
        return paintCollider.ClosestPoint(position);
    }

    public void AddDrawPoint(Vector3 point, PlayerType type)
    {
        // Graphics.SetRandomWriteTarget(2, colorCountTexture);

        Vector4 point4 = point;

        if (type == PlayerType.Human)
        {
            point4.w = 1;
        }
        else if (type == PlayerType.Ai)
        {
            point4.w = 2;
        }
        else
        {
            // default color
            point4.w = 0;
        }

        // 登録されていない場合は追加
        if (!colorRecord.ContainsKey(type)) colorRecord.Add(type, 0);

        if (lastColorInfo.Color == type)
        {
            // 最後に塗られた色が一致した場合は距離を求める
            float dist = Vector3.Distance(point, lastColorInfo.Position);

            // 距離分を足す
            colorRecord[type] += dist;

            // ColorInfoの更新
            lastColorInfo.Position = point;
        }
        else
        {
            // 最後に塗られた色が違う場合はColorInfoの更新だけ行う
            lastColorInfo.Color = type;
            lastColorInfo.Position = point;
        }

        drawWorldPositionsAndColorNumbers[currentIndex] = point4;

        if (positionAndColorAryLength < 1023) positionAndColorAryLength++;

        materialBlock.SetVectorArray(drawWorldPositionsAndColorNumbersId, drawWorldPositionsAndColorNumbers);
        materialBlock.SetInt(currentIndexId, currentIndex);
        materialBlock.SetInt(positionAndColorAryLengthId, positionAndColorAryLength);

        paintRenderer.SetPropertyBlock(materialBlock);

        if (++currentIndex >= 1023)
        {
            currentIndex = 0;
        }
    }

    // void OnPostRender()
    // {
    //     // Graphics.ClearRandomWriteTargets();
    //     Graphics.SetRandomWriteTarget(2, colorCountTexture);

    //     var tempTex = RenderTexture.GetTemporary(colorCountTexture.width, colorCountTexture.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
    //     Graphics.Blit(colorCountTexture, tempTex, paintRenderer.material);
    //     Graphics.Blit(tempTex, colorCountTexture);
    //     RenderTexture.ReleaseTemporary(tempTex);

    //     // Graphics.ClearRandomWriteTargets();
    // }

    public void Clear()
    {
        drawWorldPositionsAndColorNumbers = new Vector4[1023];
        currentIndex = 0;
        positionAndColorAryLength = 0;
        colorRecord = new Dictionary<PlayerType, float>();
        lastColorInfo = new ColorInfo();

        materialBlock.SetVectorArray(drawWorldPositionsAndColorNumbersId, drawWorldPositionsAndColorNumbers);
        materialBlock.SetInt(currentIndexId, currentIndex);
        materialBlock.SetInt(positionAndColorAryLengthId, positionAndColorAryLength);

        paintRenderer.SetPropertyBlock(materialBlock);

        // // バッファの初期化
        // if (buffer != null) buffer.Release();
        // // buffer = new ComputeBuffer(3, sizeof(int));
        // buffer = new ComputeBuffer(8 * 16, sizeof(float) * 4);

        // // 集計用テクスチャの生成
        // if (colorCountTexture != null) colorCountTexture.Release();
        // colorCountTexture = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGB32);
        // colorCountTexture.hideFlags = HideFlags.HideAndDontSave;
        // colorCountTexture.enableRandomWrite = true;
        // colorCountTexture.filterMode = FilterMode.Point;
        // colorCountTexture.Create();

        // Graphics.SetRenderTarget(colorCountTexture);

        // colorCountShader.SetBuffer(kernelId, bufferId, buffer);
        // colorCountShader.SetTexture(kernelId, "_Source", colorCountTexture);

        // var commandBuffer = new CommandBuffer();
        // int tempId = Shader.PropertyToID("_TempTex");
        // commandBuffer.GetTemporaryRT(tempId, -1, -1);

        // commandBuffer.Blit(BuiltinRenderTextureType.CurrentActive, tempId);
        // commandBuffer.Blit(tempId, colorCountTexture);

        // commandBuffer.ReleaseTemporaryRT(tempId);
        // Camera.main.AddCommandBuffer(CameraEvent.AfterEverything, commandBuffer);

        // Graphics.SetRandomWriteTarget(2, colorCountTexture);
    }

    // private int[] GetColorCount()
    // {
    //     // buffer.SetData(new int[3]);
    //     buffer.SetData(new Vector4[8 * 16]);
    //     colorCountShader.Dispatch(kernelId, 16, 16, 1);

    //     // int[] count = new int[3];
    //     Vector4[] count = new Vector4[8 * 16];
    //     buffer.GetData(count);

    //     foreach (var c in count)
    //     {
    //         Debug.Log(c);
    //     }

    //     // return count;
    //     return new int[3];
    // }

    public void ComputeColorRatio(PlayerType type, UnityAction<float> callback)
    {
        IEnumerator coroutine = ComputeColorRatioAction(type, callback);
        StartCoroutine(coroutine);
    }

    public float ComputeColorRatio(PlayerType type)
    {
        float targetCount = 0, otherCount = 0;
        if (colorRecord.ContainsKey(type)) targetCount = colorRecord[type];
        if (colorRecord.ContainsKey(type.Opposite())) otherCount = colorRecord[type.Opposite()];

        float sum = targetCount + otherCount;
        return (targetCount / sum);
    }

    IEnumerator ComputeColorRatioAction(PlayerType type, UnityAction<float> callback)
    {
        // // フロアーの範囲をキャプチャ
        // Vector3 left = StageData.FloorPosition;
        // left.x -= StageData.FloorRadius;
        // Vector3 right = StageData.FloorPosition;
        // right.x += StageData.FloorRadius;
        // Vector3 top = StageData.FloorPosition;
        // top.z += StageData.FloorRadius;
        // Vector3 bottom = StageData.FloorPosition;
        // bottom.z -= StageData.FloorRadius;

        // // スクリーン座標に変換
        // left = Camera.main.WorldToScreenPoint(left);
        // right = Camera.main.WorldToScreenPoint(right);
        // top = Camera.main.WorldToScreenPoint(top);
        // bottom = Camera.main.WorldToScreenPoint(bottom);
        // int width = Mathf.FloorToInt(right.x - left.x);
        // int height = Mathf.FloorToInt(top.y - bottom.y);

        // Texture2D texture = new Texture2D(width, height, TextureFormat.RGB24, false);

        // // レンダリング完了まで待つ
        // yield return new WaitForEndOfFrame();

        // // キャプチャ
        // Rect captureRange = new Rect(left.x, bottom.y, width, height);
        // texture.ReadPixels(captureRange, 0, 0);
        // texture.Apply();

        // sprite.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        // Color[] colors = texture.GetPixels();

        // yield break;

        // // _MainTexをTexture2Dにキャスト
        // Texture2D texture = paintRenderer.material.mainTexture as Texture2D;

        // Color[] colors = texture.GetPixels();
        // int[] colorCount = new int[2] { 0, 0 };
        // foreach (Color color in colors)
        // {
        //     if (color == color1)
        //     {
        //         colorCount[0]++;
        //     }
        //     else if (color == color2)
        //     {
        //         colorCount[1]++;
        //     }
        // }
        yield return null;

        // int[] count = GetColorCount();
        // float sum = count[(int)PlayerType.Human] + count[(int)PlayerType.Ai];
        // callback(count[(int)type] / sum);

        float targetCount = 0, otherCount = 0;
        if (colorRecord.ContainsKey(type)) targetCount = colorRecord[type];
        if (colorRecord.ContainsKey(type.Opposite())) otherCount = colorRecord[type.Opposite()];

        float sum = targetCount + otherCount;
        callback(targetCount / sum);

        // float sum = colorCount[0] + colorCount[1];
        // if (type == PlayerType.Human)
        // {
        //     callback(colorCount[0] / sum);
        //     // yield return colorCount[0] / sum;
        // }
        // else
        // {
        //     callback(colorCount[1] / sum);
        //     // yield return colorCount[1] / sum;
        // }
    }
}