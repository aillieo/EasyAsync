using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using UnityEngine;

namespace AillieoUtils.EasyAsync.CoroutineExtensions
{
    public static class IEnumeratorExtensions
    {
        public static Awaiter GetAwaiter(this IEnumerator enumerator)
        {
            return new Awaiter(enumerator);
        }

        public class Awaiter : INotifyCompletion
        {
            public Awaiter(IEnumerator enumerator)
            {
                this.enumerator = enumerator;
                CoroutineRunner.Instance.StartCoroutine(new EnumWrapper(this));
            }

            internal IEnumerator enumerator;
            private Action continuation;
            private Exception exception;

            public bool IsCompleted { get; private set; }

            void INotifyCompletion.OnCompleted(Action continuation)
            {
                this.continuation = continuation;
            }

            internal void SetComplete(Exception exception = null)
            {
                this.exception = exception;
                IsCompleted = true;
                this.continuation?.Invoke();
            }

            public void GetResult()
            {
                if (exception != null)
                {
                    ExceptionDispatchInfo.Capture(exception).Throw();
                }
            }
        }

        internal class EnumWrapper : IEnumerator
        {
            private readonly Awaiter awaiter;
            private object current;

            public EnumWrapper(Awaiter awaiter)
            {
                this.awaiter = awaiter;
            }

            object IEnumerator.Current => current;

            bool IEnumerator.MoveNext()
            {
                bool result = awaiter.enumerator.MoveNext();
                current = awaiter.enumerator.Current;
                if (!result)
                {
                    awaiter.SetComplete();
                }

                return result;
            }

            void IEnumerator.Reset()
            {
                throw new NotImplementedException();
            }
        }
    }
}
