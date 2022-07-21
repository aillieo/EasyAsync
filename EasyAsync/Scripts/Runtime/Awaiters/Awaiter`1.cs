using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using UnityEngine.Assertions;

namespace AillieoUtils.EasyAsync
{
    public struct Awaiter<T> : ICriticalNotifyCompletion
    {
        internal readonly Promise<T> promise;

        public Awaiter(Promise<T> promise)
        {
            this.promise = promise;
        }

        public bool IsCompleted
        {
            get
            {
                if (promise != null)
                {
                    return promise.state != Promise.State.Pending;
                }

                return true;
            }
        }

        public T GetResult()
        {
            Assert.IsTrue(IsCompleted);

            if (promise != null && promise.exception != null)
            {
                ExceptionDispatchInfo.Capture(promise.exception).Throw();
            }

            return promise.value;
        }

        //public void Complete(T result, Exception e = null)
        //{
        //    Assert.IsFalse(IsCompleted);
        //    Assert.IsNotNull(promise);

        //    if (e == null)
        //    {
        //        promise.Resolve(result);
        //    }
        //    else
        //    {
        //        promise.Reject(e);
        //    }
        //}

        void INotifyCompletion.OnCompleted(Action continuation)
        {
            Assert.IsFalse(IsCompleted);
            Assert.IsNotNull(promise);

            promise.OnFulfilled(continuation).OnRejected(continuation);
        }

        void ICriticalNotifyCompletion.UnsafeOnCompleted(Action continuation)
        {
            Assert.IsFalse(IsCompleted);
            Assert.IsNotNull(promise);

            promise.OnFulfilled(continuation).OnRejected(continuation);
        }
    }
}
