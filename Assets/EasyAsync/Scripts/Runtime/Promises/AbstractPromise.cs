using System.Collections.Generic;
using System;
using UnityEngine.Assertions;

namespace AillieoUtils.EasyAsync
{
    public abstract class AbstractPromise
    {
        private Promise.Status statusValue = Promise.Status.Pending;

        public Promise.Status status
        {
            get
            {
                return statusValue;
            }

            protected set
            {
                lock (this)
                {
                    Assert.AreEqual(statusValue, Promise.Status.Pending);
                    Assert.AreNotEqual(value, Promise.Status.Pending);
                    statusValue = value;
                }
            }
        }

        public string reason { get; protected set; }

        public Exception exception { get; protected set; }

        internal Queue<Callback> callbacks;

        public AbstractPromise OnFulfilled(Action onFulfilled)
        {
            if (status == Promise.Status.Pending)
            {
                this.callbacks = this.callbacks ?? CallbackQueue.Get();
                this.callbacks.Enqueue(new Callback(Promise.Status.Fulfilled, onFulfilled));
            }
            else if (status == Promise.Status.Fulfilled)
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

        public AbstractPromise OnRejected(Action onRejected)
        {
            if (status == Promise.Status.Pending)
            {
                this.callbacks = this.callbacks ?? CallbackQueue.Get();
                this.callbacks.Enqueue(new Callback(Promise.Status.Rejected, onRejected));
            }
            else if (status == Promise.Status.Rejected)
            {
                if (callbacks != null)
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

        public AbstractPromise OnRejected(Action<string> onRejected)
        {
            if (status == Promise.Status.Pending)
            {
                this.callbacks = this.callbacks ?? CallbackQueue.Get();
                this.callbacks.Enqueue(new Callback(Promise.Status.Rejected, () => onRejected(this.reason)));
            }
            else if (status == Promise.Status.Rejected)
            {
                if (callbacks != null)
                {
                    this.callbacks.Enqueue(new Callback(Promise.Status.Rejected, () => onRejected(this.reason)));
                }
                else
                {
                    onRejected(reason);
                }
            }

            return this;
        }

        public AbstractPromise Then(Action onFulfilled, Action onRejected = null)
        {
            if (status == Promise.Status.Pending)
            {
                this.callbacks = this.callbacks ?? CallbackQueue.Get();
                this.callbacks.Enqueue(new Callback(Promise.Status.Fulfilled, onFulfilled));
                this.callbacks.Enqueue(new Callback(Promise.Status.Rejected, onRejected));
            }
            else if (status == Promise.Status.Fulfilled)
            {
                if (callbacks != null)
                {
                    this.callbacks.Enqueue(new Callback(Promise.Status.Fulfilled, onFulfilled));
                }
                else
                {
                    onFulfilled();
                }
            }
            else if (status == Promise.Status.Rejected)
            {
                if (callbacks != null)
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

        public AbstractPromise Then(Action onFulfilled, Action<string> onRejected = null)
        {
            if (status == Promise.Status.Pending)
            {
                this.callbacks = this.callbacks ?? CallbackQueue.Get();
                this.callbacks.Enqueue(new Callback(Promise.Status.Fulfilled, onFulfilled));
                this.callbacks.Enqueue(new Callback(Promise.Status.Rejected, () => onRejected(this.reason)));
            }
            else if (status == Promise.Status.Fulfilled)
            {
                if (callbacks != null)
                {
                    this.callbacks.Enqueue(new Callback(Promise.Status.Fulfilled, onFulfilled));
                }
                else
                {
                    onFulfilled();
                }
            }
            else if (status == Promise.Status.Rejected)
            {
                if (callbacks != null)
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

        public AbstractPromise Then(Func<Promise> onFulfilled, Func<AbstractPromise> onRejected = null)
        {
            if (status == Promise.Status.Pending)
            {
                Promise newPromise = new Promise();
                this.callbacks = this.callbacks ?? CallbackQueue.Get();
                this.callbacks.Enqueue(new Callback(Promise.Status.Fulfilled, () => onFulfilled()?.OnFulfilled(() => newPromise.Resolve())));
                this.callbacks.Enqueue(new Callback(Promise.Status.Rejected, () => onRejected()?.OnRejected(rsn => newPromise.Reject(rsn))));
                return newPromise;
            }
            else if (status == Promise.Status.Fulfilled)
            {
                if (callbacks != null)
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
            else if (status == Promise.Status.Rejected)
            {
                if (callbacks != null)
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

        public void Reject(string reason)
        {
            this.reason = reason;
            this.exception = new Exception(reason);

            status = Promise.Status.Rejected;

            ProcessCallbacks();
        }

        public void Reject(Exception exception)
        {
            if (exception != null)
            {
                this.exception = exception;
                this.reason = exception.Message;
            }

            status = Promise.Status.Rejected;

            ProcessCallbacks();
        }

        protected void ProcessCallbacks()
        {
            if (callbacks != null)
            {
                try
                {
                    while (callbacks.Count > 0)
                    {
                        Callback callback = callbacks.Dequeue();
                        if ((callback.mask & status) != 0)
                        {
                            callback.action?.Invoke();
                        }
                    }
                }
                finally
                {
                    callbacks.Clear();
                    CallbackQueue.Recycle(callbacks);
                    callbacks = null;
                }
            }
        }
    }
}
