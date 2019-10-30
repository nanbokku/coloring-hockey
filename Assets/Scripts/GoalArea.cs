using UnityEngine;
using UnityEngine.Events;
using Constants;

public class GoalArea : MonoBehaviour
{
    [SerializeField]
    private PlayerType score = PlayerType.Human;

    public UnityAction<PlayerType> OnGoaled { get; set; } = null;

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != StageData.TagNameOfPuck) return;

        if (OnGoaled != null) OnGoaled(score);
    }
}
