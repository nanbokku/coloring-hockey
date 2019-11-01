using UnityEngine;
using UnityEngine.Events;
using Constants;

public class GoalArea : MonoBehaviour
{
    [SerializeField]
    private PlayerType score = PlayerType.Human;
    [SerializeField]
    private Vector3 goalDirection = Vector3.zero;

    public UnityAction<PlayerType> OnGoaled { get; set; } = null;

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != StageData.TagNameOfPuck) return;

        Rigidbody rb = other.GetComponent<Rigidbody>();
        Vector3 direction = rb.velocity.normalized;

        // ゴール方向にpuckが抜けていかないときは無視
        if (Vector3.Angle(direction, goalDirection) > 90f) return;

        if (OnGoaled != null) OnGoaled(score);
    }
}
