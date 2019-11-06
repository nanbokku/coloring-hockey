using UnityEngine;
using MonoBehaviourUtility;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    private GameSceneStateBase currentState = null;

    void Start()
    {
        ChangeSceneState(new GSS_Title());
    }

    void Update()
    {
        if (currentState == null) return;

        currentState.Update();
    }

    void ChangeSceneState(GameSceneStateBase next)
    {
        if (currentState != null)
        {
            currentState.Exit();
            currentState.OnStateChanged = null;
        }

        currentState = next;
        currentState.OnStateChanged = (state) =>
        {
            ChangeSceneState(state);
        };

        currentState.Enter();
    }
}
