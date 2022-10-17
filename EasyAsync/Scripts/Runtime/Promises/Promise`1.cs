using System;
using System.Runtime.CompilerServices;
using UnityEngine.Assertions;

namespace AillieoUtils.EasyAsync
{
    [AsyncMethodBuilder(typeof(EasyAsyncTaskMethodBuilder<>))]
    public sealed partial class Promise<T> : AbstractPromise
    {
        private T resultValue;

        public T result
        {
            get
            {
                Assert.AreNotEqual(status, Promise.Status.Pending);
                return resultValue;
            }

            private set
            {
                Assert.AreEqual(status, Promise.Status.Pending);
                resultValue = value;
            }
        }

        public Promise<T> OnFulfilled(Action<T> onFulfilled)
        {
            if (status == Promise.Status.Pending)
            {
                this.callbacks = this.callbacks ?? CallbackQueue.Get();
                this.callbacks.Enqueue(new Callback(Promise.Status.Fulfilled, () => onFulfilled(this.result)));
            }
            else if (status == Promise.Status.Fulfilled)
            {
                if (callbacks != null)
                {
                    this.callbacks.Enqueue(new Callback(Promise.Status.Fulfilled, () => onFulfilled(this.result)));
                }
                else
                {
                    onFulfilled(result);
                }
            }

            return this;
        }

        public Promise<T> Then(Func<Promise<T>> onFulfilled, Func<Promise<T>> onRejected)
        {
            if (status == Promise.Status.Pending)
            {
                Promise<T> newPromise = new Promise<T>();
                this.callbacks = this.callbacks ?? CallbackQueue.Get();
                this.callbacks.Enqueue(new Callback(Promise.Status.Fulfilled, () => onFulfilled()?.OnFulfilled(val => newPromise.Resolve(val))));
                this.callbacks.Enqueue(new Callback(Promise.Status.Rejected, () => onFulfilled()?.OnRejected(rsn => newPromise.Reject(rsn))));
                return newPromise;
            }
            else if (status == Promise.Status.Fulfilled)
            {
                if (callbacks != null)
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
            else if (status == Promise.Status.Rejected)
            {
                if (callbacks != null)
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

        public void Resolve(T v)
        {
            result = v;
            status = Promise.Status.Fulfilled;
            ProcessCallbacks();
        }

        public Awaiter<T> GetAwaiter()
        {
            return new Awaiter<T>(this);
        }

        public Promise ToTypeless()
        {
            return ((AbstractPromise)this).Then((Action)null, (Action)null) as Promise;
        }
    }
}
