using UnityEngine;
using System.Collections;

namespace QJCombat
{
    public class ShootProjectiles : MonoBehaviour
    {
        /// <summary>
        /// Projectile to shoot
        /// </summary>
        [SerializeField]
        GameObject projectile = null;

        /// <summary>
        /// Speed of the projectile
        /// </summary>
        [SerializeField]
        float projectileSpeed = 10;

        /// <summary>
        /// Position offset from transform position for projectile spawn
        /// </summary>
        [SerializeField]
        Vector2 shootPositionOffset = Vector2.zero;

        /// <summary>
        /// Projectile to shoot
        /// </summary>
        public GameObject Projectile
        {
            get { return projectile; }
            private set { projectile = value; }
        }

        /// <summary>
        /// Speed of the projectile
        /// </summary>
        public float ProjectileSpeed
        {
            get { return projectileSpeed; }
            set { projectileSpeed = value; }
        }

        /// <summary>
        /// Position offset from transform position for projectile spawn
        /// </summary>
        public Vector2 ShootPositionOffset
        {
            get { return shootPositionOffset; }
            set { shootPositionOffset = value; }
        }

        /// <summary>
        /// Shoot a projectile
        /// </summary>
        public void ShootProjectile()
        {
            GameObject spawnedProjectile = Instantiate<GameObject>(projectile);
            spawnedProjectile.transform.position = transform.position + new Vector3(shootPositionOffset.x, shootPositionOffset.y, 0);
            spawnedProjectile.transform.up = transform.right;

            ProjectileMovement projectileMovement = spawnedProjectile.GetComponent<ProjectileMovement>();
            if(projectileMovement != null)
                projectileMovement.ProjectileSpeed = projectileSpeed;
        }
    }
}
