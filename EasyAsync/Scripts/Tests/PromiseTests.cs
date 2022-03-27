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
            Promise promise = new Promise();
            promise.OnFulfilled(() => i++);
            Assert.AreEqual(i, 0);
            promise.Resolve();
            Assert.AreEqual(i, 1);
            Assert.Catch(typeof(Exception), () => promise.Resolve());
        }

        [Test]
        public static void TestSimpleReject()
        {
            string reason = "TestReject";
            int i = 0;
            Promise promise = new Promise();
            promise.OnRejected(() => i++);
            Assert.AreEqual(i, 0);
            promise.Reject(reason);
            Assert.AreEqual(i, 1);
            Assert.AreEqual(promise.reason, reason);
            Assert.Catch(typeof(Exception), () => promise.Reject(reason));
        }
    }
}
