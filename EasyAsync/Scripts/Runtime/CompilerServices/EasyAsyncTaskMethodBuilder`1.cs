using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

namespace AillieoUtils.EasyAsync
{
    public struct EasyAsyncTaskMethodBuilder<T>
    {
        private Promise<T> promise;

        public Promise<T> Task
        {
            get
            {
                if (promise == null)
                {
                    promise = new Promise<T>();
                }

                return promise;
            }
        }

        public static EasyAsyncTaskMethodBuilder<T> Create()
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

        public void SetResult(T result)
        {
            Task.Resolve(result);
        }

        public void SetException(Exception exception)
        {
            Task.Reject(exception);
        }
    }
}
