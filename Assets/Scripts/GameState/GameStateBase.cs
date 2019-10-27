using UnityEngine.Events;

public abstract class GameStateBase
{
    public UnityAction<GameStateBase> OnStateChanged { get; set; }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}
