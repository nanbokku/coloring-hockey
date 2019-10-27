using UnityEngine;
using UnityEngine.Events;

public class StartTextAnimation : MonoBehaviour
{
    [SerializeField]
    private Animator startAnimator = null;

    public UnityAction OnStartAnimFinished { get; set; } = null;

    private const string StartAnimTriggerStr = "show";

    public void Show()
    {
        startAnimator.SetTrigger(StartAnimTriggerStr);
    }

    public void StartAnimFinished()
    {
        OnStartAnimFinished();
    }
}
