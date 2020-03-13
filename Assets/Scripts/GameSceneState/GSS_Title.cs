using UnityEngine;
using UnityEngine.SceneManagement;
using Constants;

public class GSS_Title : GameSceneStateBase
{
    private TitleUIController uIController = null;

    public override void Enter()
    {
        uIController = MonoBehaviour.FindObjectOfType<TitleUIController>();

        // スタートボタンを押下したらInGameシーンをロード
        uIController.OnStartBtnClicked = () =>
        {
            SceneManager.LoadScene(SceneName.InGame);
        };

        // シーンをロードしたときのイベントを登録
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    public override void Update()
    {

    }

    public override void Exit()
    {
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    }

    /// <summary>
    /// シーンをロードしたときの処理
    /// </summary>
    /// <param name="before"></param>
    /// <param name="after"></param>
    private void OnActiveSceneChanged(Scene before, Scene after)
    {
        // InGameシーンステートに遷移する
        OnStateChanged(new GSS_InGame());
    }
}
