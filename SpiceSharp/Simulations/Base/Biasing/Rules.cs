﻿using SpiceSharp.Validation;
using System.Collections.Generic;

namespace SpiceSharp.Simulations.Biasing
{
    /// <summary>
    /// Necessary rules for biasing simulations.
    /// </summary>
    /// <seealso cref="BaseRules" />
    public class Rules : BaseRules,
        IParameterized<ComponentRuleParameters>
    {
        private readonly FloatingNodeRule _floatingNode;
        private readonly VoltageLoopRule _voltageLoop = new VoltageLoopRule();
        private readonly VariablePresenceRule _groundPresence;

        /// <summary>
        /// Gets the parameter set.
        /// </summary>
        /// <value>
        /// The parameter set.
        /// </value>
        public ComponentRuleParameters Parameters { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rules"/> class.
        /// </summary>
        /// <param name="factory">The variable factory.</param>
        public Rules(IVariableFactory<IVariable> factory)
        {
            var ground = factory.MapNode(Constants.Ground);
            _floatingNode = new FloatingNodeRule(ground);
            _groundPresence = new VariablePresenceRule(ground);
            Parameters = new ComponentRuleParameters(factory);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public override IEnumerator<IRule> GetEnumerator()
        {
            yield return _floatingNode;
            yield return _voltageLoop;
            yield return _groundPresence;
        }
    }
}
