using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using System;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using AillieoUtils.EasyAsync.TaskExtensions;

namespace AillieoUtils.EasyAsync.Tests
{
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
