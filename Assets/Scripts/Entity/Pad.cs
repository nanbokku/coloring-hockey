using UnityEngine;
using Constants;

public class Pad : MonoBehaviour
{
    [SerializeField]
    private MeshFilter meshFilter = null;
    [SerializeField]
    private Rigidbody rigidBody = null;

    /// <summary>
    /// パッドの半径
    /// </summary>
    /// <value></value>
    public float Radius { get; private set; } = 0;

    private float positionY = 0;
    private float floorCenterOffset = 0;

    void Awake()
    {
        positionY = this.transform.position.y;

        this.rigidBody.isKinematic = true;

        // メッシュ情報から半径を取得
        Radius = meshFilter.mesh.bounds.size.x * transform.localScale.x / 2.0f;
        floorCenterOffset = StageData.FloorRadius - Radius;
    }

    /// <summary>
    /// 移動を止める
    /// </summary>
    public void StopMovement()
    {
        this.rigidBody.velocity = Vector3.zero;
    }

    /// <summary>
    /// 位置を移動する
    /// </summary>
    /// <param name="destination"></param>
    public void MovePosition(Vector3 destination)
    {
        this.rigidBody.MovePosition(destination);
    }

    /// <summary>
    /// 移動先を制限する
    /// </summary>
    /// <param name="minZ">最小Z座標</param>
    /// <param name="maxZ">最大Z座標</param>
    /// <param name="position">位置</param>
    /// <returns></returns>
    public Vector3 RestrictDestination(float minZ, float maxZ, Vector3 position)
    {
        position.y = positionY;

        // Y座標をPadと合わせる
        Vector3 floorPosition = StageData.FloorPosition;
        floorPosition.y = positionY;

        // Z座標を制限する
        float min = Mathf.Max(minZ, StageData.FloorPosition.z - floorCenterOffset);
        float max = Mathf.Min(maxZ, StageData.FloorPosition.z + floorCenterOffset);
        position.z = Mathf.Clamp(position.z, min, max);

        // floorの外にpadが出た場合の処理
        float distance = (position - floorPosition).magnitude;
        if (distance > floorCenterOffset)
        {
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
