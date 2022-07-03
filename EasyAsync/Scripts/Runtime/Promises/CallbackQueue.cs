using System;
using System.Collections;
using System.Collections.Generic;

namespace AillieoUtils.EasyAsync
{
    internal static class CallbackQueue
    {
        private const int capacity = 256;
        private static readonly Stack<Queue<Callback>> pool = new Stack<Queue<Callback>>();
        private static object locker = new object();

        public static Queue<Callback> Get()
        {
            lock (locker)
            {
                if (pool.Count > 0)
                {
                    return pool.Pop();
                }
            }

            return new Queue<Callback>();
        }

        public static void Recycle(Queue<Callback> queue)
        {
            lock (locker)
            {
                if (pool.Count < capacity)
                {
                    pool.Push(queue);
                }
            }
        }
    }
}
