using UnityEngine;

namespace BattleTank.Generics
{
    public class GenericSingleton<T> : MonoBehaviour where T : GenericSingleton<T>
    {
        private static T instance = null;
        public static T Instance { get { return instance; } }

        protected virtual void Awake()
        {
            if (instance != null)
            {
                Destroy(this.gameObject);
            }
            else
            {
                instance = (T)this;
                DontDestroyOnLoad(this.gameObject);
            }
        }
    }
}
