using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AillieoUtils.EasyAsync
{
    public struct EasyAsyncTaskMethodBuilder
    {
        private Promise promise;

        [DebuggerHidden]
        public Promise Task
        {
            get
            {
                return promise;
            }
        }

        [DebuggerHidden]
        public static EasyAsyncTaskMethodBuilder Create()
        {
            EasyAsyncTaskMethodBuilder builder = default;
            builder.promise = new Promise();
            return builder;
        }

        [DebuggerHidden]
        public void Start<TStateMachine>(ref TStateMachine stateMachine)
            where TStateMachine : IAsyncStateMachine
        {
            stateMachine.MoveNext();
        }

        [DebuggerHidden]
        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {
        }

        [DebuggerHidden]
        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            awaiter.OnCompleted(stateMachine.MoveNext);
        }

        [DebuggerHidden]
        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            awaiter.UnsafeOnCompleted(stateMachine.MoveNext);
        }

        [DebuggerHidden]
        public void SetResult()
        {

            Task.Resolve();
        }

        [DebuggerHidden]
        public void SetException(Exception exception)
        {
            Task.Reject(exception);
        }
    }
}
