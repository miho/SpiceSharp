﻿using SpiceSharp.Attributes;
using SpiceSharp.Behaviors;
using SpiceSharp.Entities;
using SpiceSharp.IntegrationMethods;
using SpiceSharp.Simulations;

namespace SpiceSharp.Components.DiodeBehaviors
{
    /// <summary>
    /// Transient behavior for a <see cref="Diode"/>
    /// </summary>
    public class TransientBehavior : DynamicParameterBehavior, ITimeBehavior
    {
        /// <summary>
        /// Gets the capacitance charge.
        /// </summary>
        [ParameterName("charge"), ParameterInfo("Diode capacitor charge")]
        public sealed override double CapCharge
        {
            get => _capCharge.Current;
            protected set => _capCharge.Current = value;
        }

        /// <summary>
        /// Gets the capacitor current.
        /// </summary>
        [ParameterName("capcur"), ParameterInfo("Diode capacitor current")]
        public double CapCurrent => _capCharge.Derivative;

        /// <summary>
        /// The charge on the junction capacitance
        /// </summary>
        private StateDerivative _capCharge;
        private int _negNode, _posPrimeNode;

        /// <summary>
        /// Creates a new instance of the <see cref="TransientBehavior"/> class.
        /// </summary>
        /// <param name="name">Name</param>
        public TransientBehavior(string name) : base(name) { }

        /// <summary>
        /// Bind the behavior to a simulation.
        /// </summary>
        /// <param name="context">The binding context.</param>
        public override void Bind(BindingContext context)
        {
            base.Bind(context);

            var c = (ComponentBindingContext)context;
            _negNode = BiasingState.Map[c.Nodes[1]];
            _posPrimeNode = BiasingState.Map[PosPrime];

            var method = context.States.GetValue<ITimeSimulationState>().Method;
            _capCharge = method.CreateDerivative();
        }

        /// <summary>
        /// Calculate the state values
        /// </summary>
        void ITimeBehavior.InitializeStates()
        {
            double vd = BiasingState.Solution[_posPrimeNode] - BiasingState.Solution[_negNode];
            CalculateCapacitance(vd);
        }

        /// <summary>
        /// Transient behavior
        /// </summary>
        void ITimeBehavior.Load()
        {
            // Calculate the capacitance
            var state = BiasingState;
            double vd = state.Solution[_posPrimeNode] - state.Solution[_negNode];
            CalculateCapacitance(vd);

            // Integrate
            _capCharge.Integrate();
            var geq = _capCharge.Jacobian(Capacitance);
            var ceq = _capCharge.RhsCurrent(geq, vd);

            // Store the current
            Current += _capCharge.Derivative;

            Elements.Add(
                // Y-matrix
                0, geq, geq, -geq, -geq, 0, 0,
                // RHS vector
                ceq, -ceq);
        }
    }
}
