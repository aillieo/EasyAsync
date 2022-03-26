using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Assertions;

namespace AillieoUtils.EasyAsync
{
    public sealed partial class Promise<T> : AbstractPromise
    {
        private T v;

        public T value
        {
            get
            {
                Assert.AreNotEqual(state, State.Pending);
                return v;
            }

            private set
            {
                Assert.AreEqual(state, State.Pending);
                v = value;
            }
        }

        public Promise<T> OnFulfilled(Action<T> func)
        {
            if (state == State.Pending)
            {
                this.callbacks = this.callbacks ?? new Queue<Callback>();
                this.callbacks.Enqueue(new Callback(() => func(value), State.Fulfilled));
            }
            else if (state == State.Fulfilled)
            {
                func(value);
            }
            return this;
        }

        public Promise<T> OnComplete(Action<T> func)
        {
            if(state == State.Pending)
            {
                this.callbacks = this.callbacks ?? new Queue<Callback>();
                this.callbacks.Enqueue(new Callback(() => func(value), State.Fulfilled | State.Rejected));
            }
            else
            {
                func(value);
            }
            return this;
        }

        public Promise<T> Then(Func<Promise<T>> func)
        {
            Promise<T> newPromise = new Promise<T>();
            if (state == State.Pending)
            {
                this.callbacks = this.callbacks ?? new Queue<Callback>();
                this.callbacks.Enqueue(new Callback(
                    () => func()?
                        .OnFulfilled(value => newPromise.Resolve(value))
                        .OnRejected(value => newPromise.Reject(reason)),
                    State.Fulfilled | State.Rejected));
            }
            else
            {
                return func();
            }
            return newPromise;
        }

        public void Resolve(T arg)
        {
            Assert.AreEqual(state, State.Pending);

            value = arg;
            state = State.Fulfilled;

            if(callbacks != null)
            {
                while (callbacks.Count > 0)
                {
                    Callback callback = callbacks.Dequeue();
                    if ((callback.flag & state) != 0)
                    {
                        callback.action?.Invoke();
                    }
                }
                callbacks = null;
            }
        }

        public void Reject(string reason)
        {
            Assert.AreEqual(state, State.Pending);

            this.reason = reason;
            state = State.Rejected;

            if(callbacks != null)
            {
                while (callbacks.Count > 0)
                {
                    Callback callback = callbacks.Dequeue();
                    if ((callback.flag & state) != 0)
                    {
                        callback.action?.Invoke();
                    }
                }
                callbacks = null;
            }
        }
    }
}
