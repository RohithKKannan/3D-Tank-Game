using UnityEngine;
using BattleTank.Generics;

namespace BattleTank.ObjectPool
{
    public class TankExplosionPoolService : GenericObjectPool<ParticleSystem>
    {
        [SerializeField] private ParticleSystem prefab;

        public ParticleSystem GetExplosion(ParticleSystem explosionEffect)
        {
            prefab = explosionEffect;
            return GetItem();
        }

        protected override ParticleSystem CreateItem()
        {
            ParticleSystem newExplosion = GameObject.Instantiate<ParticleSystem>(prefab, Vector3.zero, Quaternion.identity);
            return newExplosion;
        }
    }
}
