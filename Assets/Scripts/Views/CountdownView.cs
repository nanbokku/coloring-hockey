using UnityEngine;
using UnityEngine.Events;

public class CountdownView : MonoBehaviour
{
    [SerializeField]
    private StartTextAnimation startAnimation = null;

    public UnityAction OnStartAnimFinished { get; set; } = null;

    void Start()
    {
        startAnimation.OnStartAnimFinished = () =>
        {
            OnStartAnimFinished();
        };

        startAnimation.Show();
    }
}
