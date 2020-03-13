using UnityEngine;
using Constants;

[RequireComponent(typeof(Pad))]
public class AirHockeyAI : MonoBehaviour
{
    [SerializeField]
    private Pad pad = null;
    private IHockeyStrategy strategy = null;
    private Vector3 destination = Vector3.zero;
    private Transform puck = null;
    private bool isActive = false;

    void Awake()
    {
        destination = this.transform.position;
    }

    /// <summary>
    /// AIの初期化
    /// </summary>
    /// <param name="strategy"></param>
    public void Initialize(IHockeyStrategy strategy)
    {
        this.strategy = strategy;
    }

    /// <summary>
    /// ゲームをリセットする
    /// </summary>
    /// <param name="puck"></param>
    public void ResetGame(GameObject puck)
    {
        this.transform.position = PlayerData.AiInitPosition;
        this.puck = puck.transform;
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

    void Update()
    {
        if (!isActive) return;
        if (strategy == null) return;

        // 移動先を決定
        destination = strategy.GetDestination(pad, puck.position);

        // 移動先の補正
        destination = pad.RestrictDestination(StageData.FloorPosition.z + pad.Radius, StageData.FloorPosition.z + StageData.FloorRadius, destination);
    }

    void FixedUpdate()
    {
        if (!isActive) return;

        pad.MovePosition(destination);
    }
}
