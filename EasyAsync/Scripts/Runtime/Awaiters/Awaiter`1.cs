// -----------------------------------------------------------------------
// <copyright file="Awaiter`1.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.EasyAsync
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.ExceptionServices;
    using UnityEngine.Assertions;

    /// <summary>
    /// Represents an awaiter for a <see cref="Promise{T}"/> object.
    /// </summary>
    /// <typeparam name="T">The type of the result produced by the promise.</typeparam>
    public readonly struct Awaiter<T> : ICriticalNotifyCompletion
    {
        private readonly Promise<T> promise;

        internal Awaiter(Promise<T> promise)
        {
            this.promise = promise;
        }

        /// <summary>
        /// Gets a value indicating whether the asynchronous operation associated with this awaiter has completed.
        /// </summary>
        public bool IsCompleted
        {
            get
            {
                if (this.promise != null)
                {
                    return this.promise.status != Promise.Status.Pending;
                }

                return true;
            }
        }

        /// <summary>
        /// Ends the await on the completed asynchronous operation and returns the result.
        /// </summary>
        /// <returns>The result produced by the asynchronous operation.</returns>
        public T GetResult()
        {
            Assert.IsTrue(this.IsCompleted);

            if (this.promise != null && this.promise.exception != null)
            {
                ExceptionDispatchInfo.Capture(this.promise.exception).Throw();
            }

            return this.promise.result;
        }

        /// <inheritdoc/>
        void INotifyCompletion.OnCompleted(Action continuation)
        {
            Assert.IsFalse(this.IsCompleted);
            Assert.IsNotNull(this.promise);

            this.promise.OnFulfilled(continuation).OnRejected(continuation);
        }

        /// <inheritdoc/>
        void ICriticalNotifyCompletion.UnsafeOnCompleted(Action continuation)
        {
            Assert.IsFalse(this.IsCompleted);
            Assert.IsNotNull(this.promise);

            this.promise.OnFulfilled(continuation).OnRejected(continuation);
        }
    }
}
