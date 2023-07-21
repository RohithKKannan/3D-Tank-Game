using BattleTank.Generics;
using BattleTank.Bullet;
using BattleTank.ScriptableObjects;

namespace BattleTank.ObjectPool
{
    public class PistolBulletPoolService : GenericObjectPool<BulletController>
    {
        BulletScriptableObject bulletData;
        BulletType bulletType = BulletType.Pistol;

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
