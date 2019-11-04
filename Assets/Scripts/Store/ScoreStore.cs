using UnityEngine;
using UnityEngine.Events;

public enum PlayerType
{
    Human, Ai, None
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

    private ScoreStore() { }

    public int GetScore(PlayerType type)
    {
        if (type == PlayerType.Human) return humanScore;
        else return aiScore;
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
    }
}
