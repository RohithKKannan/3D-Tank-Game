using UnityEngine;
using UnityEngine.UI;
using BattleTank.ScriptableObjects;

namespace BattleTank.Enemy
{
    public class EnemyController
    {
        public EnemyModel enemyModel { get; }
        public EnemyView enemyView { get; }

        private int health;
        private Transform playerTransform;
        private Transform enemyTransform;

        private Camera playerCamera;
        private Canvas enemyCanvas;
        private Slider healthBar;

        public EnemyController(EnemyScriptableObject enemyData, EnemyType enemyType, Camera _playerCamera, Canvas _enemyCanvas)
        {
            enemyView = GameObject.Instantiate<EnemyView>(enemyData.enemyView);
            enemyModel = new EnemyModel(enemyData, enemyType);

            enemyView.SetEnemyController(this);
            enemyModel.SetEnemyController(this);

            health = enemyModel.health;
            playerCamera = _playerCamera;
            enemyCanvas = _enemyCanvas;
            healthBar = enemyView.GetHealthBar();
            enemyTransform = enemyView.transform;

            SetupHealthBar();
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

            healthBar.value = health;

            if (health <= 0)
                EnemyDeath();
        }

        private void EnemyDeath()
        {
            EnemyService.Instance.DestoryEnemy(this, enemyModel.enemyType);
        }

        public void EnableEnemyTank(Transform _playerTransform, Vector3 _newPosition)
        {
            health = enemyModel.health;
            healthBar.value = enemyModel.health;
            healthBar.gameObject.SetActive(true);

            playerTransform = _playerTransform;
            enemyView.transform.position = _newPosition;
            enemyView.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

            enemyView.gameObject.SetActive(true);
        }

        public void DisableEnemyTank()
        {
            healthBar.transform.localPosition = Vector3.zero;
            healthBar.gameObject.SetActive(false);
            enemyView.gameObject.SetActive(false);
        }

        private void SetupHealthBar()
        {
            healthBar.transform.SetParent(enemyCanvas.transform, false);
            healthBar.maxValue = enemyModel.health;
        }

        public void UpdateHealthBar()
        {
            healthBar.transform.LookAt(healthBar.transform.position + playerCamera.transform.rotation * Vector3.back, playerCamera.transform.rotation * Vector3.up);
            healthBar.transform.localPosition = new Vector3(enemyTransform.position.x, 9, enemyTransform.position.z);
        }
    }
}
