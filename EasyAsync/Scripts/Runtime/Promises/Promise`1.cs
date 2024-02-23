// -----------------------------------------------------------------------
// <copyright file="Promise`1.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.EasyAsync
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine.Assertions;

    /// <summary>
    /// Represents a promise that can be fulfilled or rejected with a value of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the value produced by the promise.</typeparam>
    [AsyncMethodBuilder(typeof(EasyAsyncTaskMethodBuilder<>))]
    public sealed partial class Promise<T> : AbstractPromise
    {
        private T resultValue;

        /// <summary>
        /// Gets the result value of the promise.
        /// </summary>
        public T result
        {
            get
            {
                Assert.AreEqual(this.status, Promise.Status.Fulfilled);
                return this.resultValue;
            }

            private set
            {
                Assert.AreEqual(this.status, Promise.Status.Pending);
                this.resultValue = value;
            }
        }

        /// <summary>
        /// Registers an action to be executed when the promise is fulfilled.
        /// </summary>
        /// <param name="onFulfilled">The action to be executed when the promise is fulfilled.</param>
        /// <returns>The current promise.</returns>
        public Promise<T> OnFulfilled(Action<T> onFulfilled)
        {
            if (this.status == Promise.Status.Pending)
            {
                this.callbacks = this.callbacks ?? CallbackQueue.Get();
                this.callbacks.Enqueue(new Callback(Promise.Status.Fulfilled, () => onFulfilled(this.result)));
            }
            else if (this.status == Promise.Status.Fulfilled)
            {
                if (this.callbacks != null)
                {
                    this.callbacks.Enqueue(new Callback(Promise.Status.Fulfilled, () => onFulfilled(this.result)));
                }
                else
                {
                    onFulfilled(this.result);
                }
            }

            return this;
        }

        /// <summary>
        /// Registers functions to be executed when the promise is fulfilled or rejected.
        /// </summary>
        /// <param name="onFulfilled">The function to be executed when the promise is fulfilled.</param>
        /// <param name="onRejected">The function to be executed when the promise is rejected.</param>
        /// <returns>A new promise that represents the result of the executed functions.</returns>
        public Promise<T> Then(Func<Promise<T>> onFulfilled, Func<Promise<T>> onRejected)
        {
            if (this.status == Promise.Status.Pending)
            {
                Promise<T> newPromise = new Promise<T>();
                this.callbacks = this.callbacks ?? CallbackQueue.Get();
                this.callbacks.Enqueue(new Callback(Promise.Status.Fulfilled, () => onFulfilled()?.OnFulfilled(val => newPromise.Resolve(val))));
                this.callbacks.Enqueue(new Callback(Promise.Status.Rejected, () => onRejected()?.OnRejected(rsn => newPromise.Reject(rsn))));
                return newPromise;
            }
            else if (this.status == Promise.Status.Fulfilled)
            {
                if (this.callbacks != null)
                {
                    Promise<T> newPromise = new Promise<T>();
                    this.callbacks.Enqueue(new Callback(Promise.Status.Fulfilled, () => onFulfilled()?.OnFulfilled(val => newPromise.Resolve(val))));
                    return newPromise;
                }
                else
                {
                    return onFulfilled();
                }
            }
            else if (this.status == Promise.Status.Rejected)
            {
                if (this.callbacks != null)
                {
                    Promise<T> newPromise = new Promise<T>();
                    this.callbacks.Enqueue(new Callback(Promise.Status.Rejected, () => onFulfilled()?.OnRejected(rsn => newPromise.Reject(rsn))));
                    return newPromise;
                }
                else
                {
                    return onRejected();
                }
            }

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Resolves the promise with the specified value.
        /// </summary>
        /// <param name="v">The value to resolve the promise with.</param>
        public void Resolve(T v)
        {
            this.result = v;
            this.status = Promise.Status.Fulfilled;
            this.ProcessCallbacks();
        }

        /// <summary>
        /// Gets an awaiter for the promise.
        /// </summary>
        /// <returns>An awaiter for the promise.</returns>
        public Awaiter<T> GetAwaiter()
        {
            return new Awaiter<T>(this);
        }

        /// <summary>
        /// Converts the promise to a typeless promise.
        /// </summary>
        /// <returns>A typeless promise.</returns>
        public Promise ToTypeless()
        {
            var promise = new Promise();
            this.Then(promise.Resolve, promise.Reject);
            return promise;
        }
    }
}
