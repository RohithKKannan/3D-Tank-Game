using UnityEngine;
using BattleTank.ScriptableObjects;
using BattleTank.PlayerCamera;
using BattleTank.Events;

namespace BattleTank.PlayerTank
{
    public class TankController
    {
        public TankModel tankModel { get; }
        public TankView tankView { get; }

        private int health;
        private Rigidbody rb;

        private Vector3 tankDirection;
        private Vector3 oldPosition;
        private float totalDistanceTravelled;
        private float currentDistance;

        public TankController(TankScriptableObject tank, FixedJoystick _joystick, CameraController cameraController)
        {
            tankView = GameObject.Instantiate<TankView>(tank.tankView);
            cameraController.SetTankTransform(tankView.transform);
            tankModel = new TankModel(tank);

            tankView.SetTankController(this);
            tankModel.SetTankController(this);
            tankView.SetJoystick(_joystick);

            rb = tankView.GetRigidbody();
            health = tankModel.health;
            EventService.Instance.InvokeSetMaxHealthBar(health);
            EventService.Instance.InvokeSetPlayerHealthBar(health);
            oldPosition = rb.transform.position;
        }

        public void MoveTank(float _horizontalMove, float _verticalMove)
        {
            tankDirection = Vector3.forward * _verticalMove + Vector3.right * _horizontalMove;
            tankDirection = Quaternion.Euler(0, 60, 0) * tankDirection;

            rb.velocity = tankDirection * tankModel.speed;
            tankView.transform.LookAt(tankDirection.normalized + tankView.transform.position);

            CalculateDistance();
        }

        private void CalculateDistance()
        {
            currentDistance = (rb.transform.position - oldPosition).magnitude;

            totalDistanceTravelled += currentDistance;
            oldPosition = rb.transform.position;

            TankService.Instance.distanceTravelled(totalDistanceTravelled);
        }

        public void Shoot(Transform gunTransform)
        {
            TankService.Instance.ShootBullet(tankModel.bulletType, gunTransform);
        }

        public void TakeDamage(int damage)
        {
            health -= damage;
            EventService.Instance.InvokeSetPlayerHealthBar(health);
            if (health <= 0)
                TankDeath();
        }

        private void TankDeath()
        {
            TankService.Instance.DestoryTank(tankView);
        }

        public Transform GetTransform()
        {
            return rb.transform;
        }
    }
}
