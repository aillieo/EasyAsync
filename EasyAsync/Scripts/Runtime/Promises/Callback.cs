using System;

namespace AillieoUtils.EasyAsync
{
    internal struct Callback
    {
        public readonly Action action;
        public readonly Promise.Status mask;

        public Callback(Promise.Status mask, Action action)
        {
            this.mask = mask;
            this.action = action;
        }
    }
}
