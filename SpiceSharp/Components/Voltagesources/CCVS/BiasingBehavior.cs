﻿using SpiceSharp.Entities;
using SpiceSharp.Attributes;
using SpiceSharp.Behaviors;
using SpiceSharp.Simulations;
using SpiceSharp.Algebra;

namespace SpiceSharp.Components.CurrentControlledVoltageSourceBehaviors
{
    /// <summary>
    /// General behavior for <see cref="CurrentControlledVoltageSource"/>
    /// </summary>
    public class BiasingBehavior : Behavior, IBiasingBehavior
    {
        /// <summary>
        /// Gets the base parameters.
        /// </summary>
        protected BaseParameters BaseParameters { get; private set; }

        /// <summary>
        /// Gets the current through the source.
        /// </summary>
        /// <returns></returns>
        [ParameterName("i"), ParameterName("c"), ParameterName("i_r"), ParameterInfo("Output current")]
        public double GetCurrent() => BiasingState.ThrowIfNotBound(this).Solution[_brNode];

        /// <summary>
        /// Gets the voltage applied by the source.
        /// </summary>
        [ParameterName("v"), ParameterName("v_r"), ParameterInfo("Output voltage")]
        public double GetVoltage() => BiasingState.ThrowIfNotBound(this).Solution[_posNode] - BiasingState.Solution[_negNode];

        /// <summary>
        /// Gets the power dissipated by the source.
        /// </summary>
        [ParameterName("p"), ParameterName("p_r"), ParameterInfo("Power")]
        public double GetPower() => BiasingState.ThrowIfNotBound(this).Solution[_brNode] * (BiasingState.Solution[_posNode] - BiasingState.Solution[_negNode]);

        /// <summary>
        /// Gets the controlling branch equation row.
        /// </summary>
        protected Variable ControlBranch { get; private set; }

        /// <summary>
        /// Gets the branch equation.
        /// </summary>
        /// <value>
        /// The branch.
        /// </value>
        protected Variable Branch { get; private set; }

        /// <summary>
        /// Gets the matrix elements.
        /// </summary>
        /// <value>
        /// The matrix elements.
        /// </value>
        protected ElementSet<double> Elements { get; private set; }

        /// <summary>
        /// Gets the biasing simulation state.
        /// </summary>
        /// <value>
        /// The biasing simulation state.
        /// </value>
        protected IBiasingSimulationState BiasingState { get; private set; }

        private int _posNode, _negNode, _cbrNode, _brNode;

        /// <summary>
        /// Creates a new instance of the <see cref="BiasingBehavior"/> class.
        /// </summary>
        /// <param name="name">Name</param>
        public BiasingBehavior(string name) : base(name) { }

        /// <summary>
        /// Bind the behavior to a simulation.
        /// </summary>
        /// <param name="context">The binding context.</param>
        public override void Bind(BindingContext context)
        {
            base.Bind(context);

            BaseParameters = context.Behaviors.Parameters.GetValue<BaseParameters>();

            var c = (CommonBehaviors.ControlledBindingContext)context;
            BiasingState = context.States.GetValue<IBiasingSimulationState>();
            _posNode = BiasingState.Map[c.Nodes[0]];
            _negNode = BiasingState.Map[c.Nodes[1]];

            var behavior = c.ControlBehaviors.GetValue<VoltageSourceBehaviors.BiasingBehavior>();
            ControlBranch = behavior.Branch;
            _cbrNode = BiasingState.Map[ControlBranch];
            
            Branch = context.Variables.Create(Name.Combine("branch"), VariableType.Current);
            _brNode = BiasingState.Map[Branch];

            Elements = new ElementSet<double>(BiasingState.Solver,
                new MatrixLocation(_posNode, _brNode),
                new MatrixLocation(_negNode, _brNode),
                new MatrixLocation(_brNode, _posNode),
                new MatrixLocation(_brNode, _negNode),
                new MatrixLocation(_brNode, _cbrNode));
        }

        /// <summary>
        /// Unbind the behavior.
        /// </summary>
        public override void Unbind()
        {
            base.Unbind();
            BiasingState = null;
            Elements?.Destroy();
            Elements = null;
        }

        /// <summary>
        /// Execute behavior
        /// </summary>
        void IBiasingBehavior.Load()
        {
            Elements.Add(1, -1, 1, -1, -BaseParameters.Coefficient);
        }

        /// <summary>
        /// Tests convergence at the device-level.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the device determines the solution converges; otherwise, <c>false</c>.
        /// </returns>
        bool IBiasingBehavior.IsConvergent() => true;
    }
}
