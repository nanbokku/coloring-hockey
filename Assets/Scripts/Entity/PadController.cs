using UnityEngine;
using Constants;

[RequireComponent(typeof(Pad))]
public class PadController : MonoBehaviour
{
    [SerializeField]
    private Pad pad = null;

    private Vector3 destination = Vector3.zero;
    private bool isActive = false;

    /// <summary>
    /// ゲームをリセットする
    /// </summary>
    public void ResetGame()
    {
        this.transform.position = PlayerData.HumanInitPosition;
    }

    /// <summary>
    /// アクティブ状態を変更する
    /// </summary>
    /// <param name="active"></param>
    public void SetActiveOperation(bool active)
    {
        isActive = active;

        if (isActive == false)
        {
            pad.StopMovement();
        }
    }

    void Awake()
    {
        destination = this.transform.position;
    }

    void Update()
    {
        if (!isActive) return;

        // マウスのワールド座標を取得
        Vector3 position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y));

        // 移動位置の補正
        destination = pad.RestrictDestination(StageData.FloorPosition.z - StageData.FloorRadius, StageData.FloorPosition.z - pad.Radius, position);
    }

    void FixedUpdate()
    {
        if (!isActive) return;

        pad.MovePosition(destination);
    }
}
