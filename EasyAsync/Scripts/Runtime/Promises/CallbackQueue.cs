// -----------------------------------------------------------------------
// <copyright file="CallbackQueue.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.EasyAsync
{
    using System.Collections.Generic;

    internal static class CallbackQueue
    {
        private const int capacity = 256;
        private static readonly Stack<Queue<Callback>> pool = new Stack<Queue<Callback>>();
        private static readonly object syncRoot = new object();

        public static Queue<Callback> Get()
        {
            lock (syncRoot)
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
            lock (syncRoot)
            {
                if (pool.Count < capacity)
                {
                    pool.Push(queue);
                }
            }
        }
    }
}
