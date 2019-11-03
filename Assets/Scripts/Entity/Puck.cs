using UnityEngine;

public class Puck : MonoBehaviour
{
    [SerializeField]
    private float restitution = 0;  // TODO: Wallの変数にするべき
    [SerializeField]
    private SphereCollider sphereCollider = null;

    // TODO: rigidbodyで動きを実装するとすり抜けがはげしかったりする

    private Vector3 lastPosition = Vector3.zero;

    private float velocity = 0;
    private DynamicPaint paint = null;
    private int layerMask = 0;

    void Awake()
    {
        paint = FindObjectOfType<DynamicPaint>();
        layerMask = 1 << LayerMask.NameToLayer("Floor");
    }

    void Start()
    {
        lastPosition = this.transform.position;
    }

    void Update()
    {
        // TODO: 通った道筋に色塗り（最後に触れたプレイヤーの色）
        paint.AddDrawPoint(paint.ClosestPoint(this.transform.position), PlayerType.Human);
    }

    // void FixedUpdate()
    // {
    //     // 前フレームの位置から現在位置間で衝突するか調べる
    //     Ray ray = new Ray(lastPosition, this.transform.position);

    //     RaycastHit hit;
    //     bool isHit = Physics.SphereCast(ray.origin, sphereCollider.radius * this.transform.localScale.x, ray.direction, out hit, Vector3.Distance(lastPosition, this.transform.position));

    //     if (isHit)
    //     {
    //         // 衝突を検知したときは，位置を衝突前に戻す
    //         this.transform.position = hit.point - ray.direction * sphereCollider.radius * this.transform.localScale.x;
    //     }

    //     lastPosition = this.transform.position;
    // }

    // public void SetVelocity(float velocity)
    // {
    //     this.velocity = velocity;
    // }

    // void OnCollisionEnter(Collision other)
    // {
    //     velocity = restitution * velocity;
    // }
}
