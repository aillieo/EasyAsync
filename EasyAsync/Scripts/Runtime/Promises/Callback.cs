using System;

namespace AillieoUtils.EasyAsync
{
    internal struct Callback
    {
        public readonly Action action;
        public readonly Promise.State flag;

        public Callback(Action action, Promise.State flag)
        {
            this.action = action;
            this.flag = flag;
        }
    }
}
