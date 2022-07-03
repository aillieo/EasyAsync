using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Assertions;

namespace AillieoUtils.EasyAsync
{
    public abstract class AbstractPromise
    {
        private Promise.State s = Promise.State.Pending;
        public Promise.State state
        {
            get { return s;}
            protected set
            {
                lock (this)
                {
                    Assert.AreEqual(s, Promise.State.Pending);
                    Assert.AreNotEqual(value, Promise.State.Pending);
                    s = value;
                }
            }
        }

        public string reason { get; protected set; }
        public Exception exception { get; protected set; }

        internal Queue<Callback> callbacks;

        public AbstractPromise OnFulfilled(Action onFulfilled)
        {
            if (state == Promise.State.Pending)
            {
                this.callbacks = this.callbacks ?? CallbackQueue.Get();
                this.callbacks.Enqueue(new Callback(onFulfilled, Promise.State.Fulfilled));
            }
            else if (state == Promise.State.Fulfilled)
            {
                onFulfilled();
            }
            return this;
        }

        public AbstractPromise OnRejected(Action onRejected)
        {
            if (state == Promise.State.Pending)
            {
                this.callbacks = this.callbacks ?? CallbackQueue.Get();
                this.callbacks.Enqueue(new Callback(onRejected, Promise.State.Rejected));
            }
            else if (state == Promise.State.Rejected)
            {
                onRejected();
            }
            return this;
        }

        public AbstractPromise OnRejected(Action<string> onRejected)
        {
            if (state == Promise.State.Pending)
            {
                this.callbacks = this.callbacks ?? CallbackQueue.Get();
                this.callbacks.Enqueue(new Callback(() => onRejected(reason), Promise.State.Rejected));
            }
            else if (state == Promise.State.Rejected)
            {
                onRejected(reason);
            }
            return this;
        }

        public AbstractPromise Then(Action onFulfilled, Action onRejected = null)
        {
            if (state == Promise.State.Pending)
            {
                Promise newPromise = new Promise();
                this.callbacks = this.callbacks ?? CallbackQueue.Get();
                this.callbacks.Enqueue(new Callback(
                    onFulfilled,
                    Promise.State.Fulfilled));
                this.callbacks.Enqueue(new Callback(
                    onRejected,
                    Promise.State.Rejected));
            }
            else if (state == Promise.State.Fulfilled)
            {
                onFulfilled();
            }
            else if (state == Promise.State.Rejected)
            {
                onRejected();
            }

            return this;
        }

        public AbstractPromise Then(Action onFulfilled, Action<string> onRejected = null)
        {
            if (state == Promise.State.Pending)
            {
                this.callbacks = this.callbacks ?? CallbackQueue.Get();
                this.callbacks.Enqueue(new Callback(
                    onFulfilled,
                    Promise.State.Fulfilled));
                this.callbacks.Enqueue(new Callback(
                    () => onRejected(this.reason),
                    Promise.State.Rejected));
            }
            else if (state == Promise.State.Fulfilled)
            {
                onFulfilled();
            }
            else if (state == Promise.State.Rejected)
            {
                onRejected(this.reason);
            }

            return this;
        }

        public AbstractPromise Then(Func<Promise> onFulfilled, Func<AbstractPromise> onRejected = null)
        {
            if (state == Promise.State.Pending)
            {
                Promise newPromise = new Promise();
                this.callbacks = this.callbacks ?? CallbackQueue.Get();
                this.callbacks.Enqueue(new Callback(
                    () => onFulfilled()?
                        .OnFulfilled(() => newPromise.Resolve()),
                    Promise.State.Fulfilled));
                this.callbacks.Enqueue(new Callback(
                    () => onRejected()?
                        .OnRejected(rsn => newPromise.Reject(rsn)),
                    Promise.State.Rejected));
                return newPromise;
            }
            else if (state == Promise.State.Fulfilled)
            {
                return onFulfilled();
            }
            else if (state == Promise.State.Rejected)
            {
                return onRejected();
            }

            throw new Exception();
        }

        public void Reject(string reason)
        {
            this.reason = reason;
            this.exception = new Exception(reason);

            state = Promise.State.Rejected;

            ProcessCallbacks();
        }

        public void Reject(Exception exception)
        {
            this.exception = exception;
            this.reason = exception.Message;
            state = Promise.State.Rejected;

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
                        if ((callback.flag & state) != 0)
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
