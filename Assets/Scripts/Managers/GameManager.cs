using UnityEngine;
using MonoBehaviourUtility;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    private GameSceneStateBase currentState = null;

    void Start()
    {

    }

    void Update()
    {

    }

    void ChangeSceneState(GameSceneStateBase next)
    {
        if (currentState != null)
        {
            currentState.Exit();
            currentState.OnStateChanged = null;
        }

        currentState.OnStateChanged = (state) =>
        {
            ChangeSceneState(state);
        };

        currentState.Enter();
    }
}
