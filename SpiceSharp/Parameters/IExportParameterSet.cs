﻿using System;

namespace SpiceSharp
{
    /// <summary>
    /// A set of properties that support exporting or getting their values.
    /// </summary>
    /// <remarks>
    /// Properties can be named using the <see cref="Attributes.ParameterNameAttribute" />.
    /// </remarks>
    public interface IExportPropertySet
    {
        /// <summary>
        /// Gets the value of the parameter with the specified name.
        /// </summary>
        /// <typeparam name="P">The value type.</typeparam>
        /// <param name="name">The name.</param>
        /// <returns>The value.</returns>
        P GetProperty<P>(string name);

        /// <summary>
        /// Tries to get the value of the parameter with the specified name.
        /// </summary>
        /// <typeparam name="P">The value type.</typeparam>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// <c>true</c> if the parameter was found; otherwise <c>false</c>.
        /// </returns>
        bool TryGetProperty<P>(string name, out P value);

        /// <summary>
        /// Creates a getter for a parameter with the specified name.
        /// </summary>
        /// <typeparam name="P">The value type.</typeparam>
        /// <param name="name">The name of the parameter.</param>
        /// <returns>
        /// A getter if the parameter exists; otherwise <c>null</c>.
        /// </returns>
        Func<P> CreatePropertyGetter<P>(string name);
    }
}