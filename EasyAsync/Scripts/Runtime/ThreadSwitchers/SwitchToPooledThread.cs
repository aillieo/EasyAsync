using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine.Assertions;

namespace AillieoUtils.EasyAsync
{
    public struct SwitchToPooledThread : INotifyCompletion
    {
        void INotifyCompletion.OnCompleted(Action continuation)
        {
            Assert.IsNotNull(continuation);
            Assert.IsFalse(IsCompleted);
            ThreadPool.QueueUserWorkItem(_ => continuation());
        }

        // 可能会从一个后台线程切到另一个后台线程
        public bool IsCompleted => false;

        public void GetResult()
        {
            Assert.IsTrue(IsCompleted);
        }

        public SwitchToPooledThread GetAwaiter()
        {
            return this;
        }
    }
}
