using UnityEngine;

namespace Constants
{
    public static class StageData
    {
        public const string LayerNameOfFloor = "Floor";
        public const string LayerNameOfWall = "Wall";

        public const string TagNameOfWall = "Wall";
        public const string TagNameOfPuck = "Puck";

        public const float FloorRadius = 5;
        public static readonly Vector3 FloorPosition = new Vector3(0, 0, 0);

        public static readonly GameObject PuckPrefab = Resources.Load<GameObject>("AirHockeyPuck");
        public static readonly Vector3 PuckInitPosition = new Vector3(0f, 0.6f, 0f);
        public static readonly Quaternion PuckInitRotation = Quaternion.Euler(-90, 0, 0);
    }
}