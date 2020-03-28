﻿using SpiceSharp.Simulations.Variables;
using System;

namespace SpiceSharp.Simulations
{
    /// <summary>
    /// An <see cref="IVariable{T}"/> that takes its value from an <see cref="ISolverSimulationState{T}"/>.
    /// </summary>
    /// <typeparam name="T">The base value type.</typeparam>
    /// <seealso cref="IVariable{T}" />
    public class SolverVariable<T> : IVariable<T> where T : IFormattable
    {
        private readonly ISolverSimulationState<T> _state;
        private readonly int _index;

        /// <summary>
        /// Gets the name of the variable.
        /// </summary>
        /// <value>
        /// The name of the variable.
        /// </value>
        public string Name { get; }

        /// <summary>
        /// Gets the value of the variable.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public T Value => _state.Solution[_index];

        /// <summary>
        /// Gets the units of the quantity.
        /// </summary>
        /// <value>
        /// The units.
        /// </value>
        public IUnit Unit { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SolverVariable{T}"/> class.
        /// </summary>
        /// <param name="state">The state where to find the solution of the variable.</param>
        /// <param name="name">The name of the variable.</param>
        /// <param name="index">The index of the variable.</param>
        /// <param name="unit">The unit of the variable.</param>
        public SolverVariable(ISolverSimulationState<T> state, string name, int index, IUnit unit)
        {
            _state = state.ThrowIfNull(nameof(state));
            Name = name.ThrowIfNull(nameof(name));
            _index = index;
            Unit = unit;
        }
    }
}