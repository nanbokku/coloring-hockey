using UnityEngine;
using UnityEngine.SceneManagement;
using Constants;

public class GSS_Title : GameSceneStateBase
{
    private TitleUIController uIController = null;

    public override void Enter()
    {
        uIController = MonoBehaviour.FindObjectOfType<TitleUIController>();

        uIController.OnStartBtnClicked = () =>
        {
            SceneManager.LoadScene(SceneName.InGame);
        };

        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    public override void Update()
    {

    }

    public override void Exit()
    {
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    }

    private void OnActiveSceneChanged(Scene before, Scene after)
    {
        OnStateChanged(new GSS_InGame());
    }
}
