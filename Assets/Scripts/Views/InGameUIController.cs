using UnityEngine;
using UnityEngine.Events;

public class InGameUIController : MonoBehaviour
{
    [SerializeField]
    private CountdownView countdownView = null;
    [SerializeField]
    private PlayView playView = null;

    /// <summary>
    /// カウントダウン終了時に呼ばれるイベント
    /// </summary>
    /// <value></value>
    public UnityAction OnCountdownFinished { get; set; } = null;
    /// <summary>
    /// スコアが更新されたときのイベント
    /// </summary>
    /// <value></value>
    public UnityAction OnScoreUpdated { get; set; } = null;

    /// <summary>
    /// カウントダウンの表示
    /// </summary>
    public void CountdownView()
    {
        // カウントダウンのアニメーションが終了したときの処理
        countdownView.OnStartAnimFinished = () =>
        {
            OnCountdownFinished();
        };

        countdownView.gameObject.SetActive(true);
        playView.gameObject.SetActive(false);
    }

    /// <summary>
    /// ゲームプレイ時のUIを表示
    /// </summary>
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

        // UIが更新されたらイベントを発火
        playView.OnScoreUpdated = () =>
        {
            OnScoreUpdated();
        };

        countdownView.gameObject.SetActive(false);
        playView.gameObject.SetActive(true);
    }
}
