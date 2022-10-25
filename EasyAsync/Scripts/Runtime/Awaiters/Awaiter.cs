using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using UnityEngine.Assertions;

namespace AillieoUtils.EasyAsync
{
    public struct Awaiter : ICriticalNotifyCompletion
    {
        private readonly Promise promise;

        public Awaiter(Promise promise)
        {
            this.promise = promise;
        }

        public bool IsCompleted
        {
            get
            {
                if (promise != null)
                {
                    return promise.status != Promise.Status.Pending;
                }

                return true;
            }
        }

        public void GetResult()
        {
            Assert.IsTrue(IsCompleted);

            if (promise != null && promise.exception != null)
            {
                ExceptionDispatchInfo.Capture(promise.exception).Throw();
            }
        }

        //public void Complete(Exception e = null)
        //{
        //    Assert.IsFalse(IsCompleted);
        //    Assert.IsNotNull(promise);

        //    if (e == null)
        //    {
        //        promise.Resolve();
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
