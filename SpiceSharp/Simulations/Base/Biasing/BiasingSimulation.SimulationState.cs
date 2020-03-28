﻿using SpiceSharp.Algebra;
using SpiceSharp.Simulations.Variables;
using System.Collections.Generic;

namespace SpiceSharp.Simulations
{
    public abstract partial class BiasingSimulation
    {
        /// <summary>
        /// A simulation state for simulations using real numbers.
        /// </summary>
        /// <seealso cref="IBiasingSimulationState" />
        private class SimulationState : IBiasingSimulationState
        {
            private readonly VariableMap _map;
            private readonly VariableSet<IVariable<double>> _solved;

            /// <summary>
            /// Gets the solution vector.
            /// </summary>
            public IVector<double> Solution { get; private set; }

            /// <summary>
            /// Gets the previous solution vector.
            /// </summary>
            /// <remarks>
            /// This vector is needed for determining convergence.
            /// </remarks>
            public IVector<double> OldSolution { get; private set; }

            /// <summary>
            /// Gets the map that maps variables to indices for the solver.
            /// </summary>
            /// <value>
            /// The map.
            /// </value>
            public IVariableMap Map => _map;

            /// <summary>
            /// Gets the sparse solver.
            /// </summary>
            /// <value>
            /// The solver.
            /// </value>
            public ISparseSolver<double> Solver { get; }

            /// <summary>
            /// Initializes a new instance of the <see cref="SimulationState" /> class.
            /// </summary>
            /// <param name="solver">The solver.</param>
            /// <param name="solvedVariables">The set of variables that tracks the solved variables.</param>
            public SimulationState(ISparseSolver<double> solver, VariableSet<IVariable<double>> solvedVariables)
            {
                Solver = solver.ThrowIfNull(nameof(solver));
                var gnd = new GroundVariable<double>();
                _map = new VariableMap(gnd);
                _solved = solvedVariables;
                _solved.Add(gnd);
            }

            /// <summary>
            /// Maps a shared node in the simulation.
            /// </summary>
            /// <param name="name">The name of the shared node.</param>
            /// <returns>
            /// The shared node variable.
            /// </returns>
            public IVariable<double> MapNode(string name)
            {
                if (_solved.TryGetValue(name, out var result))
                    return result;
                result = Create(name, Units.Volt);
                _solved.Add(result);
                return result;
            }

            /// <summary>
            /// Maps a number of nodes.
            /// </summary>
            /// <param name="names">The nodes.</param>
            /// <returns>
            /// The shared node variables.
            /// </returns>
            public IEnumerable<IVariable<double>> MapNodes(IEnumerable<string> names)
            {
                foreach (var name in names)
                    yield return MapNode(name);
            }

            /// <summary>
            /// Determines whether the specified variable is a node without mapping it.
            /// </summary>
            /// <param name="name">The name of the node.</param>
            /// <returns>
            /// <c>true</c> if the specified variable has node; otherwise, <c>false</c>.
            /// </returns>
            public bool HasNode(string name) => _solved.ContainsKey(name);

            /// <summary>
            /// Creates a local variable that should not be shared by the state with anyone else.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <param name="unit">The unit of the variable.</param>
            /// <returns>
            /// The local variable.
            /// </returns>
            public IVariable<double> Create(string name, IUnit unit)
            {
                var result = new SolverVariable<double>(this, name, _map.Count, unit);
                _map.Add(result, _map.Count);
                return result;
            }

            /// <summary>
            /// Sets up the simulation state.
            /// </summary>
            public void Setup()
            {
                // Initialize all matrices
                Solution = new DenseVector<double>(Solver.Size);
                OldSolution = new DenseVector<double>(Solver.Size);
                Solver.Reset();
            }

            /// <summary>
            /// Stores the solution.
            /// </summary>
            public void StoreSolution()
            {
                var tmp = OldSolution;
                OldSolution = Solution;
                Solution = tmp;
            }
        }
    }
}
