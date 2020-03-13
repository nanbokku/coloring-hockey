using UnityEngine;
using Constants;

public class HS_Normal : IHockeyStrategy
{
    private const float Speed = 10f;

    public Vector3 GetDestination(Pad pad, Vector3 puckPosition)
    {
        float distance = Vector3.Distance(PlayerData.AiInitPosition, puckPosition);

        // 距離が遠い場合は初期位置に向かう
        if (distance > DistanceFromPuckToGoal(pad))
        {
            Vector3 relativePosition = PlayerData.AiInitPosition - pad.transform.position;

            if (relativePosition.magnitude < Speed * Time.deltaTime)
            {
                return PlayerData.AiInitPosition;
            }
            else
            {
                return pad.transform.position + relativePosition.normalized * Speed * Time.deltaTime;
            }
        }
        else
        {
            // Y座標をそろえる  
            puckPosition.y = pad.transform.position.y;

            Vector3 direction = (puckPosition - pad.transform.position).normalized;

            return pad.transform.position + direction * Speed * Time.deltaTime;
        }
    }

    /// <summary>
    /// パックに向かっていく最大距離
    /// </summary>
    /// <param name="pad"></param>
    /// <returns></returns>
    private float DistanceFromPuckToGoal(Pad pad)
    {
        return StageData.FloorRadius + pad.Radius * 2;
    }
}