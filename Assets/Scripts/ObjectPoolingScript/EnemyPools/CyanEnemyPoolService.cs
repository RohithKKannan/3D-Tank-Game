using BattleTank.Generics;
using BattleTank.ScriptableObjects;
using BattleTank.Enemy;

namespace BattleTank.ObjectPool
{
    public class CyanEnemyPoolService : GenericObjectPool<EnemyController>
    {
        EnemyScriptableObject enemyData;
        EnemyType enemyType;

        public EnemyController GetEnemy(EnemyScriptableObject _enemyData, EnemyType _enemyType)
        {
            enemyData = _enemyData;
            enemyType = _enemyType;
            return GetItem();
        }

        protected override EnemyController CreateItem()
        {
            EnemyController enemyController = new EnemyController(enemyData, enemyType);
            return enemyController;
        }
    }
}
