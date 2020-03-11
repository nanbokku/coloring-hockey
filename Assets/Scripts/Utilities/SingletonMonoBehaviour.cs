using UnityEngine;

namespace MonoBehaviourUtility
{
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
    {
        private static T singleton = null;
        public static T Instance
        {
            get
            {
                if (singleton == null)
                {
                    singleton = FindObjectOfType<T>();

                    if (singleton == null)
                    {
                        Debug.LogError(typeof(T) + " is nothing");
                    }
                }

                return singleton;
            }
        }

        protected virtual void Awake()
        {
            if (Instance != this)
            {
                Destroy(this);
                return;
            }

            this.transform.parent = null;
            DontDestroyOnLoad(this.gameObject);
        }
    }

}