using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattleTank.Generics;
using BattleTank.ScriptableObjects;
using BattleTank.ObjectPool;
using BattleTank.Events;
using BattleTank.Level;
using BattleTank.PlayerCamera;
using BattleTank.Bullet;

public enum TankType
{
    Player, Enemy
}

namespace BattleTank.PlayerTank
{
    public class TankService : GenericSingleton<TankService>
    {
        private int bulletCount;
        private List<int> distanceCheckpointsAchieved = new();
        private TankController tankController;
        private TankExplosionPoolService tankExplosionPoolService;

        [SerializeField] private TankScriptableObjectList playerTankList;
        [SerializeField] private FixedJoystick joystick;
        [SerializeField] private CameraController mainCamera;
        [SerializeField] private ParticleSystem tankExplosion;

        [Header("Distance achievement")]
        [SerializeField] private int[] distanceCheckpoints;

        private void Start()
        {
            bulletCount = 0;
            tankExplosionPoolService = new TankExplosionPoolService();
            CreatePlayerTank(UnityEngine.Random.Range(0, playerTankList.tanks.Length));
        }

        public void CreatePlayerTank(int index)
        {
            TankScriptableObject tank = playerTankList.tanks[index];
            tankController = new TankController(tank, joystick, mainCamera);
        }

        public void ShootBullet(BulletType bulletType, Transform gunTransform)
        {
            bulletCount++;
            EventService.Instance.InvokePlayerFiredBullet(bulletCount);
            BulletService.Instance.SpawnBullet(bulletType, gunTransform, TankType.Player);
        }

        public void DestoryTank(TankView tankView)
        {
            Vector3 pos = tankView.transform.position;

            mainCamera.SetTankTransform(null);
            Destroy(tankView.gameObject);

            StartCoroutine(TankExplosion(pos));
            StartCoroutine(LevelService.Instance.DestroyLevel());
        }

        public IEnumerator TankExplosion(Vector3 tankPos)
        {
            ParticleSystem newTankExplosion = tankExplosionPoolService.GetExplosion(tankExplosion);
            newTankExplosion.transform.position = tankPos;
            newTankExplosion.gameObject.SetActive(true);
            newTankExplosion.Play();

            yield return new WaitForSeconds(2f);

            newTankExplosion.gameObject.SetActive(false);
            tankExplosionPoolService.ReturnItem(newTankExplosion);
        }

        public Transform GetPlayerTransform()
        {
            return tankController.GetTransform();
        }

        public void distanceTravelled(float distance)
        {
            for (int i = distanceCheckpoints.Length - 1; i >= 0; i--)
            {
                if (distance > distanceCheckpoints[i] && !distanceCheckpointsAchieved.Contains(distanceCheckpoints[i]))
                {
                    EventService.Instance.InvokeDistanceTravelled(distanceCheckpoints[i]);
                    distanceCheckpointsAchieved.Add(distanceCheckpoints[i]);
                    return;
                }
            }
        }
    }
}
