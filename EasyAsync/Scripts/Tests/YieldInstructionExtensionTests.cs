using System.Collections;
using NUnit.Framework;
using UnityEngine;
using System;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using AillieoUtils.EasyAsync.CoroutineExtensions;

namespace AillieoUtils.EasyAsync.Tests
{
    [Category("YieldInstructionExtensionTests")]
    public class YieldInstructionExtensionTests
    {
        [UnityTest]
        public IEnumerator TestWaitForSeconds()
        {
            var task = TestWaitForSecondsImpl();
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
            var task = TestCustomCoroutineImpl();
            yield return new WaitUntil(() => task.IsCompleted);
        }

        private async Task TestCustomCoroutineImpl()
        {
            DateTime start = DateTime.Now;

            await CustomCoroutine();

            Assert.GreaterOrEqual((DateTime.Now - start).TotalMilliseconds, 1000);

            start = DateTime.Now;

            CoroutineRunner runner = new GameObject("runner").AddComponent<CoroutineRunner>();
            await runner.StartCoroutine(CustomCoroutine());

            Assert.GreaterOrEqual((DateTime.Now - start).TotalMilliseconds, 1000);

            GameObject.Destroy(runner.gameObject);
        }

        private IEnumerator CustomCoroutine()
        {
            yield return new WaitForSeconds(2);
        }
    }
}
