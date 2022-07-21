using System;
using System.Runtime.CompilerServices;
using UnityEngine.Assertions;

namespace AillieoUtils.EasyAsync
{
    [AsyncMethodBuilder(typeof(EasyAsyncTaskMethodBuilder<>))]
    public sealed partial class Promise<T> : AbstractPromise
    {
        private T v;

        public T value
        {
            get
            {
                Assert.AreNotEqual(state, Promise.State.Pending);
                return v;
            }

            private set
            {
                Assert.AreEqual(state, Promise.State.Pending);
                v = value;
            }
        }

        public Promise<T> OnFulfilled(Action<T> onFulfilled)
        {
            if (state == Promise.State.Pending)
            {
                this.callbacks = this.callbacks ?? CallbackQueue.Get();
                this.callbacks.Enqueue(new Callback(() => onFulfilled(value), Promise.State.Fulfilled));
            }
            else if (state == Promise.State.Fulfilled)
            {
                onFulfilled(value);
            }
            return this;
        }

        public Promise<T> Then(Func<Promise<T>> onFulfilled, Func<Promise<T>> onRejected)
        {
            if (state == Promise.State.Pending)
            {
                Promise<T> newPromise = new Promise<T>();
                this.callbacks = this.callbacks ?? CallbackQueue.Get();
                this.callbacks.Enqueue(new Callback(
                    () => onFulfilled()?
                        .OnFulfilled(value => newPromise.Resolve(value)),
                    Promise.State.Fulfilled));
                this.callbacks.Enqueue(new Callback(
                    () => onFulfilled()?
                        .OnRejected(value => newPromise.Reject(reason)),
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

        public void Resolve(T v)
        {
            value = v;
            state = Promise.State.Fulfilled;
            ProcessCallbacks();
        }

        public Awaiter<T> GetAwaiter()
        {
            return new Awaiter<T>(this);
        }

        public Promise ToTypeless()
        {
            return new Promise();
        }
    }
}
