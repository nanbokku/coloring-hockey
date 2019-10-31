using UnityEngine;
using Constants;

[RequireComponent(typeof(Pad))]
public class AirHockeyAI : MonoBehaviour
{
    [SerializeField]
    private Pad pad = null;
    private IHockeyStrategy strategy = null;
    private Vector3 destination = Vector3.zero;

    void Awake()
    {
        destination = this.transform.position;
    }

    public void Initialize(IHockeyStrategy strategy)
    {
        this.strategy = strategy;
    }

    void Update()
    {
        if (strategy == null) return;

        // TODO: 取得方法は？
        Vector3 puckPosition = GameObject.FindGameObjectWithTag("Puck").transform.position;

        destination = strategy.GetDestination(pad, puckPosition);

        destination = pad.RestrictDestination(StageData.FloorPosition.z + pad.Radius, StageData.FloorPosition.z + StageData.FloorRadius, destination);
    }

    void FixedUpdate()
    {
        pad.MovePosition(destination);
    }
}
