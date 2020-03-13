using UnityEngine;
using UnityEngine.SceneManagement;
using Constants;

public class GSS_Result : GameSceneStateBase
{
    private ResultUIController uIController = null;

    public override void Enter()
    {
        uIController = MonoBehaviour.FindObjectOfType<ResultUIController>();

        // タイトルボタンを押下したらタイトルシーンをロード
        uIController.OnTitleBtnClicked = () =>
        {
            SceneManager.LoadScene(SceneName.Title);
        };

        // シーンをロードしたときのイベントを登録
        SceneManager.activeSceneChanged += OnActiveSceneChanged;

        int humanScore = ScoreStore.Instance.GetScore(PlayerType.Human);
        int aiScore = ScoreStore.Instance.GetScore(PlayerType.Ai);

        if (humanScore >= aiScore)
        {
            uIController.Win();
        }
        else
        {
            uIController.Lose();
        }
    }

    public override void Update()
    {

    }

    public override void Exit()
    {
        ScoreStore.Instance.Initialize();

        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    }

    /// <summary>
    /// シーンをロードしたときのイベント
    /// </summary>
    /// <param name="before"></param>
    /// <param name="after"></param>
    private void OnActiveSceneChanged(Scene before, Scene after)
    {
        // タイトルシーンステートに遷移
        OnStateChanged(new GSS_Title());
    }
}
