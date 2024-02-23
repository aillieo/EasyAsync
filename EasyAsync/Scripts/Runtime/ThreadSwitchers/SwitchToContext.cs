// -----------------------------------------------------------------------
// <copyright file="SwitchToContext.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.EasyAsync
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using UnityEngine.Assertions;

    /// <summary>
    /// Represents a switch to a specific <see cref="SynchronizationContext"/>.
    /// </summary>
    public struct SwitchToContext : INotifyCompletion
    {
        private SynchronizationContext target;

        /// <summary>
        /// Initializes a new instance of the <see cref="SwitchToContext"/> struct with the specified <see cref="SynchronizationContext"/>.
        /// </summary>
        /// <param name="synchronizationContext">The target <see cref="SynchronizationContext"/>.</param>
        public SwitchToContext(SynchronizationContext synchronizationContext)
        {
            this.target = synchronizationContext;
        }

        /// <summary>
        /// Gets a value indicating whether the switch to the target <see cref="SynchronizationContext"/> is completed.
        /// </summary>
        public bool IsCompleted => this.target == SynchronizationContext.Current;

        /// <inheritdoc/>
        void INotifyCompletion.OnCompleted(Action continuation)
        {
            Assert.IsNotNull(continuation);
            Assert.IsFalse(this.IsCompleted);
            this.target.Post(_ => continuation(), default);
        }

        /// <summary>
        /// Gets the result of the switch operation.
        /// </summary>
        public void GetResult()
        {
            Assert.IsTrue(this.IsCompleted);
        }

        /// <summary>
        /// Gets an awaiter for the switch operation.
        /// </summary>
        /// <returns>An awaiter for the switch operation.</returns>
        public SwitchToContext GetAwaiter()
        {
            return this;
        }
    }
}
