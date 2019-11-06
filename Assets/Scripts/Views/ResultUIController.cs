using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class ResultUIController : MonoBehaviour
{
    [SerializeField]
    private GameObject backgroundPink = null;
    [SerializeField]
    private GameObject backgroundBlue = null;
    [SerializeField]
    private TextMeshProUGUI winText = null;
    [SerializeField]
    private TextMeshProUGUI loseText = null;
    [SerializeField]
    private TextMeshProUGUI scoreText = null;
    [SerializeField]
    private Button TitleBtn = null;

    public UnityAction OnTitleBtnClicked { get; set; } = null;

    void Start()
    {
        TitleBtn.onClick.AddListener(() =>
        {
            OnTitleBtnClicked();
        });
    }

    public void Win()
    {
        int score = ScoreStore.Instance.GetScore(PlayerType.Human);

        scoreText.text = "SCORE : " + score.ToString();

        backgroundPink.SetActive(true);
        backgroundBlue.SetActive(false);

        winText.gameObject.SetActive(true);
        loseText.gameObject.SetActive(false);
    }

    public void Lose()
    {
        int score = ScoreStore.Instance.GetScore(PlayerType.Human);

        scoreText.text = "SCORE : " + score.ToString();

        backgroundPink.SetActive(false);
        backgroundBlue.SetActive(true);

        winText.gameObject.SetActive(false);
        loseText.gameObject.SetActive(true);
    }
}
