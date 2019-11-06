using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class InstantScore : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI playerScoreTxt = null;
    [SerializeField]
    private TextMeshProUGUI enemyScoreTxt = null;
    [SerializeField]
    private Animator instantScoreAnimator = null;
    [SerializeField]
    private Color defaultColor = Color.white;
    [SerializeField]
    private Color addedScoreColor = default;
    [SerializeField]
    private TextMeshProUGUI multTxt = null;

    public UnityAction OnScoreAnimFinished { get; set; } = null;

    private const string PlayerScoreAnimTriggerStr = "flash_left";
    private const string EnemyScoreAnimTriggerStr = "flash_right";

    void Awake()
    {
        playerScoreTxt.text = 0.ToString();
        enemyScoreTxt.text = 0.ToString();
    }

    public void UpdatePlayerScore(int score)
    {
        int preScore = System.Convert.ToInt32(playerScoreTxt.text);
        float ratio = (score - preScore) / 10.0f;
        multTxt.text = "×" + ratio;
        playerScoreTxt.text = score.ToString();
        playerScoreTxt.color = addedScoreColor;
        enemyScoreTxt.color = defaultColor;
        instantScoreAnimator.SetTrigger(PlayerScoreAnimTriggerStr);
    }

    public void UpdateEnemyScore(int score)
    {
        int preScore = System.Convert.ToInt32(enemyScoreTxt.text);
        float ratio = (score - preScore) / 10.0f;
        multTxt.text = "×" + ratio;
        enemyScoreTxt.text = score.ToString();
        enemyScoreTxt.color = addedScoreColor;
        playerScoreTxt.color = defaultColor;
        instantScoreAnimator.SetTrigger(EnemyScoreAnimTriggerStr);
    }

    public void ScoreAnimFinished()
    {
        OnScoreAnimFinished();
    }
}
