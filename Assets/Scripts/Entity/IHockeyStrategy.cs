using UnityEngine;

public interface IHockeyStrategy
{
    /// <summary>
    /// 移動先の決定
    /// </summary>
    /// <param name="pad">パッドオブジェクト</param>
    /// <param name="puckPosition">パックの位置</param>
    /// <returns></returns>
    Vector3 GetDestination(Pad pad, Vector3 puckPosition);
}