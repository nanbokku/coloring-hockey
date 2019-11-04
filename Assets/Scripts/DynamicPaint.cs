using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using Constants;

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
    private int drawWorldPositionsAndColorNumbersId = 0;
    private int currentIndexId = 0;
    private int positionAndColorAryLengthId = 0;
    private int[] colorCount = new int[3] { 0, 0, 0 };

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

        // プロパティIDを取得
        drawWorldPositionsAndColorNumbersId = Shader.PropertyToID("_DrawWorldPositionsAndColorNumbers");
        currentIndexId = Shader.PropertyToID("_CurrentIndex");
        positionAndColorAryLengthId = Shader.PropertyToID("_PositionAndColorAryLength");

        Clear();
    }

    public Vector3 ClosestPoint(Vector3 position)
    {
        return paintCollider.ClosestPoint(position);
    }

    public void AddDrawPoint(Vector3 point, PlayerType type)
    {
        // TODO: カラー集計用に何か計算を追加する
        Vector4 point4 = point;

        if (type == PlayerType.Human)
        {
            point4.w = 1;
            colorCount[1]++;
        }
        else if (type == PlayerType.Ai)
        {
            point4.w = 2;
            colorCount[2]++;
        }
        else
        {
            // default color
            point4.w = 0;
            colorCount[0]++;
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

    public void Clear()
    {
        drawWorldPositionsAndColorNumbers = new Vector4[1023];
        currentIndex = 0;
        positionAndColorAryLength = 0;
        colorCount = new int[3] { 0, 0, 0 };

        materialBlock.SetVectorArray(drawWorldPositionsAndColorNumbersId, drawWorldPositionsAndColorNumbers);
        materialBlock.SetInt(currentIndexId, currentIndex);
        materialBlock.SetInt(positionAndColorAryLengthId, positionAndColorAryLength);

        paintRenderer.SetPropertyBlock(materialBlock);
    }

    public void ComputeColorRatio(PlayerType type, UnityAction<float> callback)
    {
        IEnumerator coroutine = ComputeColorRatioAction(type, callback);
        StartCoroutine(coroutine);
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
        // Rect captureRange = new Rect(left.x, bottom.y, width, height);

        // // レンダリング完了まで待つ
        // yield return new WaitForEndOfFrame();

        // // キャプチャ
        // texture.ReadPixels(captureRange, 0, 0);
        // texture.Apply();

        // Color[] colors = texture.GetPixels();

        yield return null;

        // // _MainTexをTexture2Dにキャスト
        // Texture2D texture = paintRenderer.material.mainTexture as Texture2D;

        // Color[] colors = texture.GetPixels();
        // int[] colorCount = new int[2] { 0, 0 };
        // foreach (Color color in colors)
        // {
        //     Debug.Log(color);
        //     Debug.Log(color1);
        //     if (color == color1)
        //     {
        //         colorCount[0]++;
        //     }
        //     else if (color == color2)
        //     {
        //         colorCount[1]++;
        //     }
        // }

        float sum = colorCount[1] + colorCount[2];
        if (type == PlayerType.Human)
        {
            callback(colorCount[1] / sum);
            // yield return colorCount[0] / sum;
        }
        else
        {
            callback(colorCount[2] / sum);
            // yield return colorCount[1] / sum;
        }
    }
}