using UnityEngine;

namespace Constants
{
    public static class PlayerData
    {
        /// <summary>
        /// 人間の初期位置
        /// </summary>
        /// <returns></returns>
        public static readonly Vector3 HumanInitPosition = new Vector3(0, 0.8f, -4f);
        /// <summary>
        /// AIの初期位置
        /// </summary>
        /// <returns></returns>
        public static readonly Vector3 AiInitPosition = new Vector3(0, 0.8f, 4f);
    }
}