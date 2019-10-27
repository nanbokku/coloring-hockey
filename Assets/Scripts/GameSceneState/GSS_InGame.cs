using UnityEngine;
using UnityEngine.SceneManagement;

public class GSS_InGame : GameSceneStateBase
{
    private InGameUIController inGameUiController = null;
    private GameStateBase currentState = null;

    public override void Enter()
    {
        inGameUiController = MonoBehaviour.FindObjectOfType<InGameUIController>();

        SceneManager.activeSceneChanged += OnActiveSceneChanged;

        ChangeState(new GS_Countdown(inGameUiController));
    }

    public override void Update()
    {
        if (currentState == null) return;

        currentState.Update();
    }

    public override void Exit()
    {
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    }

    private void OnActiveSceneChanged(Scene before, Scene after)
    {
        // OnStateChanged(new GSS_InGame());
    }

    private void ChangeState(GameStateBase next)
    {
        if (currentState != null)
        {
            currentState.Exit();
            currentState.OnStateChanged = null;
        }

        currentState = next;
        currentState.OnStateChanged = (state) =>
        {
            ChangeState(state);
        };

        currentState.Enter();
    }
}
