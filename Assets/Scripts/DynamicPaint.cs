using UnityEngine;

public class DynamicPaint : MonoBehaviour
{
    [SerializeField]
    private Renderer paintRenderer = null;
    [SerializeField]
    private Collider paintCollider = null;
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

    void Awake()
    {
        drawWorldPositionsAndColorNumbersId = Shader.PropertyToID("_DrawWorldPositionsAndColorNumbers");
        currentIndexId = Shader.PropertyToID("_CurrentIndex");
        positionAndColorAryLengthId = Shader.PropertyToID("_PositionAndColorAryLength");

        materialBlock = new MaterialPropertyBlock();

        Clear();
    }

    public Vector3 ClosestPoint(Vector3 position)
    {
        return paintCollider.ClosestPoint(position);
    }

    public void AddDrawPoint(Vector3 point, PlayerType type)
    {
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

        materialBlock.SetVectorArray(drawWorldPositionsAndColorNumbersId, drawWorldPositionsAndColorNumbers);
        materialBlock.SetInt(currentIndexId, currentIndex);
        materialBlock.SetInt(positionAndColorAryLengthId, positionAndColorAryLength);

        paintRenderer.SetPropertyBlock(materialBlock);
    }
}