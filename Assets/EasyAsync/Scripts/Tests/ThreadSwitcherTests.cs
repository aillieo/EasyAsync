// -----------------------------------------------------------------------
// <copyright file="ThreadSwitcherTests.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.EasyAsync.Tests
{
    using System.Collections;
    using System.Threading;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using UnityEngine;
    using UnityEngine.TestTools;

    [Category("ThreadSwitchers")]
    public class ThreadSwitcherTests
    {
        [UnityTest]
        public IEnumerator RunTestSwitch()
        {
            var task = this.TestSwitch();
            yield return new WaitUntil(() => task.IsCompleted);
        }

        public async Task<int> TestSwitch()
        {
            int mainThread = Thread.CurrentThread.ManagedThreadId;
            SynchronizationContext unity = SynchronizationContext.Current;

            Assert.AreEqual(mainThread, Thread.CurrentThread.ManagedThreadId);
            Assert.AreEqual(unity, SynchronizationContext.Current);

            await new SwitchToPooledThread();

            int thread1 = Thread.CurrentThread.ManagedThreadId;
            SynchronizationContext context1 = SynchronizationContext.Current;

            Assert.AreNotEqual(mainThread, Thread.CurrentThread.ManagedThreadId);
            Assert.AreNotEqual(unity, SynchronizationContext.Current);

            await new SwitchToMainThread();

            Assert.AreEqual(mainThread, Thread.CurrentThread.ManagedThreadId);
            Assert.AreEqual(unity, SynchronizationContext.Current);

            await new SwitchToContext(context1);

            Assert.AreEqual(thread1, Thread.CurrentThread.ManagedThreadId);
            Assert.AreEqual(context1, SynchronizationContext.Current);

            await Task.Delay(1);

            return 0;
        }
    }
}
