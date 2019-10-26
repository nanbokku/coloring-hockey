using UnityEngine.Events;

public abstract class GameSceneStateBase
{
    public UnityAction<GameSceneStateBase> OnStateChanged { get; set; } = null;

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}
