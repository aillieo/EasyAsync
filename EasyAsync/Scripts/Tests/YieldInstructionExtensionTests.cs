// -----------------------------------------------------------------------
// <copyright file="YieldInstructionExtensionTests.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.EasyAsync.Tests
{
    using System;
    using System.Collections;
    using System.Threading.Tasks;
    using AillieoUtils.EasyAsync.CoroutineExtensions;
    using NUnit.Framework;
    using UnityEngine;
    using UnityEngine.TestTools;

    [Category("YieldInstructionExtensionTests")]
    public class YieldInstructionExtensionTests
    {
        [UnityTest]
        public IEnumerator TestWaitForSeconds()
        {
            var task = this.TestWaitForSecondsImpl();
            yield return new WaitUntil(() => task.IsCompleted);
        }

        private async Task TestWaitForSecondsImpl()
        {
            DateTime start = DateTime.Now;
            await new WaitForSeconds(1);
            DateTime end = DateTime.Now;
            Assert.GreaterOrEqual((end - start).TotalMilliseconds, 1000);
        }

        [UnityTest]
        public IEnumerator TestCustomCoroutine()
        {
            var task = this.TestCustomCoroutineImpl();
            yield return new WaitUntil(() => task.IsCompleted);
        }

        private async Task TestCustomCoroutineImpl()
        {
            DateTime start = DateTime.Now;

            await this.CustomCoroutine();

            Assert.GreaterOrEqual((DateTime.Now - start).TotalMilliseconds, 1000);

            start = DateTime.Now;

            CoroutineRunner runner = new GameObject("runner").AddComponent<CoroutineRunner>();
            await runner.StartCoroutine(this.CustomCoroutine());

            Assert.GreaterOrEqual((DateTime.Now - start).TotalMilliseconds, 1000);

            GameObject.Destroy(runner.gameObject);
        }

        private IEnumerator CustomCoroutine()
        {
            yield return new WaitForSeconds(2);
        }
    }
}
