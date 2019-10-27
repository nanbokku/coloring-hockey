using UnityEngine;
using UnityEngine.UI;

public class InGameUIController : MonoBehaviour
{
    [SerializeField]
    private CountdownView countdownView = null;
    [SerializeField]
    private PlayView playView = null;

    public void Initialize()
    {
        countdownView.OnStartAnimFinished = () =>
        {
            // TODO: スタート表示が終了，PlayViewの表示
            Debug.Log("finish start");
        };

        countdownView.gameObject.SetActive(true);
    }
}
