using UnityEngine;
using UnityEngine.Events;

public class InGameUIController : MonoBehaviour
{
    [SerializeField]
    private CountdownView countdownView = null;
    [SerializeField]
    private PlayView playView = null;

    public UnityAction OnCountdownFinished { get; set; } = null;
    public UnityAction OnScoreUpdated { get; set; } = null;

    public void CountdownView()
    {
        countdownView.OnStartAnimFinished = () =>
        {
            OnCountdownFinished();
        };

        countdownView.gameObject.SetActive(true);
        playView.gameObject.SetActive(false);
    }

    public void PlayView()
    {
        // ScoreStoreが更新されたらviewも更新
        ScoreStore.Instance.OnPointIncremented = (type) =>
        {
            if (type == PlayerType.Human)
            {
                playView.UpdatePlayerScore(ScoreStore.Instance.GetScore(type));
            }
            else
            {
                playView.UpdateEnemyScore(ScoreStore.Instance.GetScore(type));
            }
        };

        playView.OnScoreUpdated = () =>
        {
            // TODO: スコア更新アニメーション終了，リスタート
            Debug.Log("score updated");
            OnScoreUpdated();
        };

        countdownView.gameObject.SetActive(false);
        playView.gameObject.SetActive(true);
    }
}
