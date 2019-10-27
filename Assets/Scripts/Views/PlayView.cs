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

    public UnityAction OnScoreUpdated { get; set; } = null;

    void Awake()
    {
        playerScoreTxt.text = 0.ToString();
        enemyScoreTxt.text = 0.ToString();
    }

    void Start()
    {
        instantScore.OnScoreAnimFinished = () =>
        {
            OnScoreUpdated();
        };
    }

    public void UpdatePlayerScore(int score)
    {
        playerScoreTxt.text = score.ToString();
        instantScore.UpdatePlayerScore(score);
    }

    public void UpdateEnemyScore(int score)
    {
        enemyScoreTxt.text = score.ToString();
        instantScore.UpdateEnemyScore(score);
    }
}
