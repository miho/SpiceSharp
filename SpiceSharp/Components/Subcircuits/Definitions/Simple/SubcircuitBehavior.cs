﻿using SpiceSharp.Behaviors;

namespace SpiceSharp.Components.SubcircuitBehaviors.Simple
{
    /// <summary>
    /// A template for a subcircuit behavior.
    /// </summary>
    /// <typeparam name="B">The behavior type.</typeparam>
    /// <seealso cref="Behavior" />
    public abstract class SubcircuitBehavior<B> : Behavior where B : IBehavior
    {
        /// <summary>
        /// Gets the simulation.
        /// </summary>
        /// <value>
        /// The simulation.
        /// </value>
        protected SubcircuitSimulation Simulation { get; }

        /// <summary>
        /// Gets the behaviors.
        /// </summary>
        /// <value>
        /// The behaviors.
        /// </value>
        protected BehaviorList<B> Behaviors { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubcircuitBehavior{B}"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="simulation">The simulation.</param>
        protected SubcircuitBehavior(string name, SubcircuitSimulation simulation)
            : base(name)
        {
            Simulation = simulation.ThrowIfNull(nameof(simulation));
            Behaviors = Simulation.EntityBehaviors.GetBehaviorList<B>();
        }
    }
}