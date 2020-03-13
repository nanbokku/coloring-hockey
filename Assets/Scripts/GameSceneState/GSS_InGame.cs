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

        // シーンをロードしたときのイベントを登録
        SceneManager.activeSceneChanged += OnActiveSceneChanged;

        // ゲームステートの初期化
        ChangeState(new GS_Countdown(inGameUiController));
    }

    public override void Update()
    {
        if (currentState != null)
        {
            currentState.Update();
        }

        // ゲーム終了フラグが立っていたらリザルトシーンに遷移
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

    /// <summary>
    /// シーンをロードしたときのイベント
    /// </summary>
    /// <param name="before"></param>
    /// <param name="after"></param>
    private void OnActiveSceneChanged(Scene before, Scene after)
    {
        OnStateChanged(new GSS_Result());
    }

    /// <summary>
    /// シーンステートを変更する
    /// </summary>
    /// <param name="next">次のゲームシーンステート</param>
    private void ChangeState(GameStateBase next)
    {
        if (currentState != null)
        {
            currentState.Exit();
            currentState.OnStateChanged = null;
        }

        // 次のステートがなかったらゲーム終了
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
