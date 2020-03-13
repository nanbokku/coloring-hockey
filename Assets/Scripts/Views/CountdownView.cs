using UnityEngine;
using UnityEngine.Events;

public class CountdownView : MonoBehaviour
{
    [SerializeField]
    private StartTextAnimation startAnimation = null;

    /// <summary>
    /// アニメーションが終了したときのイベント
    /// </summary>
    /// <value></value>
    public UnityAction OnStartAnimFinished { get; set; } = null;

    void Start()
    {
        // アニメーションが終了したらイベントを発火
        startAnimation.OnStartAnimFinished = () =>
        {
            OnStartAnimFinished();
        };

        startAnimation.Show();
    }
}
