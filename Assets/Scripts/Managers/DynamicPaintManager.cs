using UnityEngine;
using MonoBehaviourUtility;

[RequireComponent(typeof(DynamicPaintManager))]
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


    public void AddDrawPoint(Vector3 point, PlayerType type)
    {
        Paint.AddDrawPoint(ClosestPoint(point), type);
    }

    public Vector3 ClosestPoint(Vector3 position)
    {
        return Paint.ClosestPoint(position);
    }

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

    public void ComputeColorRatio()
    {
        // 色の割合を計算
    }
}
