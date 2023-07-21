using System;

namespace BattleTank.Events
{
    public class EventService
    {
        private static EventService instance = null;
        public static EventService Instance
        {
            get
            {
                if (instance == null)
                    instance = new EventService();

                return instance;
            }
        }

        public event Action<int> OnEnemyDestroy;
        public event Action<int> OnPlayerFiredBullet;
        public event Action<float> OnDistanceTravelled;

        public void InvokeEnemyDestroy(int enemiesDestroyedCount)
        {
            OnEnemyDestroy?.Invoke(enemiesDestroyedCount);
        }

        public void InvokePlayerFiredBullet(int bulletCount)
        {
            OnPlayerFiredBullet?.Invoke(bulletCount);
        }

        public void InvokeDistanceTravelled(float distance)
        {
            OnDistanceTravelled?.Invoke(distance);
        }
    }
}
