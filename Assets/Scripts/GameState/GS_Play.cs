using UnityEngine;
using Constants;

public class GS_Play : GameStateBase
{
    private InGameUIController inGameUiController = null;
    private GoalArea[] goalAreas = null;
    private AirHockeyAI ai = null;
    private PadController player = null;
    private GameObject puck = null;

    public GS_Play(InGameUIController uIController) : base(uIController)
    {
        inGameUiController = uIController;
    }

    public override void Enter()
    {
        goalAreas = MonoBehaviour.FindObjectsOfType<GoalArea>();
        ai = MonoBehaviour.FindObjectOfType<AirHockeyAI>();
        player = MonoBehaviour.FindObjectOfType<PadController>();

        foreach (GoalArea area in goalAreas)
        {
            area.OnGoaled = (type) =>
            {
                puck.SetActive(false);
                ai.SetActiveOperation(false);
                player.SetActiveOperation(false);

                ScoreStore.Instance.IncrementPoint(type);
            };
        }

        inGameUiController.OnScoreUpdated = () =>
        {
            Reset();
        };

        ai.Initialize(new HS_Normal());
        inGameUiController.PlayView();
        Reset();
    }

    public override void Update()
    {

    }

    public override void Exit()
    {

    }

    private void Reset()
    {
        if (puck == null)
        {
            puck = MonoBehaviour.Instantiate(StageData.PuckPrefab, StageData.PuckInitPosition, StageData.PuckInitRotation);
        }
        else
        {
            puck.transform.position = StageData.PuckInitPosition;
            puck.transform.rotation = StageData.PuckInitRotation;
            puck.SetActive(true);
        }

        ai.ResetGame(puck);
        player.ResetGame();

        ai.SetActiveOperation(true);
        player.SetActiveOperation(true);

        DynamicPaintManager.Instance.Clear();
    }
}
