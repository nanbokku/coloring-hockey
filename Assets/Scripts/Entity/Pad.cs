using UnityEngine;
using Constants;

public class Pad : MonoBehaviour
{
    [SerializeField]
    private MeshFilter meshFilter = null;
    [SerializeField]
    private Rigidbody rigidBody = null;

    public float Radius { get; private set; } = 0;

    private float positionY = 0;
    private float floorCenterOffset = 0;

    void Awake()
    {
        positionY = this.transform.position.y;

        this.rigidBody.isKinematic = true;

        Radius = meshFilter.mesh.bounds.size.x * transform.localScale.x / 2.0f;
        floorCenterOffset = StageData.FloorRadius - Radius;
    }

    public void MovePosition(Vector3 destination)
    {
        this.rigidBody.MovePosition(destination);
    }

    public Vector3 RestrictDestination(float minZ, float maxZ, Vector3 position)
    {
        position.y = positionY;

        // Y座標をPadと合わせる
        Vector3 floorPosition = StageData.FloorPosition;
        floorPosition.y = positionY;

        // floorの外にpadが出た場合の処理
        float distance = (position - floorPosition).magnitude;
        if (distance > floorCenterOffset)
        {
            // Z座標を制限する
            float min = Mathf.Max(minZ, StageData.FloorPosition.z - floorCenterOffset);
            float max = Mathf.Min(maxZ, StageData.FloorPosition.z + floorCenterOffset);
            position.z = Mathf.Clamp(position.z, min, max);

            float sin = position.z / floorCenterOffset;
            float theta = Mathf.Asin(sin);

            if (position.x < StageData.FloorPosition.x)
            {
                theta = theta > 0 ? Mathf.PI - theta : Mathf.PI + theta;
            }

            position.x = floorCenterOffset * Mathf.Cos(theta);
        }

        return position;
    }
}
