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
