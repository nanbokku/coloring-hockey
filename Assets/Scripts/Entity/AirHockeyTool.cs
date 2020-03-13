using UnityEngine;
using Constants;

public class AirHockeyTool : MonoBehaviour
{
    [SerializeField]
    private MeshFilter meshFilter = null;

    /// <summary>
    /// ステージの外側に出れるか
    /// </summary>
    /// <value></value>
    public bool CanGoOut { get; set; } = false;

    private float radiusX = 0;

    void Awake()
    {
        // メッシュ情報から半径を取得
        radiusX = meshFilter.mesh.bounds.size.x * this.transform.localScale.x / 2.0f;
    }

    void OnCollisionEnter(Collision other)
    {
        // 外側に出れる場合は無視
        if (CanGoOut) return;

        if (other.gameObject.tag == StageData.TagNameOfWall)
        {
            // 衝突したcollider上のFloorの中心位置に向かって一番近い位置
            Vector3 point = Physics.ClosestPoint(StageData.FloorPosition, other.collider, other.collider.transform.position, other.transform.rotation);

            Vector3 floorPosition = StageData.FloorPosition;

            // Y座標を合わせる
            point.y = this.transform.position.y;
            floorPosition.y = this.transform.position.y;

            Vector3 direction = (StageData.FloorPosition - point).normalized;

            // ステージの外側に出ないようにする
            this.transform.position = point + direction * radiusX;
        }
    }
}
