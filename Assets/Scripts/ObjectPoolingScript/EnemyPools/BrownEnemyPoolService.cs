using UnityEngine;
using BattleTank.Generics;
using BattleTank.ScriptableObjects;
using BattleTank.Enemy;

namespace BattleTank.ObjectPool
{
    public class BrownEnemyPoolService : GenericObjectPool<EnemyController>
    {
        private EnemyScriptableObject enemyData;
        private EnemyType enemyType;
        private Camera playerCamera;
        private Canvas enemyCanvas;

        public EnemyController GetEnemy(EnemyScriptableObject _enemyData, EnemyType _enemyType, Camera _playerCamera, Canvas _enemyCanvas)
        {
            enemyData = _enemyData;
            enemyType = _enemyType;
            playerCamera = _playerCamera;
            enemyCanvas = _enemyCanvas;
            return GetItem();
        }

        protected override EnemyController CreateItem()
        {
            EnemyController enemyController = new EnemyController(enemyData, enemyType, playerCamera, enemyCanvas);
            return enemyController;
        }
    }
}
