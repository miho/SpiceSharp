﻿using SpiceSharp.Behaviors;

namespace SpiceSharp.Components.CurrentSourceBehaviors
{
    /// <summary>
    /// Accept behavior for a <see cref="CurrentSource"/>
    /// </summary>
    public class AcceptBehavior : BiasingBehavior, IAcceptBehavior
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AcceptBehavior"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="context">The context.</param>
        public AcceptBehavior(string name, ComponentBindingContext context) 
            : base(name, context) 
        {
        }

        /// <summary>
        /// Called when a new timepoint is being tested.
        /// </summary>
        void IAcceptBehavior.Probe()
        {
            Waveform?.Probe();
        }

        /// <summary>
        /// Accept the current timepoint
        /// </summary>
        void IAcceptBehavior.Accept()
        {
            Waveform?.Accept();
        }
    }
}
