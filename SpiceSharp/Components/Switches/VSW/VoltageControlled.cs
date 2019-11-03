﻿using SpiceSharp.Entities;
using SpiceSharp.Simulations;

namespace SpiceSharp.Components.SwitchBehaviors
{
    /// <summary>
    /// Controller for making a switch voltage-controlled.
    /// </summary>
    /// <seealso cref="SpiceSharp.Components.SwitchBehaviors.Controller" />
    public class VoltageControlled : Controller
    {
        /// <summary>
        /// Gets the controlling positive node index.
        /// </summary>
        protected int ContPosNode { get; private set; }

        /// <summary>
        /// Gets the controlling negative node index.
        /// </summary>
        protected int ContNegNode { get; private set; }

        /// <summary>
        /// Bind the behavior. for the specified simulation.
        /// </summary>
        /// <param name="context">The context.</param>
        public override void Bind(BindingContext context)
        {
            var c = (ComponentBindingContext)context;
            var state = context.States.GetValue<IBiasingSimulationState>();
            c.Nodes.ThrowIfNot("nodes", 4);
            ContPosNode = state.Map[c.Nodes[2]];
            ContNegNode = state.Map[c.Nodes[3]];
        }

        /// <summary>
        /// Gets the value that is controlling the switch.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns></returns>
        public override double GetValue(IBiasingSimulationState state) =>
            state.Solution[ContPosNode] - state.Solution[ContNegNode];
    }
}
