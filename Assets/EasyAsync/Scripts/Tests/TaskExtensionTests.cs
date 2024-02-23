// -----------------------------------------------------------------------
// <copyright file="TaskExtensionTests.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.EasyAsync.Tests
{
    using System;
    using System.Collections;
    using System.Threading.Tasks;
    using AillieoUtils.EasyAsync.TaskExtensions;
    using NUnit.Framework;
    using UnityEngine.TestTools;

    [Category("TaskExtensionTests")]
    public class TaskExtensionTests
    {
        [UnityTest]
        public IEnumerator TestDelay()
        {
            DateTime start = DateTime.Now;
            yield return Task.Delay(1000).AsCoroutine();
            DateTime end = DateTime.Now;
            Assert.GreaterOrEqual((end - start).TotalMilliseconds, 1000);
            yield return null;
        }
    }
}
