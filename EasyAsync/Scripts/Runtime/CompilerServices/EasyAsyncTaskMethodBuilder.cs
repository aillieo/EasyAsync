using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

namespace AillieoUtils.EasyAsync
{
    public struct EasyAsyncTaskMethodBuilder
    {
        private Promise promise;

        public Promise Task
        {
            get
            {
                if (promise == null)
                {
                    promise = new Promise();
                }

                return promise;
            }
        }

        public static EasyAsyncTaskMethodBuilder Create()
        {
            return default;
        }

        public void Start<TStateMachine>(ref TStateMachine stateMachine)
            where TStateMachine : IAsyncStateMachine
        {
            stateMachine.MoveNext();
        }

        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {
        }

        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            awaiter.OnCompleted(stateMachine.MoveNext);
        }

        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            awaiter.UnsafeOnCompleted(stateMachine.MoveNext);
        }

        public void SetResult()
        {
            Task.Resolve();
        }

        public void SetException(Exception exception)
        {
            Task.Reject(exception);
        }
    }
}
