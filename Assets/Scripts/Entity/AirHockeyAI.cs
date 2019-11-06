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

    public void Initialize(IHockeyStrategy strategy)
    {
        this.strategy = strategy;
    }

    public void ResetGame(GameObject puck)
    {
        this.transform.position = PlayerData.AiInitPosition;
        this.puck = puck.transform;
    }

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

        destination = strategy.GetDestination(pad, puck.position);

        destination = pad.RestrictDestination(StageData.FloorPosition.z + pad.Radius, StageData.FloorPosition.z + StageData.FloorRadius, destination);
    }

    void FixedUpdate()
    {
        if (!isActive) return;

        pad.MovePosition(destination);
    }
}
