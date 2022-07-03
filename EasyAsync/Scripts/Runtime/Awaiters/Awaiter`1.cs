using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using UnityEngine.Assertions;

namespace AillieoUtils.EasyAsync
{
    public class Awaiter<T> : ICriticalNotifyCompletion
    {
        private Exception exception;
        private Action continuation;
        private T result;

        public bool IsCompleted { get; private set; } = false;

        public T GetResult()
        {
            Assert.IsTrue(IsCompleted);

            if (exception != null)
            {
                ExceptionDispatchInfo.Capture(exception).Throw();
            }

            return result;
        }

        public void Complete(T result, Exception e = null)
        {
            Assert.IsFalse(IsCompleted);

            IsCompleted = true;
            exception = e;
            this.result = result;

            continuation?.Invoke();
        }

        void INotifyCompletion.OnCompleted(Action continuation)
        {
            Assert.IsNull(this.continuation);
            Assert.IsFalse(IsCompleted);

            this.continuation = continuation;
        }

        void ICriticalNotifyCompletion.UnsafeOnCompleted(Action continuation)
        {
            Assert.IsNull(this.continuation);
            Assert.IsFalse(IsCompleted);

            this.continuation = continuation;
        }
    }
}
