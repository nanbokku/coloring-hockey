using UnityEngine;
using UnityEngine.SceneManagement;
using Constants;

public class GSS_Result : GameSceneStateBase
{
    private ResultUIController uIController = null;

    public override void Enter()
    {
        uIController = MonoBehaviour.FindObjectOfType<ResultUIController>();

        uIController.OnTitleBtnClicked = () =>
        {
            SceneManager.LoadScene(SceneName.Title);
        };

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

    private void OnActiveSceneChanged(Scene before, Scene after)
    {
        OnStateChanged(new GSS_Title());
    }
}
