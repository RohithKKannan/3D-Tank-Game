using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleTank.Generics;
using BattleTank.ObjectPool;
using BattleTank.Events;
using BattleTank.ScriptableObjects;
using BattleTank.PlayerTank;
using BattleTank.Bullet;
using BattleTank.PlayerCamera;

namespace BattleTank.Enemy
{
    public enum EnemyType
    {
        Brown, Purple, Cyan
    }

    public class EnemyService : GenericSingleton<EnemyService>
    {
        private int maxEnemyCount = 10;
        private int enemiesDestroyedCount = 0;
        private Transform playerTransform;

        private List<EnemyController> enemies = new();
        private List<Transform> spawnPoints = new();
        private List<Transform> pointsAlreadySpawned = new();

        private BrownEnemyPoolService brownEnemyPoolService = new();
        private PurpleEnemyPoolService purpleEnemyPoolService = new();
        private CyanEnemyPoolService cyanEnemyPoolService = new();

        private TankExplosionPoolService tankExplosionPoolService = new();

        private Camera playerCamera;
        private Canvas enemyUICanvas;
        private Transform SpawnPointParent;

        [SerializeField] private EnemyScriptableObjectList enemyTankList;
        [SerializeField] private ParticleSystem tankExplosion;
        [SerializeField] private int enemyCount = 3;

        public void StartEnemyService(Camera _playerCamera, Canvas _enemyUICanvas, Transform _spawnPointParent)
        {
            playerCamera = _playerCamera;
            enemyUICanvas = _enemyUICanvas;
            SpawnPointParent = _spawnPointParent;

            playerTransform = TankService.Instance.GetPlayerTransform();
            enemyCount = Mathf.Min(enemyCount, maxEnemyCount);

            spawnPoints.Clear();
            foreach (Transform item in SpawnPointParent)
                spawnPoints.Add(item);

            StartCoroutine(SpawnEnemyTanks(enemyCount));
        }

        IEnumerator SpawnEnemyTank()
        {
            yield return new WaitForSeconds(2f);

            Vector3 newPosition = GetExistingSpawnPoint();
            EnemyType enemyType = GetRandomEnemyType();

            if (newPosition == Vector3.zero)
                yield break;

            EnemyController enemyController = CreateEnemyTank(enemyType, newPosition);
            enemies.Add(enemyController);
        }

        IEnumerator SpawnEnemyTanks(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Vector3 newPosition = GetRandomSpawnPoint();
                EnemyType enemyType = GetRandomEnemyType();

                if (newPosition == Vector3.zero)
                    break;

                EnemyController enemyController = CreateEnemyTank(enemyType, newPosition);
                enemies.Add(enemyController);

                yield return new WaitForSeconds(0.1f);
            }
        }

        public Vector3 GetExistingSpawnPoint()
        {
            if (pointsAlreadySpawned.Count == 0)
                return Vector3.zero;

            int spawnPointIndex;
            Transform newSpawnPoint;

            if (!playerTransform)
                return Vector3.zero;

            do
            {
                spawnPointIndex = UnityEngine.Random.Range(0, pointsAlreadySpawned.Count);
                newSpawnPoint = pointsAlreadySpawned[spawnPointIndex];
            }
            while (Vector3.Distance(newSpawnPoint.position, playerTransform.position) < 10f);

            return newSpawnPoint.position;
        }

        public Vector3 GetRandomSpawnPoint()
        {
            if (spawnPoints.Count == 0)
                return Vector3.zero;

            int spawnPointIndex = UnityEngine.Random.Range(0, spawnPoints.Count);
            Transform newSpawnPoint = spawnPoints[spawnPointIndex];

            pointsAlreadySpawned.Add(newSpawnPoint);
            spawnPoints.RemoveAt(spawnPointIndex);

            return newSpawnPoint.position;
        }

        public EnemyType GetRandomEnemyType()
        {
            return (EnemyType)UnityEngine.Random.Range(0, enemyTankList.enemies.Length);
        }

        public EnemyController CreateEnemyTank(EnemyType enemyType, Vector3 newPosition)
        {
            EnemyScriptableObject enemyData = enemyTankList.enemies[(int)enemyType];

            EnemyController enemyController = null;

            switch (enemyType)
            {
                case EnemyType.Brown:
                    enemyController = brownEnemyPoolService.GetEnemy(enemyData, enemyType, playerCamera, enemyUICanvas);
                    break;
                case EnemyType.Purple:
                    enemyController = purpleEnemyPoolService.GetEnemy(enemyData, enemyType, playerCamera, enemyUICanvas);
                    break;
                case EnemyType.Cyan:
                    enemyController = cyanEnemyPoolService.GetEnemy(enemyData, enemyType, playerCamera, enemyUICanvas);
                    break;
                default: break;
            }

            enemyController.EnableEnemyTank(playerTransform, newPosition);

            return enemyController;
        }

        public void ShootBullet(BulletType bulletType, Transform gunTransform)
        {
            BulletService.Instance.SpawnBullet(bulletType, gunTransform, TankType.Enemy);
        }

        public void DestoryEnemy(EnemyController _enemyController, EnemyType _enemyType)
        {
            if (_enemyController.enemyView == null)
                return;

            Vector3 pos = _enemyController.GetPosition();
            _enemyController.DisableEnemyTank();

            switch (_enemyType)
            {
                case EnemyType.Brown:
                    brownEnemyPoolService.ReturnItem(_enemyController);
                    break;
                case EnemyType.Purple:
                    purpleEnemyPoolService.ReturnItem(_enemyController);
                    break;
                case EnemyType.Cyan:
                    cyanEnemyPoolService.ReturnItem(_enemyController);
                    break;
            }

            enemies.Remove(_enemyController);
            StartCoroutine(TankExplosion(pos));

            if (playerTransform != null)
            {
                EventService.Instance.InvokeEnemyDestroy(++enemiesDestroyedCount);
                StartCoroutine(SpawnEnemyTank());
            }
        }

        public IEnumerator TankExplosion(Vector3 tankPos)
        {
            ParticleSystem newTankExplosion = tankExplosionPoolService.GetExplosion(tankExplosion);
            newTankExplosion.transform.position = tankPos;
            newTankExplosion.gameObject.SetActive(true);
            newTankExplosion.Play();

            yield return new WaitForSeconds(2f);

            if (newTankExplosion != null)
            {
                newTankExplosion.gameObject.SetActive(false);
                tankExplosionPoolService.ReturnItem(newTankExplosion);
            }
        }

        public IEnumerator DestroyAllEnemies()
        {
            List<EnemyController> enemyList = new List<EnemyController>(enemies);

            yield return new WaitForSeconds(2f);

            foreach (EnemyController enemy in enemyList)
            {
                DestoryEnemy(enemy, enemy.GetEnemyType());
                yield return new WaitForSeconds(1f);
            }

            enemies.Clear();
        }
    }
}
