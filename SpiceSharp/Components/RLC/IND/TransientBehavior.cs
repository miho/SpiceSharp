﻿using System;
using SpiceSharp.Entities;
using SpiceSharp.Attributes;
using SpiceSharp.Behaviors;
using SpiceSharp.IntegrationMethods;
using SpiceSharp.Simulations;
using SpiceSharp.Algebra;

namespace SpiceSharp.Components.InductorBehaviors
{
    /// <summary>
    /// Transient behavior for an <see cref="Inductor" />.
    /// </summary>
    public class TransientBehavior : BiasingBehavior, ITimeBehavior
    {
        /// <summary>
        /// An event called when the flux can be updated
        /// Can be used by mutual inductances
        /// </summary>
        public event EventHandler<UpdateFluxEventArgs> UpdateFlux;

        /// <summary>
        /// Gets the transient matrix elements.
        /// </summary>
        /// <value>
        /// The transient matrix elements.
        /// </value>
        protected ElementSet<double> TransientElements { get; private set; }

        /// <summary>
        /// The state tracking the flux.
        /// </summary>
        private StateDerivative _flux;

        /// <summary>
        /// Gets the flux of the inductor.
        /// </summary>
        [ParameterName("flux"), ParameterInfo("The flux through the inductor.")]
        public double Flux => _flux.Current;

        private int _posNode, _negNode, _branchEq;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransientBehavior"/> class.
        /// </summary>
        /// <param name="name">Name</param>
        public TransientBehavior(string name) : base(name)
        {
        }

        /// <summary>
        /// Bind the behavior to a simulation.
        /// </summary>
        /// <param name="context">The binding context.</param>
        public override void Bind(BindingContext context)
        {
            base.Bind(context);

            var c = (ComponentBindingContext)context;
            _posNode = BiasingState.Map[c.Nodes[0]];
            _negNode = BiasingState.Map[c.Nodes[1]];
            _branchEq = BiasingState.Map[Branch];
            TransientElements = new ElementSet<double>(BiasingState.Solver, new[] {
                new MatrixLocation(_branchEq, _branchEq)
            }, new[] { _branchEq });

            var method = context.States.GetValue<ITimeSimulationState>().Method;
            _flux = method.CreateDerivative();
        }

        /// <summary>
        /// Unbind the behavior.
        /// </summary>
        public override void Unbind()
        {
            base.Unbind();

            // Clear all events
            if (UpdateFlux != null)
            {
                foreach (var inv in UpdateFlux.GetInvocationList())
                    UpdateFlux -= (EventHandler<UpdateFluxEventArgs>)inv;
            }

            TransientElements?.Destroy();
            TransientElements = null;
            _flux = null;
        }

        /// <summary>
        /// Calculate DC states
        /// </summary>
        void ITimeBehavior.InitializeStates()
        {
            // Get the current through
            if (BaseParameters.InitialCondition.Given)
                _flux.Current = BaseParameters.InitialCondition * BaseParameters.Inductance;
            else
                _flux.Current = BiasingState.Solution[_branchEq] * BaseParameters.Inductance;
        }

        /// <summary>
        /// Execute behaviour
        /// </summary>
        void ITimeBehavior.Load()
        {
            // Initialize
            _flux.ThrowIfNotBound(this).Current = BaseParameters.Inductance * BiasingState.Solution[_branchEq];
            
            // Allow alterations of the flux
            if (UpdateFlux != null)
            {
                var args = new UpdateFluxEventArgs(BaseParameters.Inductance, BiasingState.Solution[_branchEq], _flux, BiasingState);
                UpdateFlux.Invoke(this, args);
            }

            // Finally load the Y-matrix
            _flux.Integrate();
            TransientElements.Add(
                -_flux.Jacobian(BaseParameters.Inductance),
                _flux.RhsCurrent()
                );
        }
    }
}
