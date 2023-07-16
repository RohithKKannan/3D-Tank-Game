using BattleTank.ScriptableObjects;
using BattleTank.Bullet;

namespace BattleTank.Enemy
{
    public class EnemyModel
    {
        private EnemyController enemyController;

        public int health { get; }
        public int speed { get; }
        public int strength { get; }
        public int bpm { get; }

        public float rotationSpeed { get; }
        public float visibilityRange { get; }
        public float detectionRange { get; }

        public BulletType bulletType { get; }
        public EnemyType enemyType { get; }

        public EnemyModel(EnemyScriptableObject enemy, EnemyType _enemyType)
        {
            health = enemy.health;
            speed = enemy.speed;
            strength = enemy.strength;
            bpm = enemy.bpm;

            rotationSpeed = enemy.speed / 100f;
            visibilityRange = enemy.visibilityRange;
            detectionRange = enemy.detectionRange;

            bulletType = enemy.bulletType;
            enemyType = _enemyType;
        }

        public void SetEnemyController(EnemyController _enemyController)
        {
            enemyController = _enemyController;
        }
    }
}
