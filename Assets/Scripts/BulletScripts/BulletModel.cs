using BattleTank.ScriptableObjects;

namespace BattleTank.Bullet
{
    public class BulletModel
    {
        public int damage { get; }
        public int range { get; }
        public TankType tankType { private set; get; }
        public BulletType bulletType { private set; get; }

        private BulletController bulletController;

        public BulletModel(BulletScriptableObject _bullet, BulletType _bulletType)
        {
            damage = _bullet.damage;
            range = _bullet.range;
            bulletType = _bulletType;
        }

        public void SetTankType(TankType _tankType)
        {
            tankType = _tankType;
        }

        public void SetBulletController(BulletController _bulletController)
        {
            bulletController = _bulletController;
        }
    }
}
