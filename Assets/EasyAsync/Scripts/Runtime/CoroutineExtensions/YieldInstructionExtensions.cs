using System;
using System.Collections;
using UnityEngine;

namespace AillieoUtils.EasyAsync.CoroutineExtensions
{
    public static class YieldInstructionExtensions
    {
        public static Awaiter GetAwaiter(this YieldInstruction yieldInstruction)
        {
            Promise promise = new Promise();
            CoroutineRunner.Instance.StartCoroutine(new EnumWrapper(promise, yieldInstruction));
            return promise.GetAwaiter();
        }

        internal class EnumWrapper : IEnumerator
        {
            private readonly Promise promise;
            private readonly YieldInstruction yieldInstruction;
            private object current;

            public EnumWrapper(Promise promise, YieldInstruction yieldInstruction)
            {
                this.promise = promise;
                this.yieldInstruction = yieldInstruction;
            }

            object IEnumerator.Current => current;

            bool IEnumerator.MoveNext()
            {
                if (current == null)
                {
                    current = yieldInstruction;
                    return true;
                }

                promise.Resolve();
                return false;
            }

            void IEnumerator.Reset()
            {
                throw new NotImplementedException();
            }
        }
    }
}
