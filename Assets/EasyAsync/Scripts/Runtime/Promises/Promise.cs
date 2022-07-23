using System;
using System.Runtime.CompilerServices;

namespace AillieoUtils.EasyAsync
{
    [AsyncMethodBuilder(typeof(EasyAsyncTaskMethodBuilder))]
    public sealed partial class Promise : AbstractPromise
    {
        [Flags]
        public enum Status : byte
        {
            Pending = 0b00,
            Fulfilled = 0b01,
            Rejected = 0b10,
        }

        public void Resolve()
        {
            status = Status.Fulfilled;
            ProcessCallbacks();
        }

        public Awaiter GetAwaiter()
        {
            return new Awaiter(this);
        }
    }
}
