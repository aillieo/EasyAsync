using System;
using System.Runtime.CompilerServices;

namespace AillieoUtils.EasyAsync
{
    [AsyncMethodBuilder(typeof(EasyAsyncTaskMethodBuilder))]
    public sealed partial class Promise : AbstractPromise
    {
        [Flags]
        public enum State : byte
        {
            Pending = 0b00,
            Fulfilled = 0b01,
            Rejected = 0b10,
        }

        public Promise(Action onFulfilled = null, Action<string> onRejected = null)
        {
            if (onFulfilled != null)
            {
                OnFulfilled(onFulfilled);
            }

            if (onRejected != null)
            {
                OnRejected(onRejected);
            }
        }

        public void Resolve()
        {
            state = State.Fulfilled;
            ProcessCallbacks();
        }

        public Awaiter GetAwaiter()
        {
            throw new Exception();
        }
    }
}
