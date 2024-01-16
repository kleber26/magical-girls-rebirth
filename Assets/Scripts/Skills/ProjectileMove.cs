using System;
using System.Collections;
using UnityEngine;

namespace Skills
{
    public class ProjectileMove : MonoBehaviour
    {
        public float speed;
        public float fireRate;
        public Transform target;

        public void MoveProjectileToTarget(Vector3 targetPosition)
        {

            // Vector3 targetPosition = target.position;
            Vector3 direction = (targetPosition - transform.position).normalized;
            StartCoroutine(MoveProjectile(direction, targetPosition));
        }

        private IEnumerator MoveProjectile(Vector3 targetDirection, Vector3 targetPosition)
        {
            while (transform.position != targetPosition)
            {
                transform.position += targetDirection * (speed * Time.deltaTime);

                float distance = Vector3.Distance (transform.position, targetPosition);
                if (distance < 0.4f)
                {
                    Destroy(gameObject);
                }
                yield return null;
            }

        }
    }
}
