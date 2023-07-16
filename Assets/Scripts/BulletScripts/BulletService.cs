using UnityEngine;
using BattleTank.Generics;
using BattleTank.ScriptableObjects;
using BattleTank.ObjectPool;

namespace BattleTank.Bullet
{
    public enum BulletType
    {
        Sniper, Assault, Pistol
    }

    public class BulletService : GenericSingleton<BulletService>
    {
        SniperBulletPoolService sniperBulletPoolService;
        AssaultBulletPoolService assaultBulletPoolService;
        PistolBulletPoolService pistolBulletPoolService;

        [SerializeField] private BulletScriptableObjectList bulletList;
        [SerializeField] private ParticleSystem bulletExplosion;

        void Start()
        {
            sniperBulletPoolService = new SniperBulletPoolService();
            assaultBulletPoolService = new AssaultBulletPoolService();
            pistolBulletPoolService = new PistolBulletPoolService();
        }

        public void SpawnBullet(BulletType bulletType, Transform _transform, TankType tankType)
        {
            switch (bulletType)
            {
                case BulletType.Sniper:
                    BulletController sniperBulletController = sniperBulletPoolService.GetBullet(bulletList.bullets[(int)bulletType]);

                    sniperBulletController.EnableBullet(_transform, tankType);
                    break;

                case BulletType.Assault:
                    BulletController assaultBulletController = assaultBulletPoolService.GetBullet(bulletList.bullets[(int)bulletType]);

                    assaultBulletController.EnableBullet(_transform, tankType);
                    break;

                case BulletType.Pistol:
                    BulletController pistolBulletController = pistolBulletPoolService.GetBullet(bulletList.bullets[(int)bulletType]);

                    pistolBulletController.EnableBullet(_transform, tankType);
                    break;

                default: break;
            }
        }

        public void BulletExplosion(BulletController bulletController, Vector3 position, BulletView bulletView, BulletType bulletType)
        {
            ParticleSystem explosion = GameObject.Instantiate<ParticleSystem>(bulletExplosion, position, Quaternion.identity);
            explosion.Play();

            bulletController.DisableBullet();
            switch (bulletType)
            {
                case BulletType.Sniper:
                    sniperBulletPoolService.ReturnItem(bulletController);
                    break;
                case BulletType.Assault:
                    assaultBulletPoolService.ReturnItem(bulletController);
                    break;
                case BulletType.Pistol:
                    pistolBulletPoolService.ReturnItem(bulletController);
                    break;
                default: break;
            }
        }
    }
}
