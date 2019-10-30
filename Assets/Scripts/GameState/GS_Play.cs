using UnityEngine;

public class GS_Play : GameStateBase
{
    private InGameUIController inGameUiController = null;
    private GoalArea[] goalAreas = null;

    public GS_Play(InGameUIController uIController) : base(uIController)
    {
        inGameUiController = uIController;
    }

    public override void Enter()
    {
        goalAreas = MonoBehaviour.FindObjectsOfType<GoalArea>();

        foreach (GoalArea area in goalAreas)
        {
            area.OnGoaled = (type) =>
            {
                ScoreStore.Instance.IncrementPoint(type);
            };
        }

        inGameUiController.OnScoreUpdated = () =>
        {
            // TODO: スコアが更新されたらリスタート
        };

        inGameUiController.PlayView();
        Debug.Log("play view");
    }

    public override void Update()
    {

    }

    public override void Exit()
    {

    }
}
