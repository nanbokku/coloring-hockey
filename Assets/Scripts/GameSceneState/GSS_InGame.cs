using UnityEngine;
using UnityEngine.SceneManagement;

public class GSS_InGame : GameSceneStateBase
{
    public override void Enter()
    {
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
        // OnStateChanged(new GSS_InGame());
    }
}
