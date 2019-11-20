﻿using SpiceSharp.Behaviors;
using SpiceSharp.Entities;
using SpiceSharp.Components.NonlinearResistorBehaviors;
using SpiceSharp.Simulations;

namespace SpiceSharp.Components
{
    /// <summary>
    /// A nonlinear resistor
    /// </summary>
    /// <seealso cref="Component" />
    public class NonlinearResistor : Component
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NonlinearResistor"/> class.
        /// </summary>
        /// <param name="name">The string of the entity.</param>
        /// <param name="nodeA">Node A</param>
        /// <param name="nodeB">Node B</param>
        public NonlinearResistor(string name, string nodeA, string nodeB) : base(name, 2)
        {
            // Add a NonlinearResistorBehaviors.BaseParameters
            Parameters.Add(new BaseParameters());

            // Connect the entity
            Connect(nodeA, nodeB);
        }

        /// <summary>
        /// Creates the behaviors for the specified simulation and registers them with the simulation.
        /// </summary>
        /// <param name="simulation">The simulation.</param>
        /// <param name="behaviors">An <see cref="T:SpiceSharp.Behaviors.IBehaviorContainer" /> where the behaviors can be stored.</param>
        public override void CreateBehaviors(ISimulation simulation, IBehaviorContainer behaviors)
        {
            base.CreateBehaviors(simulation, behaviors);

            var context = new ComponentBindingContext(simulation, behaviors, MapNodes(simulation.Variables), Model);
            if (simulation.EntityBehaviors.Tracks<IBiasingBehavior>())
                behaviors.Add(new BiasingBehavior(Name, context));
        }
    }
}
