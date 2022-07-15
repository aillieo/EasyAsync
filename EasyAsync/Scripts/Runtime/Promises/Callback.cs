using System;

namespace AillieoUtils.EasyAsync
{
    internal struct Callback
    {
        public readonly Action action;
        public readonly Promise.State mask;

        public Callback(Action action, Promise.State mask)
        {
            this.action = action;
            this.mask = mask;
        }
    }
}
