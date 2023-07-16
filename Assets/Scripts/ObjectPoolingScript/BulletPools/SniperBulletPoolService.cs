using BattleTank.Generics;
using BattleTank.Bullet;
using BattleTank.ScriptableObjects;

namespace BattleTank.ObjectPool
{
    public class SniperBulletPoolService : GenericObjectPool<BulletController>
    {
        BulletScriptableObject bulletData;
        BulletType bulletType = BulletType.Sniper;

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
