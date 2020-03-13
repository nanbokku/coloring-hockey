using UnityEngine;
using UnityEngine.Events;

public enum PlayerType
{
    Human = 1,
    Ai = 2,
    None = 0
}

public class ScoreStore
{
    private static ScoreStore singleton = new ScoreStore();
    public static ScoreStore Instance
    {
        get
        {
            return singleton;
        }
    }

    /// <summary>
    /// スコアが加算された時に呼ばれるイベント
    /// </summary>
    /// <value></value>
    public UnityAction<PlayerType> OnPointIncremented { get; set; }

    private int humanScore = 0;
    private int aiScore = 0;
    /// <summary>
    /// 現在のゲームのラウンド数
    /// </summary>
    /// <value></value>
    public int Round { get; private set; } = 0;

    private ScoreStore() { }

    /// <summary>
    /// 初期化する
    /// </summary>
    public void Initialize()
    {
        humanScore = 0;
        aiScore = 0;
        Round = 0;
    }

    /// <summary>
    /// スコアを取得する
    /// </summary>
    /// <param name="type">プレイヤータイプ</param>
    /// <returns></returns>
    public int GetScore(PlayerType type)
    {
        if (type == PlayerType.Human) return humanScore;
        else return aiScore;
    }

    /// <summary>
    /// スコアを加算する
    /// </summary>
    /// <param name="type">プレイヤータイプ</param>
    /// <param name="ratio">加算比率</param>
    public void IncrementPoint(PlayerType type, float ratio)
    {
        int point = (int)(10 * (1 + ratio));

        if (type == PlayerType.Human)
        {
            humanScore += point;

            // viewに通知
            if (OnPointIncremented != null) OnPointIncremented(PlayerType.Human);
        }
        else
        {
            aiScore += point;

            // viewに通知
            if (OnPointIncremented != null) OnPointIncremented(PlayerType.Ai);
        }

        Round++;
    }
}
