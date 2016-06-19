using UnityEngine;
using System.Collections;

namespace QJCombat
{
    public class ProjectileDamage : MonoBehaviour
    {
        /// <summary>
        /// Damage of the projectile
        /// </summary>
        [SerializeField]
        float damageAmount = 5;
        
        /// <summary>
        /// Damage of the projectile
        /// </summary>
        public float DamageAmount
        {
            get {return damageAmount;}
            set {damageAmount = value;}
        }

        /// <summary>
        /// Deal damage to a Health component
        /// </summary>
        /// <param name="damageTarget">Target health component</param>
        public void DealDamage(Health damageTarget)
        {
            damageTarget.ApplyDamage(damageAmount);
        }

        /// <summary>
        /// Deal damage to a GameObject
        /// </summary>
        /// <param name="damageTarget">Target GameObject</param>
        public void DealDamage(GameObject damageTarget)
        {
            Health healh = damageTarget.GetComponent<Health>();
            if (healh != null)
                DealDamage(healh);
        }
    }
}