using UnityEngine;

public class GS_Countdown : GameStateBase
{
    private InGameUIController inGameUiController = null;

    public override void Enter()
    {
        inGameUiController = MonoBehaviour.FindObjectOfType<InGameUIController>();

        inGameUiController.Initialize();
    }

    public override void Update()
    {

    }

    public override void Exit()
    {

    }
}
