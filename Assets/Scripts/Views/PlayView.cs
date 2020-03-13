using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayView : MonoBehaviour
{
    [SerializeField]
    private Text playerScoreTxt = null;
    [SerializeField]
    private Text enemyScoreTxt = null;
    [SerializeField]
    private InstantScore instantScore = null;

    /// <summary>
    /// スコアUIを更新したときに呼ばれるイベント
    /// </summary>
    /// <value></value>
    public UnityAction OnScoreUpdated { get; set; } = null;

    void Awake()
    {
        playerScoreTxt.text = 0.ToString();
        enemyScoreTxt.text = 0.ToString();
    }

    void Start()
    {
        // UIが更新されたらイベントを発火
        instantScore.OnScoreAnimFinished = () =>
        {
            OnScoreUpdated();
        };
    }

    /// <summary>
    /// プレイヤーのスコアを更新する
    /// </summary>
    /// <param name="score"></param>
    public void UpdatePlayerScore(int score)
    {
        playerScoreTxt.text = score.ToString();
        instantScore.UpdatePlayerScore(score);
    }

    /// <summary>
    /// エネミーのスコアを更新する
    /// </summary>
    /// <param name="score"></param>
    public void UpdateEnemyScore(int score)
    {
        enemyScoreTxt.text = score.ToString();
        instantScore.UpdateEnemyScore(score);
    }
}
