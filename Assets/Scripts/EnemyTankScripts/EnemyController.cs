using UnityEngine;
using BattleTank.ScriptableObjects;

namespace BattleTank.Enemy
{
    public class EnemyController
    {
        public EnemyModel enemyModel { get; }
        public EnemyView enemyView { get; }

        private int health;
        private Transform playerTransform;

        public EnemyController(EnemyScriptableObject enemyData, EnemyType enemyType)
        {
            enemyView = GameObject.Instantiate<EnemyView>(enemyData.enemyView);
            enemyModel = new EnemyModel(enemyData, enemyType);

            enemyView.SetEnemyController(this);
            enemyModel.SetEnemyController(this);

            health = enemyModel.health;
        }

        public int GetStrength()
        {
            return enemyModel.strength;
        }

        public float GetVisibilityRange()
        {
            return enemyModel.visibilityRange;
        }

        public float GetDetectionRange()
        {
            return enemyModel.detectionRange;
        }

        public float GetBulletsPerMinute()
        {
            return enemyModel.bpm;
        }

        public float GetSpeed()
        {
            return enemyModel.speed;
        }

        public float GetRotationSpeed()
        {
            return enemyModel.rotationSpeed;
        }

        public Vector3 GetPosition()
        {
            return enemyView.transform.position;
        }

        public Transform GetPlayerTransform()
        {
            return playerTransform;
        }

        public EnemyType GetEnemyType()
        {
            return enemyModel.enemyType;
        }

        public void Shoot(Transform gunTransform)
        {
            EnemyService.Instance.ShootBullet(enemyModel.bulletType, gunTransform);
        }

        public void TakeDamage(int damage)
        {
            health -= damage;
            if (health <= 0)
                EnemyDeath();
        }

        private void EnemyDeath()
        {
            EnemyService.Instance.DestoryEnemy(this, enemyModel.enemyType);
        }

        public void EnableEnemyTank(Transform _playerTransform, Vector3 _newPosition)
        {
            playerTransform = _playerTransform;
            enemyView.transform.position = _newPosition;
            enemyView.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

            enemyView.gameObject.SetActive(true);
        }

        public void DisableEnemyTank()
        {
            enemyView.gameObject.SetActive(false);
        }
    }
}
