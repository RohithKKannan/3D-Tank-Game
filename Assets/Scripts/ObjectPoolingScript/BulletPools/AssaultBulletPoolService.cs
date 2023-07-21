using BattleTank.Generics;
using BattleTank.Bullet;
using BattleTank.ScriptableObjects;

namespace BattleTank.ObjectPool
{
    public class AssaultBulletPoolService : GenericObjectPool<BulletController>
    {
        BulletScriptableObject bulletData;
        BulletType bulletType = BulletType.Assault;

        public BulletController GetBullet(BulletScriptableObject bulletData)
        {
            this.bulletData = bulletData;
            return GetItem();
        }

        protected override BulletController CreateItem()
        {
            BulletController bulletController = new BulletController(bulletData, bulletType);
            return bulletController;
        }
    }
}
