﻿using SpiceSharp.Behaviors;
using SpiceSharp.Circuits;
using System;
using System.Collections.Generic;

namespace SpiceSharp.Simulations
{
    /// <summary>
    /// Interface that describes a simulation.
    /// </summary>
    public interface ISimulation
    {
        /// <summary>
        /// Gets the current status of the <see cref="ISimulation"/>.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        SimulationStatus Status { get; }

        /// <summary>
        /// Gets a set of configurations for the <see cref="ISimulation"/>.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        ParameterSetDictionary Configurations { get; }

        /// <summary>
        /// Gets a set of <see cref="SimulationState"/> instances used by the <see cref="ISimulation"/>.
        /// </summary>
        /// <value>
        /// The states.
        /// </value>
        TypeDictionary<SimulationState> States { get; }

        /// <summary>
        /// Gets a set of <see cref="SpiceSharp.Simulations.Statistics"/> instances tracked by the <see cref="ISimulation"/>.
        /// </summary>
        /// <value>
        /// The statistics.
        /// </value>
        TypeDictionary<Statistics> Statistics { get; }

        /// <summary>
        /// Gets the variables.
        /// </summary>
        /// <value>
        /// The variables.
        /// </value>
        IVariableSet Variables { get; }

        /// <summary>
        /// Gets the name of the <see cref="ISimulation"/>.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; }

        /// <summary>
        /// Gets the entity behaviors.
        /// </summary>
        /// <value>
        /// The entity behaviors.
        /// </value>
        BehaviorPool EntityBehaviors { get; }

        /// <summary>
        /// Gets the entity parameters.
        /// </summary>
        /// <value>
        /// The entity parameters.
        /// </value>
        ParameterPool EntityParameters { get; }

        /// <summary>
        /// Gets the <see cref="IBehavior"/> types used by the <see cref="ISimulation"/>.
        /// </summary>
        /// <value>
        /// The behavior types.
        /// </value>
        IEnumerable<Type> BehaviorTypes { get; }

        /// <summary>
        /// Runs the <see cref="ISimulation"/> on the specified <see cref="IEntityCollection"/>.
        /// </summary>
        /// <param name="entities">The entities.</param>
        void Run(IEntityCollection entities);
    }

    /// <summary>
    /// Possible statuses for a simulation.
    /// </summary>
    public enum SimulationStatus
    {
        /// <summary>
        /// Indicates that the simulation has not started.
        /// </summary>
        None,

        /// <summary>
        /// Indicates that the simulation is now in its setup phase.
        /// </summary>
        Setup,

        /// <summary>
        /// Indicates that the simulation is running.
        /// </summary>
        Running,

        /// <summary>
        /// Indicates that the simulation is cleaning up all its resources.
        /// </summary>
        Unsetup
    }
}