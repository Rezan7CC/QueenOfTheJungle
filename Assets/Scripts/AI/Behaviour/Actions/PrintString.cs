using UnityEngine;
using System.Collections;
using Apex.AI;
using Apex.AI.Components;
using Apex.Serialization;

namespace QJAI.QJBehaviour
{
    public class PrintString : IAction
    {
        /// <summary>
        /// String that will be printed to the console
        /// </summary>
        [ApexSerialization, FriendlyName("Output String", "String that will be printed to the console")]
        string outputString = "Debug";

        /// <summary>
        /// Event that gets called if action is active
        /// </summary>
        /// <param name="aiContext">AI context object</param>
        public void Execute(IAIContext aiContext)
        {
            Debug.Log("AI: " + outputString);
        }
    }
}
