using UnityEngine;

public interface IHockeyStrategy
{
    Vector3 GetDestination(Pad pad, Vector3 puckPosition);
}