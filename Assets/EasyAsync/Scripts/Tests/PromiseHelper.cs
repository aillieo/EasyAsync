using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using System;
using System.Runtime.CompilerServices;

namespace AillieoUtils.EasyAsync.Tests
{
    public static class PromiseHelper
    {
        private static IEnumerator DelayResolve(Promise promise, float delay)
        {
            yield return new WaitForSeconds(delay);
            promise.Resolve();
        }

        private static IEnumerator DelayResolve<T>(Promise<T> promise, T value, float delay)
        {
            yield return new WaitForSeconds(delay);
            promise.Resolve(value);
        }

        public static Promise SimpleDelayNoValue(float delay)
        {
            Promise promise = new Promise();
            CoroutineRunner runner = new GameObject("runner").AddComponent<CoroutineRunner>();
            runner.StartCoroutine(DelayResolve(promise, delay));
            return promise;
        }

        public static Promise<T> SimpleDelay<T>(float delay, T value)
        {
            Promise<T> promise = new Promise<T>();
            CoroutineRunner runner = new GameObject("runner").AddComponent<CoroutineRunner>();
            runner.StartCoroutine(DelayResolve(promise, value, delay));
            return promise;
        }
    }
}
