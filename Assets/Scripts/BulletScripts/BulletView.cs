using UnityEngine;

namespace BattleTank.Bullet
{
    public class BulletView : MonoBehaviour
    {
        private BulletController bulletController;

        [SerializeField] private Rigidbody rb;

        public void SetBulletController(BulletController _bulletController)
        {
            bulletController = _bulletController;
        }

        public TankType GetTankType()
        {
            return bulletController.GetTankType();
        }

        public Rigidbody GetRigidbody()
        {
            return rb;
        }

        private void OnCollisionEnter(Collision col)
        {
            bulletController.BulletCollision(col.contacts[0].point);

            IDamageable target = col.gameObject.GetComponent<IDamageable>();
            if (target != null)
                target.TakeDamage(bulletController.GetBulletDamage(), bulletController.GetTankType());
        }
    }
}
