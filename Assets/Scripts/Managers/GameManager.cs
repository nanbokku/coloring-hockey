using UnityEngine;
using MonoBehaviourUtility;
using UnityEngine.SceneManagement;
using Constants;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    private GameSceneStateBase currentState = null;

    void Start()
    {
        // 解像度を固定する．
        Screen.SetResolution(1024, 768, false, 60);

        // 現在のシーンに応じてゲームステートをセット
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == SceneName.Title)
        {
            ChangeSceneState(new GSS_Title());
        }
        else if (sceneName == SceneName.InGame)
        {
            ChangeSceneState(new GSS_InGame());
        }
        else if (sceneName == SceneName.Result)
        {
            ChangeSceneState(new GSS_Result());
        }
    }

    void Update()
    {
        if (currentState == null) return;

        currentState.Update();
    }

    /// <summary>
    /// ゲームシーンステートを変更する
    /// </summary>
    /// <param name="next">変更するステート</param>
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
            // OnStateChangedイベントが呼ばれたら再帰的に自身を呼ぶ
            ChangeSceneState(state);
        };

        currentState.Enter();
    }
}
