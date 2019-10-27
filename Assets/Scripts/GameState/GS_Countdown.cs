using UnityEngine;

public class GS_Countdown : GameStateBase
{
    private InGameUIController inGameUiController = null;

    public GS_Countdown(InGameUIController uIController) : base(uIController)
    {
        inGameUiController = uIController;
    }

    public override void Enter()
    {
        inGameUiController = MonoBehaviour.FindObjectOfType<InGameUIController>();

        inGameUiController.OnCountdownFinished = () =>
        {
            OnStateChanged(new GS_Play(inGameUiController));
        };

        inGameUiController.CountdownView();
    }

    public override void Update()
    {

    }

    public override void Exit()
    {

    }
}
