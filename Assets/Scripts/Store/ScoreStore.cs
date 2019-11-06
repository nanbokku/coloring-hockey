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

    public UnityAction<PlayerType> OnPointIncremented { get; set; }

    private int humanScore = 0;
    private int aiScore = 0;
    public int Round { get; private set; } = 0;

    private ScoreStore() { }

    public void Initialize()
    {
        humanScore = 0;
        aiScore = 0;
        Round = 0;
    }

    public int GetScore(PlayerType type)
    {
        if (type == PlayerType.Human) return humanScore;
        else return aiScore;
    }

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

    public void IncrementPoint(PlayerType type)
    {
        if (type == PlayerType.Human)
        {
            humanScore++;

            // viewに通知
            if (OnPointIncremented != null) OnPointIncremented(PlayerType.Human);
        }
        else
        {
            aiScore++;

            // viewに通知
            if (OnPointIncremented != null) OnPointIncremented(PlayerType.Ai);
        }

        Round++;
    }
}
