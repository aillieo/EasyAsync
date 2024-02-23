// -----------------------------------------------------------------------
// <copyright file="SwitchToMainThread.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.EasyAsync
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using UnityEngine;
    using UnityEngine.Assertions;

    /// <summary>
    /// Represents a switch to the main thread.
    /// </summary>
    public struct SwitchToMainThread : INotifyCompletion
    {
        private static SynchronizationContext unitySynchronizationContext;

        /// <summary>
        /// Gets a value indicating whether the switch to the main thread is completed.
        /// </summary>
        public bool IsCompleted => unitySynchronizationContext == SynchronizationContext.Current;

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
        public SwitchToMainThread GetAwaiter()
        {
            return this;
        }

        /// <inheritdoc/>
        void INotifyCompletion.OnCompleted(Action continuation)
        {
            Assert.IsNotNull(continuation);
            Assert.IsFalse(this.IsCompleted);
            unitySynchronizationContext.Post(_ => continuation(), default);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void ConfigUnitySynchronizationContext()
        {
            unitySynchronizationContext = SynchronizationContext.Current;
        }
    }
}
