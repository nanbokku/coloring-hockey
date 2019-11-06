using UnityEngine;
using UnityEngine.SceneManagement;
using Constants;

public class GSS_InGame : GameSceneStateBase
{
    private InGameUIController inGameUiController = null;
    private GameStateBase currentState = null;
    private bool isGameFinished = false;

    public override void Enter()
    {
        inGameUiController = MonoBehaviour.FindObjectOfType<InGameUIController>();

        SceneManager.activeSceneChanged += OnActiveSceneChanged;

        ChangeState(new GS_Countdown(inGameUiController));
    }

    public override void Update()
    {
        if (currentState != null)
        {
            currentState.Update();
        }

        if (isGameFinished)
        {
            isGameFinished = false;
            SceneManager.LoadScene(SceneName.Result);
        }
    }

    public override void Exit()
    {
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    }

    private void OnActiveSceneChanged(Scene before, Scene after)
    {
        OnStateChanged(new GSS_Result());
    }

    private void ChangeState(GameStateBase next)
    {
        if (currentState != null)
        {
            currentState.Exit();
            currentState.OnStateChanged = null;
        }

        if (next == null)
        {
            isGameFinished = true;
            currentState = null;
            return;
        }

        currentState = next;
        currentState.OnStateChanged = (state) =>
        {
            ChangeState(state);
        };

        currentState.Enter();
    }
}
