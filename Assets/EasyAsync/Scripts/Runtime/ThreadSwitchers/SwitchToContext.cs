using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine.Assertions;

namespace AillieoUtils.EasyAsync
{
    public struct SwitchToContext : INotifyCompletion
    {
        private SynchronizationContext target;

        public SwitchToContext(SynchronizationContext synchronizationContext)
        {
            target = synchronizationContext;
        }

        void INotifyCompletion.OnCompleted(Action continuation)
        {
            Assert.IsNotNull(continuation);
            Assert.IsFalse(IsCompleted);
            target.Post(_ => continuation(), default);
        }

        public bool IsCompleted => target == SynchronizationContext.Current;

        public void GetResult()
        {
            Assert.IsTrue(IsCompleted);
        }

        public SwitchToContext GetAwaiter()
        {
            return this;
        }
    }
}
