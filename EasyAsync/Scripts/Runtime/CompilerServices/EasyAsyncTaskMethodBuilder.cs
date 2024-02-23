// -----------------------------------------------------------------------
// <copyright file="EasyAsyncTaskMethodBuilder.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.EasyAsync
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Provides a builder for asynchronous tasks that returns a <see cref="Promise"/>.
    /// </summary>
    public struct EasyAsyncTaskMethodBuilder
    {
        private Promise promise;

        /// <summary>
        /// Gets the <see cref="Promise"/> associated with the task being built.
        /// </summary>
        [DebuggerHidden]
        public Promise Task
        {
            get
            {
                return this.promise;
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="EasyAsyncTaskMethodBuilder"/> class.
        /// </summary>
        /// <returns>A new instance of <see cref="EasyAsyncTaskMethodBuilder"/>.</returns>
        [DebuggerHidden]
        public static EasyAsyncTaskMethodBuilder Create()
        {
            EasyAsyncTaskMethodBuilder builder = default;
            builder.promise = new Promise();
            return builder;
        }

        /// <summary>
        /// Starts the task's state machine.
        /// </summary>
        /// <typeparam name="TStateMachine">The type of the state machine.</typeparam>
        /// <param name="stateMachine">The state machine.</param>
        [DebuggerHidden]
        public void Start<TStateMachine>(ref TStateMachine stateMachine)
            where TStateMachine : IAsyncStateMachine
        {
            stateMachine.MoveNext();
        }

        /// <summary>
        /// Sets the state machine of the builder.
        /// </summary>
        /// <param name="stateMachine">The state machine.</param>
        [DebuggerHidden]
        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {
        }

        /// <summary>
        /// Configures the builder to await the completion of the specified awaiter.
        /// </summary>
        /// <typeparam name="TAwaiter">The type of the awaiter.</typeparam>
        /// <typeparam name="TStateMachine">The type of the state machine.</typeparam>
        /// <param name="awaiter">The awaiter to await.</param>
        /// <param name="stateMachine">The state machine.</param>
        [DebuggerHidden]
        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            awaiter.OnCompleted(stateMachine.MoveNext);
        }

        /// <summary>
        /// Configures the builder to await the completion of the specified awaiter in an unsafe manner.
        /// </summary>
        /// <typeparam name="TAwaiter">The type of the awaiter.</typeparam>
        /// <typeparam name="TStateMachine">The type of the state machine.</typeparam>
        /// <param name="awaiter">The awaiter to await.</param>
        /// <param name="stateMachine">The state machine.</param>
        [DebuggerHidden]
        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            awaiter.UnsafeOnCompleted(stateMachine.MoveNext);
        }

        /// <summary>
        /// Sets the result of the task.
        /// </summary>
        [DebuggerHidden]
        public void SetResult()
        {
            this.Task.Resolve();
        }

        /// <summary>
        /// Sets the exception of the task.
        /// </summary>
        /// <param name="exception">The exception to set.</param>
        [DebuggerHidden]
        public void SetException(Exception exception)
        {
            this.Task.Reject(exception);
        }
    }
}
