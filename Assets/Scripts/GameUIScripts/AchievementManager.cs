using UnityEngine;
using BattleTank.Events;

namespace BattleTank.Achievement
{
    public class AchievementManager : MonoBehaviour
    {
        [SerializeField] private AchievementScript achievementPrefab;
        [SerializeField] private int[] bulletCheckpoints;
        [SerializeField] private int[] enemiesDestroyedCheckpoints;

        void Start()
        {
            EventService.Instance.OnPlayerFiredBullet += PlayerBulletAchievement;
            EventService.Instance.OnDistanceTravelled += DistanceTravelledAchievement;
            EventService.Instance.OnEnemyDestroy += EnemyDeathAchievement;
        }

        public void PlayerBulletAchievement(int bulletCount)
        {
            for (int i = 0; i < bulletCheckpoints.Length; i++)
            {
                if (bulletCheckpoints[i] == bulletCount)
                    UnlockAchievement($"{bulletCount} Bullets fired!");
            }
        }

        public void EnemyDeathAchievement(int enemiesDestroyedCount)
        {
            for (int i = 0; i < enemiesDestroyedCheckpoints.Length; i++)
            {
                if (bulletCheckpoints[i] == enemiesDestroyedCount)
                    UnlockAchievement($"{enemiesDestroyedCount} enemies destroyed!");
            }
        }

        public void DistanceTravelledAchievement(float distance)
        {
            UnlockAchievement($"Distance Travelled {distance}!");
        }

        public void UnlockAchievement(string _achievement)
        {
            AchievementScript newAchievement = Instantiate<AchievementScript>(achievementPrefab);

            newAchievement.transform.SetParent(this.transform);
            newAchievement.SetLocalTransform();
            newAchievement.SetMessage(_achievement);

            newAchievement.ShowcaseAchievement();
        }

        void OnDestroy()
        {
            EventService.Instance.OnPlayerFiredBullet -= PlayerBulletAchievement;
            EventService.Instance.OnDistanceTravelled -= DistanceTravelledAchievement;
            EventService.Instance.OnEnemyDestroy -= EnemyDeathAchievement;
        }
    }
}
