using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using UnityEngine.Assertions;

namespace AillieoUtils.EasyAsync
{
    public class Awaiter : INotifyCompletion
    {
        private Exception exception;
        private Action continuation;

        public bool IsCompleted { get; private set; } = false;

        public void GetResult()
        {
            Assert.IsTrue(IsCompleted);

            if (exception != null)
            {
                ExceptionDispatchInfo.Capture(exception).Throw();
            }
        }

        public void Complete(Exception e = null)
        {
            Assert.IsFalse(IsCompleted);

            IsCompleted = true;
            exception = e;

            // todo 是否要返回到主线程
            continuation?.Invoke();
        }

        void INotifyCompletion.OnCompleted(Action continuation)
        {
            Assert.IsNull(this.continuation);
            Assert.IsFalse(IsCompleted);

            this.continuation = continuation;
        }
    }
}
