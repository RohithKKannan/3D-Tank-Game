using UnityEngine;
using BattleTank.ScriptableObjects;

namespace BattleTank.Bullet
{
    public class BulletController
    {
        private BulletModel bulletModel;
        private BulletView bulletView;
        private Rigidbody rb;

        public BulletController(BulletScriptableObject _bullet, BulletType _bulletType)
        {
            bulletView = GameObject.Instantiate<BulletView>(_bullet.bulletView);
            bulletModel = new BulletModel(_bullet, _bulletType);

            bulletView.SetBulletController(this);
            bulletModel.SetBulletController(this);

            rb = bulletView.GetRigidbody();
        }

        public void SetBulletTankType(TankType tankType)
        {
            bulletModel.SetTankType(tankType);
        }

        public void Shoot()
        {
            rb.AddForce(rb.transform.forward * bulletModel.range, ForceMode.Impulse);
            Debug.Log(rb.velocity);
        }

        public void BulletCollision(Vector3 position)
        {
            rb.rotation = Quaternion.identity;
            BulletService.Instance.BulletExplosion(this, position, bulletView, bulletModel.bulletType);
        }

        public int GetBulletDamage()
        {
            return bulletModel.damage;
        }

        public TankType GetTankType()
        {
            return bulletModel.tankType;
        }

        public void EnableBullet(Transform gunTransform, TankType tankType)
        {
            SetBulletTankType(tankType);

            rb.transform.position = gunTransform.position;
            rb.transform.rotation = gunTransform.rotation;

            Debug.Log(rb.transform.position);

            rb.gameObject.SetActive(true);
            Shoot();
        }

        public void DisableBullet()
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.rotation = Quaternion.identity;
            rb.gameObject.SetActive(false);
        }
    }
}
