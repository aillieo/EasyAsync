using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using System;

namespace AillieoUtils.EasyAsync.Tests
{
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

        [Test]
        public static void TestThenNoValue()
        {
            Promise promise = new Promise();
        }
    }
}
