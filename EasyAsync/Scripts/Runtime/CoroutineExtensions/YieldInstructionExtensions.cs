using System;
using System.Collections;
using UnityEngine;

namespace AillieoUtils.EasyAsync.CoroutineExtensions
{
    public static class YieldInstructionExtensions
    {
        public static Awaiter GetAwaiter(this YieldInstruction yieldInstruction)
        {
            Awaiter awaiter = new Awaiter();
            CoroutineRunner.Instance.StartCoroutine(new EnumWrapper(awaiter, yieldInstruction));
            return awaiter;
        }

        internal class EnumWrapper : IEnumerator
        {
            private readonly Awaiter awaiter;
            private readonly YieldInstruction yieldInstruction;
            private object current;

            public EnumWrapper(Awaiter awaiter, YieldInstruction yieldInstruction)
            {
                this.awaiter = awaiter;
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

                awaiter.Complete();
                return false;
            }

            void IEnumerator.Reset()
            {
                throw new NotImplementedException();
            }
        }
    }
}
