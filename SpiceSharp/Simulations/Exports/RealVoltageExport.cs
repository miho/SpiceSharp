﻿using System;

namespace SpiceSharp.Simulations
{
    /// <summary>
    /// This class can export real voltages.
    /// </summary>
    /// <seealso cref="Export{T}" />
    public class RealVoltageExport : Export<double>
    {
        /// <summary>
        /// Gets the identifier of the positive node.
        /// </summary>
        /// <value>
        /// The positive node identifier.
        /// </value>
        public string PosNode { get; }

        /// <summary>
        /// Gets the identifier of the negative node.
        /// </summary>
        /// <value>
        /// The negative node identifier.
        /// </value>
        public string NegNode { get; }

        /// <summary>
        /// Check if the simulation is a <see cref="BaseSimulation"/>.
        /// </summary>
        /// <param name="simulation"></param>
        /// <returns></returns>
        protected override bool IsValidSimulation(Simulation simulation) => simulation is BaseSimulation;

        /// <summary>
        /// Initializes a new instance of the <see cref="RealVoltageExport"/> class.
        /// </summary>
        /// <param name="posNode">The node identifier.</param>
        /// <exception cref="ArgumentNullException">posNode</exception>
        public RealVoltageExport(string posNode)
        {
            PosNode = posNode.ThrowIfNull(nameof(posNode));
            NegNode = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RealVoltageExport"/> class.
        /// </summary>
        /// <param name="simulation">The simulation.</param>
        /// <param name="posNode">The node identifier.</param>
        /// <exception cref="ArgumentNullException">posNode</exception>
        public RealVoltageExport(BaseSimulation simulation, string posNode)
            : base(simulation)
        {
            PosNode = posNode.ThrowIfNull(nameof(posNode));
            NegNode = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RealVoltageExport"/> class.
        /// </summary>
        /// <param name="simulation">The simulation.</param>
        /// <param name="posNode">The positive node identifier.</param>
        /// <param name="negNode">The negative node identifier.</param>
        /// <exception cref="ArgumentNullException">posNode</exception>
        public RealVoltageExport(BaseSimulation simulation, string posNode, string negNode)
            : base(simulation)
        {
            PosNode = posNode.ThrowIfNull(nameof(posNode));
            NegNode = negNode;
        }

        /// <summary>
        /// Initializes the export.
        /// </summary>
        /// <param name="sender">The object (simulation) sending the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs" /> instance containing the event data.</param>
        protected override void Initialize(object sender, EventArgs e)
        {
            // Create our extractor!
            var state = ((BaseSimulation) Simulation).RealState.ThrowIfNull("real state");
            if (Simulation.Variables.TryGetNode(PosNode, out var posNode))
            {
                var posNodeIndex = posNode.Index;
                if (NegNode == null)
                    Extractor = () => state.Solution[posNodeIndex];
                else if (Simulation.Variables.TryGetNode(NegNode, out var negNode))
                {
                    var negNodeIndex = negNode.Index;
                    Extractor = () => state.Solution[posNodeIndex] - state.Solution[negNodeIndex];
                }
            }
        }
    }
}
