using UnityEngine;
using BattleTank.PlayerTank;
using BattleTank.Bullet;

namespace BattleTank.ScriptableObjects
{
    [CreateAssetMenu(fileName = "TankScriptableObject", menuName = "ScriptableObjects/NewTank")]
    public class TankScriptableObject : ScriptableObject
    {
        public int health;
        public int speed;

        public BulletType bulletType;
        public TankView tankView;
    }
}
