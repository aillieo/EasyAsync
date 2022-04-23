using System;
using System.Collections;

namespace AillieoUtils.EasyAsync.CoroutineExtensions
{
    public static class IEnumeratorExtensions
    {
        public static Awaiter GetAwaiter(this IEnumerator enumerator)
        {
            Awaiter awaiter = new Awaiter();
            CoroutineRunner.Instance.StartCoroutine(new EnumWrapper(awaiter, enumerator));
            return awaiter;
        }

        internal class EnumWrapper : IEnumerator
        {
            private readonly IEnumerator enumerator;
            private readonly Awaiter awaiter;
            private object current;

            public EnumWrapper(Awaiter awaiter, IEnumerator enumerator)
            {
                this.awaiter = awaiter;
                this.enumerator = enumerator;
            }

            object IEnumerator.Current => current;

            bool IEnumerator.MoveNext()
            {
                bool result = enumerator.MoveNext();
                current = enumerator.Current;
                if (!result)
                {
                    awaiter.Complete();
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
