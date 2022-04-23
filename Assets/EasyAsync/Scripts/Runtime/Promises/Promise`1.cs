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

        public Promise(Action onFulfilled = null, Action<string> onRejected = null)
        {
            if (onFulfilled != null)
            {
                OnFulfilled(onFulfilled);
            }

            if (onRejected != null)
            {
                OnRejected(onRejected);
            }
        }

        public Promise(Action<T> onFulfilled, Action<string> onRejected = null)
        {
            if (onFulfilled != null)
            {
                OnFulfilled(onFulfilled);
            }

            if (onRejected != null)
            {
                OnRejected(onRejected);
            }
        }

        public Promise<T> OnFulfilled(Action<T> onFulfilled)
        {
            if (state == State.Pending)
            {
                this.callbacks ??= new Queue<Callback>();
                this.callbacks.Enqueue(new Callback(() => onFulfilled(value), State.Fulfilled));
            }
            else if (state == State.Fulfilled)
            {
                onFulfilled(value);
            }
            return this;
        }

        public Promise<T> Then(Func<Promise<T>> onFulfilled, Func<Promise<T>> onRejected)
        {
            if (state == State.Pending)
            {
                Promise<T> newPromise = new Promise<T>();
                this.callbacks ??= new Queue<Callback>();
                this.callbacks.Enqueue(new Callback(
                    () => onFulfilled()?
                        .OnFulfilled(value => newPromise.Resolve(value)),
                    State.Fulfilled));
                this.callbacks.Enqueue(new Callback(
                    () => onFulfilled()?
                        .OnRejected(value => newPromise.Reject(reason)),
                    State.Rejected));
                return newPromise;
            }
            else if (state == State.Fulfilled)
            {
                return onFulfilled();
            }
            else if (state == State.Rejected)
            {
                return onRejected();
            }

            throw new Exception();
        }

        public void Resolve(T v)
        {
            value = v;
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

        public Awaiter<T> GetAwaiter()
        {
            Awaiter<T> awaiter = new Awaiter<T>();
            OnFulfilled(o => awaiter.Complete(o));
            return awaiter;
        }
    }
}
