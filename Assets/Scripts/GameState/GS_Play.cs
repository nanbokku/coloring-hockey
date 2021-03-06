﻿using UnityEngine;
using Constants;

public class GS_Play : GameStateBase
{
    private InGameUIController inGameUiController = null;
    private GoalArea[] goalAreas = null;
    private AirHockeyAI ai = null;
    private PadController player = null;
    private GameObject puck = null;
    private bool isGameFinished = false;

    private const int MaxRound = 7;

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
            // ゴールしたときの処理を登録
            area.OnGoaled = (type) =>
            {
                puck.SetActive(false);
                ai.SetActiveOperation(false);
                player.SetActiveOperation(false);

                // 塗られた色の割合を求め，スコアに適用する
                float ratio = DynamicPaintManager.Instance.ComputeColorRatio(type);
                ScoreStore.Instance.IncrementPoint(type, ratio);
            };
        }

        // スコアが更新されたときの処理を登録
        inGameUiController.OnScoreUpdated = () =>
        {
            // 現在のゲームをリセットする
            if (ScoreStore.Instance.Round < MaxRound)
            {
                Reset();
            }
            // 最大ラウンドまでしたら，ゲーム終了
            else
            {
                isGameFinished = true;
            }
        };

        ai.Initialize(new HS_Normal());
        inGameUiController.PlayView();
        Reset();
    }

    public override void Update()
    {
        if (!isGameFinished) return;

        FinishGame();
    }

    public override void Exit()
    {
    }

    /// <summary>
    /// ゲームを終了する
    /// </summary>
    private void FinishGame()
    {
        isGameFinished = false;
        OnStateChanged(null);
    }

    /// <summary>
    /// ゲームをリセットする
    /// </summary>
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
