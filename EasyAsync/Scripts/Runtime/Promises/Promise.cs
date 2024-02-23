// -----------------------------------------------------------------------
// <copyright file="Promise.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.EasyAsync
{
    using System;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Represents a promise that can be fulfilled or rejected.
    /// </summary>
    [AsyncMethodBuilder(typeof(EasyAsyncTaskMethodBuilder))]
    public sealed partial class Promise : AbstractPromise
    {
        /// <summary>
        /// Represents the status of the promise.
        /// </summary>
        [Flags]
        public enum Status : byte
        {
            /// <summary>
            /// The promise is pending.
            /// </summary>
            Pending = 0b00,

            /// <summary>
            /// The promise has been fulfilled.
            /// </summary>
            Fulfilled = 0b01,

            /// <summary>
            /// The promise has been rejected.
            /// </summary>
            Rejected = 0b10,
        }

        /// <summary>
        /// Resolves the promise.
        /// </summary>
        public void Resolve()
        {
            this.status = Status.Fulfilled;
            this.ProcessCallbacks();
        }

        /// <summary>
        /// Gets an awaiter for the promise.
        /// </summary>
        /// <returns>An awaiter for the promise.</returns>
        public Awaiter GetAwaiter()
        {
            return new Awaiter(this);
        }
    }
}
