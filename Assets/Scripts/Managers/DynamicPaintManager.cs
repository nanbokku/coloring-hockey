using UnityEngine;
using MonoBehaviourUtility;

[RequireComponent(typeof(DynamicPaint))]
public class DynamicPaintManager : SingletonMonoBehaviour<DynamicPaintManager>
{
    private DynamicPaint paint = null;
    private DynamicPaint Paint
    {
        get
        {
            if (paint == null)
            {
                paint = GetComponent<DynamicPaint>();
            }

            return paint;
        }
    }

    [SerializeField]
    private GameObject humanPadObj = null;
    [SerializeField]
    private GameObject aiPadObj = null;

    /// <summary>
    /// ペイント位置を追加する
    /// </summary>
    /// <param name="point">ペイント位置</param>
    /// <param name="type">プレイヤータイプ</param>
    public void AddDrawPoint(Vector3 point, PlayerType type)
    {
        Paint.Paint(ClosestPoint(point), type);
    }

    /// <summary>
    /// ペイント対象のコライダーに一番近い位置を返す
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public Vector3 ClosestPoint(Vector3 position)
    {
        return Paint.ClosestPoint(position);
    }

    /// <summary>
    /// ペイント対象のテクスチャをリセットする
    /// </summary>
    public void Clear()
    {
        Paint.Clear();
    }

    /// <summary>
    /// puckが塗りつぶす色を変更する
    /// </summary>
    /// <param name="pad">puckに当たったpad</param>
    /// <param name="puck">対象のpuck</param>
    public void ChangeColor(GameObject pad, Puck puck)
    {
        if (pad == humanPadObj)
        {
            puck.UpdateLastCollisionPlayer(PlayerType.Human);
        }
        else if (pad == aiPadObj)
        {
            puck.UpdateLastCollisionPlayer(PlayerType.Ai);
        }
        else
        {
            puck.UpdateLastCollisionPlayer(PlayerType.None);
        }
    }

    /// <summary>
    /// 色の割合を計算
    /// </summary>
    /// <param name="type">取得する色のプレイヤータイプ</param>
    /// <returns>色の割合</returns>
    public float ComputeColorRatio(PlayerType type)
    {
        return Paint.ComputeColorRatio(type);
    }
}
