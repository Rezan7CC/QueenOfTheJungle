using UnityEngine;
using System.Collections;

namespace QJHealth
{
    public class Health : MonoBehaviour
    {
        /// <summary>
        /// The max health points
        /// </summary>
        [SerializeField]
        float maxHealthPoints = 10;

        /// <summary>
        /// The current health points
        /// </summary>
        float healthPoints;

        /// <summary>
        /// The max health points
        /// </summary>
        public float MaxHealthPoints
        {
            get { return maxHealthPoints; }
            private set { maxHealthPoints = value; }
        }

        /// <summary>
        /// The current health points
        /// </summary>
        public float HealthPoints
        {
            get { return healthPoints; }
            private set
            {
                if(healthPoints != value)
                {
                    healthPoints = value;
                    healthPoints = Mathf.Clamp(healthPoints, 0, maxHealthPoints);
                    if (Mathf.Approximately(healthPoints, 0))
                        Kill();
                }
            }
        }

        /// <summary>
        /// Apply damage to the owner
        /// </summary>
        /// <param name="damageAmount">The damage amount</param>
        public void ApplyDamage(float damageAmount)
        {
            HealthPoints -= damageAmount;
        }

        /// <summary>
        /// Kill the owner
        /// </summary>
        public void Kill()
        {
            Destroy(gameObject);
        }
    }
}
