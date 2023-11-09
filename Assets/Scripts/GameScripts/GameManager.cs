using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleTank.PlayerCamera;
using BattleTank.PlayerTank;
using BattleTank.Enemy;
using BattleTank.Level;

namespace BattleTank.Game
{
    public class GameManager : MonoBehaviour
    {
        [Header("Level Service")]
        [SerializeField] private GameObject[] entities;

        [Header("Tank Service")]
        [SerializeField] private FixedJoystick joystick;
        [SerializeField] private CameraController mainCamera;

        [Header("Enemy Service")]
        [SerializeField] private Camera playerCamera;
        [SerializeField] private Canvas enemyUICanvas;
        [SerializeField] private Transform spawnPointParent;


        private void Start()
        {
            LevelService.Instance.StartLevelService(entities, mainCamera);
            TankService.Instance.StartTankService(joystick, mainCamera);
            EnemyService.Instance.StartEnemyService(playerCamera, enemyUICanvas, spawnPointParent);
        }
    }
}
