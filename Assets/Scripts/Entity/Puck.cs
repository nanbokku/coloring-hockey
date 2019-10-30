using UnityEngine;

public class Puck : MonoBehaviour
{
    [SerializeField]
    private float restitution = 0;  // TODO: Wallの変数にするべき

    // TODO: rigidbodyで動きを実装するとすり抜けがはげしかったりする

    private float velocity = 0;

    public void SetVelocity(float velocity)
    {
        this.velocity = velocity;
    }

    void OnCollisionEnter(Collision other)
    {
        velocity = restitution * velocity;
    }
}
