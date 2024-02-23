// -----------------------------------------------------------------------
// <copyright file="YieldInstructionExtensions.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.EasyAsync.CoroutineExtensions
{
    using System;
    using System.Collections;
    using UnityEngine;

    /// <summary>
    /// Provides extension methods for <see cref="YieldInstruction"/> objects.
    /// </summary>
    public static class YieldInstructionExtensions
    {
        /// <summary>
        /// Returns an awaiter for the specified <see cref="YieldInstruction"/> object.
        /// </summary>
        /// <param name="yieldInstruction">The <see cref="YieldInstruction"/> object.</param>
        /// <returns>An awaiter for the specified <see cref="YieldInstruction"/> object.</returns>
        public static Awaiter GetAwaiter(this YieldInstruction yieldInstruction)
        {
            Promise promise = new Promise();
            CoroutineRunner.Instance.StartCoroutine(new EnumWrapper(promise, yieldInstruction));
            return promise.GetAwaiter();
        }

        /// <summary>
        /// Represents a wrapper for a <see cref="YieldInstruction"/> object.
        /// </summary>
        internal class EnumWrapper : IEnumerator
        {
            private readonly Promise promise;
            private readonly YieldInstruction yieldInstruction;
            private object current;

            /// <summary>
            /// Initializes a new instance of the <see cref="EnumWrapper"/> class with the specified <see cref="Promise"/> and <see cref="YieldInstruction"/> objects.
            /// </summary>
            /// <param name="promise">The <see cref="Promise"/> object.</param>
            /// <param name="yieldInstruction">The <see cref="YieldInstruction"/> object.</param>
            public EnumWrapper(Promise promise, YieldInstruction yieldInstruction)
            {
                this.promise = promise;
                this.yieldInstruction = yieldInstruction;
            }

            object IEnumerator.Current => this.current;

            bool IEnumerator.MoveNext()
            {
                if (this.current == null)
                {
                    this.current = this.yieldInstruction;
                    return true;
                }

                this.promise.Resolve();
                return false;
            }

            void IEnumerator.Reset()
            {
                throw new NotImplementedException();
            }
        }
    }
}
