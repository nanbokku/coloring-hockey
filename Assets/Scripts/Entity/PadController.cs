using UnityEngine;
using Constants;

[RequireComponent(typeof(Pad))]
public class PadController : MonoBehaviour
{
    [SerializeField]
    private Pad pad = null;

    private Vector3 destination = Vector3.zero;

    void Awake()
    {
        destination = this.transform.position;
    }

    void Update()
    {
        // マウスのワールド座標を取得
        Vector3 position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y));

        destination = pad.RestrictDestination(StageData.FloorPosition.z - StageData.FloorRadius, StageData.FloorPosition.z - pad.Radius, position);
    }

    void FixedUpdate()
    {
        pad.MovePosition(destination);
    }
}
