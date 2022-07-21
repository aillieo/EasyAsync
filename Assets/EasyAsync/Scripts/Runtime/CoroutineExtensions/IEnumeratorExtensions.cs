using System;
using System.Collections;

namespace AillieoUtils.EasyAsync.CoroutineExtensions
{
    public static class IEnumeratorExtensions
    {
        public static Awaiter GetAwaiter(this IEnumerator enumerator)
        {
            Promise promise = new Promise();
            CoroutineRunner.Instance.StartCoroutine(new EnumWrapper(promise, enumerator));
            return promise.GetAwaiter();
        }

        internal struct EnumWrapper : IEnumerator
        {
            private readonly Promise promise;
            private readonly IEnumerator enumerator;

            public EnumWrapper(Promise promise, IEnumerator enumerator)
            {
                this.promise = promise;
                this.enumerator = enumerator;
            }

            object IEnumerator.Current => enumerator.Current;

            bool IEnumerator.MoveNext()
            {
                bool result = enumerator.MoveNext();
                if (!result)
                {
                    promise.Resolve();
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
