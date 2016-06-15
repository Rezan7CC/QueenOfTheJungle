using System;
using System.Collections.Generic;
using Apex.AI;
using Apex.AI.Components;

namespace QJAI
{
    public class AIContext : IAIContext
    {
        /// <summary>
        /// Constructor that initializes the context provider
        /// </summary>
        /// <param name="contextProvider">Context provider that owns this context object</param>
        public AIContext(IContextProvider contextProvider)
        {
            ContextProvider = contextProvider;
        }

        /// <summary>
        /// Gets the context provider owning this context object
        /// </summary>
        public IContextProvider ContextProvider
        {
            get;
            private set;
        }
    }
}
