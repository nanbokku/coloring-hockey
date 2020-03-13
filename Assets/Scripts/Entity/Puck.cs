using UnityEngine;
using Constants;

public class Puck : MonoBehaviour
{
    [SerializeField]
    private float restitution = 0;  // TODO: Wallの変数にするべき
    [SerializeField]
    private SphereCollider sphereCollider = null;
    [SerializeField]
    private Rigidbody rigidBody = null;

    // TODO: rigidbodyで動きを実装するとすり抜けがはげしかったりする

    private Vector3 lastPosition = Vector3.zero;
    private float velocity = 0;
    private PlayerType lastPlayer = PlayerType.None;
    private bool canDraw = false;

    void Start()
    {
        lastPosition = this.transform.position;
    }

    void FixedUpdate()
    {
        if (!canDraw) return;

        if (Vector3.Distance(lastPosition, this.transform.position) < 0.1f) return;

        // 軌跡をペイントする
        lastPosition = this.transform.position;
        DynamicPaintManager.Instance.AddDrawPoint(this.transform.position, this.lastPlayer);
    }

    void OnEnable()
    {
        canDraw = true;
    }

    void OnDisable()
    {
        canDraw = false;
        UpdateLastCollisionPlayer(PlayerType.None);
        rigidBody.velocity = Vector3.zero;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag != StageData.TagNameOfPad) return;

        DynamicPaintManager.Instance.ChangeColor(other.gameObject, this);
    }

    public void UpdateLastCollisionPlayer(PlayerType type)
    {
        this.lastPlayer = type;
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
