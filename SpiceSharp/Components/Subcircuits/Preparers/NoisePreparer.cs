﻿using SpiceSharp.Simulations;

namespace SpiceSharp.Components.SubcircuitBehaviors
{
    /// <summary>
    /// Noise preparer for a <see cref="SubcircuitSimulation"/>
    /// </summary>
    /// <seealso cref="ISimulationPreparer" />
    public class NoisePreparer : ISimulationPreparer
    {
        /// <summary>
        /// Prepares the task's simulation for the behavior.
        /// </summary>
        /// <param name="simulations">The task simulations to be prepared.</param>
        /// <param name="parent">The parent simulation.</param>
        /// <param name="parameters">The parameters.</param>
        public void Prepare(SubcircuitSimulation[] simulations, ISimulation parent, ParameterSetDictionary parameters)
        {
            foreach (var sim in simulations)
            {
                var state = parent.States.GetValue<INoiseSimulationState>();
                sim.States.Add(state);
            }
        }
    }
}
