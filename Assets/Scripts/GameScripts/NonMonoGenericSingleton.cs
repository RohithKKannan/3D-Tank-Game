namespace BattleTank.Generics
{
    public class NonMonoGenericSingleton<T> where T : NonMonoGenericSingleton<T>
    {
        private static T instance = null;
        public static T Instance { get { return instance; } }

        private void Awake()
        {
            if (instance == null)
            {
                instance = (T)this;
            }
        }
    }
}
