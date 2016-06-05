using UnityEngine;
using System.Collections;
using QJHealth;

namespace QJProjectiles
{
    [RequireComponent(typeof(Health), typeof(ProjectileDamage))]
    public class ProjectileMovement : MonoBehaviour
    {
        /// <summary>
        /// Current projectile speed
        /// </summary>
        float projectileSpeed = 0;

        /// <summary>
        /// Cached Health component
        /// </summary>
        Health health;

        /// <summary>
        /// Cached ProjectileDamage component
        /// </summary>
        ProjectileDamage projectileDamage;

        /// <summary>
        /// Current projectile speed
        /// </summary>
        public float ProjectileSpeed
        {
            get { return projectileSpeed; }
            set { projectileSpeed = value; }
        }

        void Awake()
        {
            health = GetComponent<Health>();
            if (health == null)
                Debug.LogError("Health component not found!");

            projectileDamage = GetComponent<ProjectileDamage>();
            if(projectileDamage == null)
                Debug.LogError("ProjectileDamage component not found!");
        }

        void Update()
        {
            transform.position += transform.up * projectileSpeed * Time.deltaTime;
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            Health collidedHealth = collider.GetComponent<Health>();
            if (collidedHealth != null)
                projectileDamage.DealDamage(collidedHealth);

            health.Kill();
        }
    }
}