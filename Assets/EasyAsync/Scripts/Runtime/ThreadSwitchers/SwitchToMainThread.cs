using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.Assertions;

namespace AillieoUtils.EasyAsync
{
    public struct SwitchToMainThread : INotifyCompletion
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void ConfigUnitySynchronizationContext()
        {
            unitySynchronizationContext = SynchronizationContext.Current;
        }

        private static SynchronizationContext unitySynchronizationContext;

        void INotifyCompletion.OnCompleted(Action continuation)
        {
            Assert.IsNotNull(continuation);
            Assert.IsFalse(IsCompleted);
            unitySynchronizationContext.Post(_ => continuation(), default);
        }

        // 主线程直接调continuation
        public bool IsCompleted => unitySynchronizationContext == SynchronizationContext.Current;

        public void GetResult()
        {
            Assert.IsTrue(IsCompleted);
        }

        public SwitchToMainThread GetAwaiter()
        {
            return this;
        }
    }
}
