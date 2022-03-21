using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using UnityEngine;

namespace AillieoUtils.EasyAsync.CoroutineExtensions
{
    public static class YieldInstructionExtensions
    {
        public static Awaiter GetAwaiter(this YieldInstruction yieldInstruction)
        {
            return new Awaiter(yieldInstruction);
        }

        public class Awaiter : INotifyCompletion
        {
            public Awaiter(YieldInstruction yieldInstruction)
            {
                this.yieldInstruction = yieldInstruction;
                CoroutineRunner.Instance.StartCoroutine(new EnumWrapper(this));
            }

            internal YieldInstruction yieldInstruction;
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
                if (current == null)
                {
                    current = awaiter.yieldInstruction;
                    return true;
                }

                awaiter.SetComplete();
                return false;
            }

            void IEnumerator.Reset()
            {
                throw new NotImplementedException();
            }
        }
    }
}
