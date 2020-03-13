using UnityEngine.Events;

public abstract class GameSceneStateBase
{
    /// <summary>
    /// 現在のステートが終了したときに呼ばれる
    /// </summary>
    /// <value></value>
    public UnityAction<GameSceneStateBase> OnStateChanged { get; set; } = null;

    /// <summary>
    /// ステートに入ったときに呼ばれる
    /// </summary>
    public abstract void Enter();
    /// <summary>
    /// このステートで毎フレーム呼ばれる
    /// </summary>
    public abstract void Update();
    /// <summary>
    /// ステートを終了するときに呼ばれる
    /// </summary>
    public abstract void Exit();
}
