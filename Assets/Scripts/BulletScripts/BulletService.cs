using UnityEngine;
using BattleTank.Generics;
using BattleTank.ScriptableObjects;
using BattleTank.ObjectPool;
using System.Collections;

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
        BulletExplosionPoolService bulletExplosionPoolService;

        [SerializeField] private BulletScriptableObjectList bulletList;
        [SerializeField] private ParticleSystem bulletExplosion;

        void Start()
        {
            sniperBulletPoolService = new SniperBulletPoolService();
            assaultBulletPoolService = new AssaultBulletPoolService();
            pistolBulletPoolService = new PistolBulletPoolService();
            bulletExplosionPoolService = new BulletExplosionPoolService();
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
            StartCoroutine(PlayExplosionEffect(position));

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

        private IEnumerator PlayExplosionEffect(Vector3 position)
        {
            ParticleSystem explosion = bulletExplosionPoolService.GetExplosion(bulletExplosion);

            explosion.transform.position = position;
            explosion.gameObject.SetActive(true);
            explosion.Play();

            yield return new WaitForSeconds(2f);
            EndExplosionEffect(explosion);
        }

        private void EndExplosionEffect(ParticleSystem explosion)
        {
            explosion.gameObject.SetActive(false);

            bulletExplosionPoolService.ReturnItem(explosion);
        }
    }
}
