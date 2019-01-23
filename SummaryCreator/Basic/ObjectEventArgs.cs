using System;

namespace SummaryCreator.Basic
{
    /// <summary>
    /// Can save one value for EventHandler.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    public class ObjectEventArgs<T> : EventArgs where T : class
    {
        /// <summary>
        /// The value of the class.
        /// </summary>
        public readonly T Obj;

        /// <summary>
        /// Construktor: Give this class a value.
        /// </summary>
        /// <param name="obj">The value of this class.</param>
        public ObjectEventArgs(T obj)
        {
            Obj = obj;
        }
    }
}
