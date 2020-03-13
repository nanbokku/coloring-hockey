using UnityEngine.Events;

public abstract class GameStateBase
{
    public GameStateBase(InGameUIController uIController)
    { }

    /// <summary>
    /// ステートが変更された時に呼ばれるイベント
    /// </summary>
    /// <value></value>
    public UnityAction<GameStateBase> OnStateChanged { get; set; }

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
