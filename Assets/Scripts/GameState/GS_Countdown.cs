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

        // カウントダウンが終了したら次のステートへ遷移する
        inGameUiController.OnCountdownFinished = () =>
        {
            OnStateChanged(new GS_Play(inGameUiController));
        };

        // カウントダウンの開始
        inGameUiController.CountdownView();
    }

    public override void Update()
    {

    }

    public override void Exit()
    {

    }
}
