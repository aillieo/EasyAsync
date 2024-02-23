// -----------------------------------------------------------------------
// <copyright file="IEnumeratorExtensions.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.EasyAsync.CoroutineExtensions
{
    using System;
    using System.Collections;

    /// <summary>
    /// Provides extension methods for <see cref="IEnumerator"/> objects.
    /// </summary>
    public static class IEnumeratorExtensions
    {
        /// <summary>
        /// Returns an awaiter for the specified <see cref="IEnumerator"/> object.
        /// </summary>
        /// <param name="enumerator">The <see cref="IEnumerator"/> object.</param>
        /// <returns>An awaiter for the specified <see cref="IEnumerator"/> object.</returns>
        public static Awaiter GetAwaiter(this IEnumerator enumerator)
        {
            Promise promise = new Promise();
            CoroutineRunner.Instance.StartCoroutine(new EnumWrapper(promise, enumerator));
            return promise.GetAwaiter();
        }

        /// <summary>
        /// Represents a wrapper for an <see cref="IEnumerator"/> object.
        /// </summary>
        internal struct EnumWrapper : IEnumerator
        {
            private readonly Promise promise;
            private readonly IEnumerator enumerator;

            /// <summary>
            /// Initializes a new instance of the <see cref="EnumWrapper"/> struct with the specified <see cref="Promise"/> and <see cref="IEnumerator"/> objects.
            /// </summary>
            /// <param name="promise">The <see cref="Promise"/> object.</param>
            /// <param name="enumerator">The <see cref="IEnumerator"/> object.</param>
            public EnumWrapper(Promise promise, IEnumerator enumerator)
            {
                this.promise = promise;
                this.enumerator = enumerator;
            }

            object IEnumerator.Current => this.enumerator.Current;

            bool IEnumerator.MoveNext()
            {
                bool result = this.enumerator.MoveNext();
                if (!result)
                {
                    this.promise.Resolve();
                }

                return result;
            }

            void IEnumerator.Reset()
            {
                throw new NotImplementedException();
            }
        }
    }
}
