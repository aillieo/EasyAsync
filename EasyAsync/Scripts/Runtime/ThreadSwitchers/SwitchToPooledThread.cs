// -----------------------------------------------------------------------
// <copyright file="SwitchToPooledThread.cs" company="AillieoTech">
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
    /// Represents a switch to a pooled thread.
    /// </summary>
    public struct SwitchToPooledThread : INotifyCompletion
    {
        /// <summary>
        /// Gets a value indicating whether the switch to a pooled thread is completed.
        /// </summary>
        public bool IsCompleted => false;

        /// <inheritdoc/>
        void INotifyCompletion.OnCompleted(Action continuation)
        {
            Assert.IsNotNull(continuation);
            Assert.IsFalse(this.IsCompleted);
            ThreadPool.QueueUserWorkItem(_ => continuation());
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
        public SwitchToPooledThread GetAwaiter()
        {
            return this;
        }
    }
}
