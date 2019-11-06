using System;

public static class PlayerTypeExtension
{
    public static PlayerType Opposite(this PlayerType type)
    {
        switch (type)
        {
            case PlayerType.Human:
                return PlayerType.Ai;
            case PlayerType.Ai:
                return PlayerType.Human;
            default:
                return PlayerType.None;
        }
    }
}