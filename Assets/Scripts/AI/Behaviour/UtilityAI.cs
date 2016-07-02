using UnityEngine;
using System.Collections;
using Apex.AI;
using Apex.AI.Components;

namespace QJAI.QJBehaviour
{
    [ExecuteInEditMode]
    public class UtilityAI : MonoBehaviour, IContextProvider
    {
        /// <summary>
        /// AI context object for the Utility AI system
        /// </summary>
        private IAIContext aiContext = null;

        /// <summary>
        /// Utility AI component that gets created by this component
        /// </summary>
        private UtilityAIComponent utilityAIComp = null;

        void Awake()
        {
            // Create new context object if it isn't valid
            if(aiContext == null)
                aiContext = new AIContext(this);

            // Get or create Utility AI component if it isn't valid
            if (utilityAIComp == null)
            {
                utilityAIComp = GetComponent<UtilityAIComponent>();
                if(utilityAIComp == null)
                    utilityAIComp = gameObject.AddComponent<UtilityAIComponent>();
                if (utilityAIComp == null)
                    Debug.LogError("Utility AI component creation failed!");
            }
        }

        /// <summary>
        /// Get context method for Utility AI system
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IAIContext GetContext(System.Guid id)
        {
            return aiContext;
        }
    }
}
