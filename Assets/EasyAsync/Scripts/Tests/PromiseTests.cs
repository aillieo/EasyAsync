// -----------------------------------------------------------------------
// <copyright file="PromiseTests.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.EasyAsync.Tests
{
    using System;
    using System.Collections;
    using NUnit.Framework;
    using UnityEngine;
    using UnityEngine.TestTools;

    [Category("Promise")]
    public static class PromiseTests
    {
        [Test]
        public static void TestSimpleResolve()
        {
            int i = 0;
            int j = 0;
            Promise promise = new Promise();
            promise.OnFulfilled(() => i++);
            promise.OnRejected(() => j++);
            Assert.AreEqual(i, 0);
            promise.Resolve();
            Assert.AreEqual(i, 1);
            Assert.AreEqual(j, 0);
            Assert.Catch(typeof(Exception), () => promise.Resolve());
        }

        [Test]
        public static void TestSimpleReject()
        {
            string reason = "TestReject";
            int i = 0;
            int j = 0;
            Promise promise = new Promise();
            promise.OnRejected(() => i++);
            Assert.AreEqual(i, 0);
            promise.Reject(reason);
            Assert.AreEqual(i, 1);
            Assert.AreEqual(j, 0);
            Assert.AreEqual(promise.reason, reason);
            Assert.Catch(typeof(Exception), () => promise.Reject(reason));
        }

        [UnityTest]
        public static IEnumerator TestThenNoValue1()
        {
            bool finished = false;
            Promise promise = PromiseHelper.SimpleDelayNoValue(0.1f);
            promise.Then(() => finished = true, (Action)null);
            yield return new WaitUntil(() => finished);
        }

        [UnityTest]
        public static IEnumerator TestThenNoValue2()
        {
            bool finished = false;
            Promise<int> promise = PromiseHelper.SimpleDelay<int>(0.1f, 100);
            promise.Then(() => finished = true, (Action)null);
            yield return new WaitUntil(() => finished);
        }
    }
}
