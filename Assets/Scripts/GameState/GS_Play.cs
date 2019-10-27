using UnityEngine;

public class GS_Play : GameStateBase
{
    private InGameUIController inGameUiController;

    public GS_Play(InGameUIController uIController) : base(uIController)
    {
        inGameUiController = uIController;
    }

    public override void Enter()
    {
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
