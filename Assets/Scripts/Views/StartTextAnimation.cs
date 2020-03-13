using UnityEngine;
using UnityEngine.Events;

public class StartTextAnimation : MonoBehaviour
{
    [SerializeField]
    private Animator startAnimator = null;

    /// <summary>
    /// アニメーションが終了したときのイベント
    /// </summary>
    /// <value></value>
    public UnityAction OnStartAnimFinished { get; set; } = null;

    private const string StartAnimTriggerStr = "show";

    public void Show()
    {
        startAnimator.SetTrigger(StartAnimTriggerStr);
    }

    /// <summary>
    /// アニメーションが終了したときに呼ばれるメソッド
    /// </summary>
    public void StartAnimFinished()
    {
        OnStartAnimFinished();
    }
}
