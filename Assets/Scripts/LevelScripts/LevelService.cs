using System.Collections;
using UnityEngine;
using BattleTank.Generics;
using BattleTank.ObjectPool;
using BattleTank.Enemy;
using BattleTank.PlayerCamera;

namespace BattleTank.Level
{
    public class LevelService : GenericSingleton<LevelService>
    {
        private TankExplosionPoolService explosionPoolService;

        [SerializeField] private GameObject[] entities;
        [SerializeField] private ParticleSystem explosionEffect;
        [SerializeField] private CameraController cameraController;

        private void Start()
        {
            explosionPoolService = new TankExplosionPoolService();
        }

        public IEnumerator DestroyLevel()
        {
            StartCoroutine(cameraController.ZoomOut());

            yield return StartCoroutine(EnemyService.Instance.DestroyAllEnemies());
            yield return StartCoroutine(DestroyEnvironment());
        }

        public IEnumerator DestroyEnvironment()
        {
            foreach (GameObject item in entities)
            {
                yield return StartCoroutine(DestroyEntity(item));
            }
        }

        IEnumerator DestroyEntity(GameObject entity)
        {
            foreach (Transform item in entity.GetComponentsInChildren<Transform>())
            {
                if (item == entity.transform)
                    continue;

                if (item.childCount != 0)
                    continue;

                Vector3 pos = item.position;

                Destroy(item.gameObject);
                StartCoroutine(ExplosionEffect(pos));

                yield return new WaitForSeconds(1f);
            }
        }

        public IEnumerator ExplosionEffect(Vector3 pos)
        {
            ParticleSystem newExplosionEffect = explosionPoolService.GetExplosion(explosionEffect);

            newExplosionEffect.transform.position = pos;
            newExplosionEffect.gameObject.SetActive(true);
            newExplosionEffect.Play();

            yield return new WaitForSeconds(1.5f);

            newExplosionEffect.gameObject.SetActive(false);
            explosionPoolService.ReturnItem(newExplosionEffect);
        }
    }
}
