using System.Collections;
using UnityEngine;

namespace Skills
{
    public class SpawnProjectiles : MonoBehaviour
    {
        private float timeToFire = 0;
        private GameObject firePoint;

        void Start()
        {
            foreach (Transform child in transform)
            {
                if (child.CompareTag("FirePoint"))
                {
                    firePoint = child.gameObject;
                }
            }
        }

        public void SpawnProjectile(Vector3 targetPosition )
        {
            StartCoroutine(DelayToStartSkill(targetPosition));
        }

        private IEnumerator DelayToStartSkill(Vector3 targetPosition)
        {
            GameObject vfx;
            GameObject skill = null;

            foreach (Transform child in transform)
            {
                if (child.CompareTag("ProjectileSkill"))
                {
                    skill = child.gameObject;
                }
            }

            yield return new WaitForSeconds(1f);

            if (skill is null)
            {
                Debug.Log("SKILL NOT FOUND!");
                yield break;
            }

            vfx = Instantiate (skill, firePoint.transform.position, Quaternion.identity);
            vfx.transform.SetParent(transform);
            vfx.SetActive(true);

            vfx.GetComponent<ProjectileMove>().MoveProjectileToTarget(targetPosition);
        }
    }
}
