// -----------------------------------------------------------------------
// <copyright file="AbstractPromise.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.EasyAsync
{
    using System;
    using System.Collections.Generic;
    using UnityEngine.Assertions;

    /// <summary>
    /// Represents an abstract base class for promises.
    /// </summary>
    public abstract class AbstractPromise
    {
        internal Queue<Callback> callbacks;

        private Promise.Status statusValue = Promise.Status.Pending;

        /// <summary>
        /// Gets or sets the status of the promise.
        /// </summary>
        public Promise.Status status
        {
            get
            {
                return this.statusValue;
            }

            protected set
            {
                lock (this)
                {
                    Assert.AreEqual(this.statusValue, Promise.Status.Pending);
                    Assert.AreNotEqual(value, Promise.Status.Pending);
                    this.statusValue = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the reason for the promise rejection.
        /// </summary>
        public string reason { get; protected set; }

        /// <summary>
        /// Gets or sets the exception associated with the promise rejection.
        /// </summary>
        public Exception exception { get; protected set; }

        /// <summary>
        /// Registers an action to be executed when the promise is fulfilled.
        /// </summary>
        /// <param name="onFulfilled">The action to be executed when the promise is fulfilled.</param>
        /// <returns>The current promise.</returns>
        public AbstractPromise OnFulfilled(Action onFulfilled)
        {
            if (this.status == Promise.Status.Pending)
            {
                this.callbacks = this.callbacks ?? CallbackQueue.Get();
                this.callbacks.Enqueue(new Callback(Promise.Status.Fulfilled, onFulfilled));
            }
            else if (this.status == Promise.Status.Fulfilled)
            {
                if (this.callbacks != null)
                {
                    this.callbacks.Enqueue(new Callback(Promise.Status.Fulfilled, onFulfilled));
                }
                else
                {
                    onFulfilled();
                }
            }

            return this;
        }

        /// <summary>
        /// Registers an action to be executed when the promise is rejected.
        /// </summary>
        /// <param name="onRejected">The action to be executed when the promise is rejected.</param>
        /// <returns>The current promise.</returns>
        public AbstractPromise OnRejected(Action onRejected)
        {
            if (this.status == Promise.Status.Pending)
            {
                this.callbacks = this.callbacks ?? CallbackQueue.Get();
                this.callbacks.Enqueue(new Callback(Promise.Status.Rejected, onRejected));
            }
            else if (this.status == Promise.Status.Rejected)
            {
                if (this.callbacks != null)
                {
                    this.callbacks.Enqueue(new Callback(Promise.Status.Rejected, onRejected));
                }
                else
                {
                    onRejected();
                }
            }

            return this;
        }

        /// <summary>
        /// Registers an action to be executed when the promise is rejected, providing the reason for the rejection.
        /// </summary>
        /// <param name="onRejected">The action to be executed when the promise is rejected.</param>
        /// <returns>The current promise.</returns>
        public AbstractPromise OnRejected(Action<string> onRejected)
        {
            if (this.status == Promise.Status.Pending)
            {
                this.callbacks = this.callbacks ?? CallbackQueue.Get();
                this.callbacks.Enqueue(new Callback(Promise.Status.Rejected, () => onRejected(this.reason)));
            }
            else if (this.status == Promise.Status.Rejected)
            {
                if (this.callbacks != null)
                {
                    this.callbacks.Enqueue(new Callback(Promise.Status.Rejected, () => onRejected(this.reason)));
                }
                else
                {
                    onRejected(this.reason);
                }
            }

            return this;
        }

        /// <summary>
        /// Registers actions to be executed when the promise is fulfilled or rejected.
        /// </summary>
        /// <param name="onFulfilled">The action to be executed when the promise is fulfilled.</param>
        /// <param name="onRejected">The action to be executed when the promise is rejected.</param>
        /// <returns>The current promise.</returns>
        public AbstractPromise Then(Action onFulfilled, Action onRejected = null)
        {
            if (this.status == Promise.Status.Pending)
            {
                this.callbacks = this.callbacks ?? CallbackQueue.Get();
                this.callbacks.Enqueue(new Callback(Promise.Status.Fulfilled, onFulfilled));
                this.callbacks.Enqueue(new Callback(Promise.Status.Rejected, onRejected));
            }
            else if (this.status == Promise.Status.Fulfilled)
            {
                if (this.callbacks != null)
                {
                    this.callbacks.Enqueue(new Callback(Promise.Status.Fulfilled, onFulfilled));
                }
                else
                {
                    onFulfilled();
                }
            }
            else if (this.status == Promise.Status.Rejected)
            {
                if (this.callbacks != null)
                {
                    this.callbacks.Enqueue(new Callback(Promise.Status.Rejected, onRejected));
                }
                else
                {
                    onRejected();
                }
            }

            return this;
        }

        /// <summary>
        /// Registers actions to be executed when the promise is fulfilled or rejected, providing the reason for rejection.
        /// </summary>
        /// <param name="onFulfilled">The action to be executed when the promise is fulfilled.</param>
        /// <param name="onRejected">The action to be executed when the promise is rejected.</param>
        /// <returns>The current promise.</returns>
        public AbstractPromise Then(Action onFulfilled, Action<string> onRejected = null)
        {
            if (this.status == Promise.Status.Pending)
            {
                this.callbacks = this.callbacks ?? CallbackQueue.Get();
                this.callbacks.Enqueue(new Callback(Promise.Status.Fulfilled, onFulfilled));
                this.callbacks.Enqueue(new Callback(Promise.Status.Rejected, () => onRejected(this.reason)));
            }
            else if (this.status == Promise.Status.Fulfilled)
            {
                if (this.callbacks != null)
                {
                    this.callbacks.Enqueue(new Callback(Promise.Status.Fulfilled, onFulfilled));
                }
                else
                {
                    onFulfilled();
                }
            }
            else if (this.status == Promise.Status.Rejected)
            {
                if (this.callbacks != null)
                {
                    this.callbacks.Enqueue(new Callback(Promise.Status.Rejected, () => onRejected(this.reason)));
                }
                else
                {
                    onRejected(this.reason);
                }
            }

            return this;
        }

        /// <summary>
        /// Registers functions to be executed when the promise is fulfilled or rejected, returning a new promise.
        /// </summary>
        /// <param name="onFulfilled">The function to be executed when the promise is fulfilled.</param>
        /// <param name="onRejected">The function to be executed when the promise is rejected.</param>
        /// <returns>A new promise.</returns>
        public AbstractPromise Then(Func<Promise> onFulfilled, Func<AbstractPromise> onRejected = null)
        {
            if (this.status == Promise.Status.Pending)
            {
                Promise newPromise = new Promise();
                this.callbacks = this.callbacks ?? CallbackQueue.Get();
                this.callbacks.Enqueue(new Callback(Promise.Status.Fulfilled, () => onFulfilled()?.OnFulfilled(() => newPromise.Resolve())));
                this.callbacks.Enqueue(new Callback(Promise.Status.Rejected, () => onRejected()?.OnRejected(rsn => newPromise.Reject(rsn))));
                return newPromise;
            }
            else if (this.status == Promise.Status.Fulfilled)
            {
                if (this.callbacks != null)
                {
                    Promise newPromise = new Promise();
                    this.callbacks.Enqueue(new Callback(Promise.Status.Fulfilled, () => onFulfilled()?.OnFulfilled(() => newPromise.Resolve())));
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
                    Promise newPromise = new Promise();
                    this.callbacks.Enqueue(new Callback(Promise.Status.Rejected, () => onRejected()?.OnRejected(rsn => newPromise.Reject(rsn))));
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
        /// Rejects the promise with the specified reason.
        /// </summary>
        /// <param name="reason">The reason for rejecting the promise.</param>
        public void Reject(string reason)
        {
            this.reason = reason;
            this.exception = new Exception(reason);

            this.status = Promise.Status.Rejected;

            this.ProcessCallbacks();
        }

        /// <summary>
        /// Rejects the promise with the specified exception.
        /// </summary>
        /// <param name="exception">The exception for rejecting the promise.</param>
        public void Reject(Exception exception)
        {
            if (exception != null)
            {
                this.exception = exception;
                this.reason = exception.Message;
            }

            this.status = Promise.Status.Rejected;

            this.ProcessCallbacks();
        }

        protected void ProcessCallbacks()
        {
            if (this.callbacks != null)
            {
                try
                {
                    while (this.callbacks.Count > 0)
                    {
                        Callback callback = this.callbacks.Dequeue();
                        if ((callback.mask & this.status) != 0)
                        {
                            callback.action?.Invoke();
                        }
                    }
                }
                finally
                {
                    this.callbacks.Clear();
                    CallbackQueue.Recycle(this.callbacks);
                    this.callbacks = null;
                }
            }
        }
    }
}
