﻿using System;

namespace SpiceSharp.Algebra
{
    public partial class SparseMatrix<T>
    {
        /// <summary>
        /// A class that keeps track of a linked list of matrix elements for a row.
        /// </summary>
        protected class Row
        {
            /// <summary>
            /// Gets the first element in the row.
            /// </summary>
            public Element FirstInRow { get; private set; }

            /// <summary>
            /// Gets the last element in the row.
            /// </summary>
            public Element LastInRow { get; private set; }

            /// <summary>
            /// Insert an element in the row. This method assumes an element does not exist at its indices!
            /// </summary>
            /// <param name="newElement">The new element to insert.</param>
            public void Insert(Element newElement)
            {
                var column = newElement.Column;
                Element element = FirstInRow, lastElement = null;
                while (element != null)
                {
                    if (element.Column > column)
                        break;
                    lastElement = element;
                    element = element.Right;
                }

                // Update links for last element
                if (lastElement == null)
                    FirstInRow = newElement;
                else
                    lastElement.Right = newElement;
                newElement.Left = lastElement;

                // Update links for next element
                if (element == null)
                    LastInRow = newElement;
                else
                    element.Left = newElement;
                newElement.Right = element;
            }

            /// <summary>
            /// Creates or get an element in the row.
            /// </summary>
            /// <param name="row">The row index used for creating a new element</param>
            /// <param name="column">The column index.</param>
            /// <param name="result">The found or created element.</param>
            /// <returns>True if the element was found, false if it was created.</returns>
            public bool CreateGetElement(int row, int column, out Element result)
            {
                Element element = FirstInRow, lastElement = null;
                while (element != null)
                {
                    if (element.Column > column)
                        break;
                    if (element.Column == column)
                    {
                        // Found the element
                        result = element;
                        return true;
                    }

                    lastElement = element;
                    element = element.Right;
                }

                // Create a new element
                result = new Element(row, column);

                // Update links for last element
                if (lastElement == null)
                    FirstInRow = result;
                else
                    lastElement.Right = result;
                result.Left = lastElement;

                // Update links for next element
                if (element == null)
                    LastInRow = result;
                else
                    element.Left = result;
                result.Right = element;

                // Could not find existing element
                return false;
            }

            /// <summary>
            /// Find an element in the row without creating it.
            /// </summary>
            /// <param name="column">The column index.</param>
            /// <returns>The element at the specified column, or null if it doesn't exist.</returns>
            public Element Find(int column)
            {
                var element = FirstInRow;
                while (element != null)
                {
                    if (element.Column == column)
                        return element;
                    if (element.Column > column)
                        return null;
                    element = element.Right;
                }

                return null;
            }

            /// <summary>
            /// Remove an element from the row.
            /// </summary>
            /// <param name="element">The element to be removed.</param>
            public void Remove(Element element)
            {
                if (element.Left == null)
                    FirstInRow = element.Right;
                else
                    element.Left.Right = element.Right;
                if (element.Right == null)
                    LastInRow = element.Left;
                else
                    element.Right.Left = element.Left;
            }

            /// <summary>
            /// Clears all matrix elements in the row.
            /// </summary>
            public void Clear()
            {
                var elt = FirstInRow;
                while (elt != null)
                {
                    elt.Left = null;
                    elt = elt.Right;
                    if (elt != null)
                        elt.Left.Right = null;
                }
                FirstInRow = null;
                LastInRow = null;
            }

            /// <summary>
            /// Swap two elements in the row, <paramref name="first"/> and <paramref name="columnFirst"/> 
            /// are supposed to come first in the row. Does not update column pointers!
            /// </summary>
            /// <param name="first">The first matrix element.</param>
            /// <param name="second">The second matrix element.</param>
            /// <param name="columnFirst">The first column.</param>
            /// <param name="columnSecond">The second column.</param>
            public void Swap(Element first, Element second, int columnFirst,
                int columnSecond)
            {
                if (first == null && second == null)
                    throw new ArgumentException("Both matrix elements cannot be null");

                if (first == null)
                {
                    // Do we need to move the element?
                    if (second.Left == null || second.Left.Column < columnFirst)
                    {
                        second.Column = columnFirst;
                        return;
                    }

                    // Move the element back
                    var element = second.Left;
                    Remove(second);
                    while (element.Left != null && element.Left.Column > columnFirst)
                        element = element.Left;

                    // We now have the first element below the insertion point
                    if (element.Left == null)
                        FirstInRow = second;
                    else
                        element.Left.Right = second;
                    second.Left = element.Left;
                    element.Left = second;
                    second.Right = element;
                    second.Column = columnFirst;
                }
                else if (second == null)
                {
                    // Do we need to move the element?
                    if (first.Right == null || first.Right.Column > columnSecond)
                    {
                        first.Column = columnSecond;
                        return;
                    }

                    // Move the element forward
                    var element = first.Right;
                    Remove(first);
                    while (element.Right != null && element.Right.Column < columnSecond)
                        element = element.Right;

                    // We now have the first element above the insertion point
                    if (element.Right == null)
                        LastInRow = first;
                    else
                        element.Right.Left = first;
                    first.Right = element.Right;
                    element.Right = first;
                    first.Left = element;
                    first.Column = columnSecond;
                }
                else
                {
                    // Are they adjacent or not?
                    if (first.Right == second)
                    {
                        // Correct surrounding links
                        if (first.Left == null)
                            FirstInRow = second;
                        else
                            first.Left.Right = second;
                        if (second.Right == null)
                            LastInRow = first;
                        else
                            second.Right.Left = first;

                        // Correct element links
                        first.Right = second.Right;
                        second.Left = first.Left;
                        first.Left = second;
                        second.Right = first;
                        first.Column = columnSecond;
                        second.Column = columnFirst;
                    }
                    else
                    {
                        // Swap surrounding links
                        if (first.Left == null)
                            FirstInRow = second;
                        else
                            first.Left.Right = second;
                        first.Right.Left = second;
                        if (second.Right == null)
                            LastInRow = first;
                        else
                            second.Right.Left = first;
                        second.Left.Right = first;

                        // Swap element links
                        var element = first.Left;
                        first.Left = second.Left;
                        second.Left = element;

                        element = first.Right;
                        first.Right = second.Right;
                        second.Right = element;
                        first.Column = columnSecond;
                        second.Column = columnFirst;
                    }
                }
            }
        }
    }
}
