using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AillieoUtils.EasyAsync
{
    public partial class Promise
    {
        private static readonly Promise resolved = NewResolved();
        private static readonly Promise rejected = NewRejected();

        public static Promise All(params Promise[] promises)
        {
            return All(promises as IEnumerable<Promise>);
        }

        // 全部成功 或者有一个失败
        public static Promise All(IEnumerable<Promise> promises)
        {
            int count = promises.Count();
            if (count == 0)
            {
                return Resolved();
            }

            Promise promise = new Promise();

            int rest = count;
            bool success = true;

            foreach (Promise p in promises)
            {
                p.OnFulfilled(() =>
                {
                    rest--;
                    if (rest == 0 && success)
                    {
                        promise.Resolve();
                    }
                })
                .OnRejected(rsn =>
                {
                    rest--;
                    if (success)
                    {
                        success = false;
                        promise.Reject(rsn);
                    }
                });
            }

            return promise;
        }

        public static Promise AllSettled(params Promise[] promises)
        {
            return AllSettled(promises as IEnumerable<Promise>);
        }

        // 全部成功或者失败
        public static Promise AllSettled(IEnumerable<Promise> promises)
        {
            int count = promises.Count();
            if (count == 0)
            {
                return Resolved();
            }

            Promise promise = new Promise();

            int rest = count;
            bool success = true;
            string reason = null;

            foreach (Promise p in promises)
            {
                p.OnFulfilled(() =>
                {
                    rest--;
                    if (rest == 0)
                    {
                        if (success)
                        {
                            promise.Resolve();
                        }
                        else
                        {
                            promise.Reject(reason);
                        }
                    }
                })
                .OnRejected(rsn =>
                {
                    rest--;
                    if (success)
                    {
                        success = false;
                        reason = rsn;
                    }

                    if (rest == 0)
                    {
                        promise.Reject(reason);
                    }
                });
            }

            return promise;
        }

        // 第一个成功
        public static Promise Any(params Promise[] promises)
        {
            return Any(promises as IEnumerable<Promise>);
        }

        public static Promise Any(IEnumerable<Promise> promises)
        {
            int count = promises.Count();
            if (count == 0)
            {
                return Resolved();
            }

            Promise promise = new Promise();

            int rest = count;
            bool success = false;
            string reason = null;

            foreach (Promise p in promises)
            {
                p.OnFulfilled(() =>
                {
                    rest--;
                    if (!success)
                    {
                        success = true;
                        promise.Resolve();
                    }
                })
                .OnRejected(rsn =>
                {
                    rest--;

                    if (reason == null)
                    {
                        reason = rsn;
                    }

                    if (rest == 0 && !success)
                    {
                        promise.Reject(rsn);
                    }
                });
            }

            return promise;
        }

        // 第一个成功或失败
        public static Promise Race(params Promise[] promises)
        {
            return Race(promises as IEnumerable<Promise>);
        }

        public static Promise Race(IEnumerable<Promise> promises)
        {
            int count = promises.Count();
            if (count == 0)
            {
                return Resolved();
            }

            Promise promise = new Promise();

            bool first = false;

            foreach (Promise p in promises)
            {
                p.OnFulfilled(() =>
                {
                    if (!first)
                    {
                        first = true;
                        promise.Resolve();
                    }
                })
                .OnRejected(rsn =>
                {
                    if (!first)
                    {
                        first = true;
                        promise.Reject(rsn);
                    }
                });
            }

            return promise;
        }

        public static Promise NewResolved()
        {
            Promise promise = new Promise();
            promise.Resolve();
            return promise;
        }

        public static Promise Resolved()
        {
            return resolved;
        }

        public static Promise Rejected(string reason)
        {
            Promise promise = new Promise();
            promise.Reject(reason);
            return promise;
        }

        public static Promise Rejected(Exception exception)
        {
            Promise promise = new Promise();
            promise.Reject(exception);
            return promise;
        }

        public static Promise NewRejected()
        {
            Promise promise = new Promise();
            promise.Reject("Default rejected promise");
            return promise;
        }

        public static Promise Rejected()
        {
            return rejected;
        }
    }

    public partial class Promise<T>
    {
        private static readonly Promise<T> rejected = NewRejected();

        public static Promise<T> Resolved(T value)
        {
            Promise<T> promise = new Promise<T>();
            promise.Resolve(value);
            return promise;
        }

        public static Promise<T> Rejected(string reason)
        {
            Promise<T> promise = new Promise<T>();
            promise.Reject(reason);
            return promise;
        }

        public static Promise<T> Rejected(Exception exception)
        {
            Promise<T> promise = new Promise<T>();
            promise.Reject(exception);
            return promise;
        }

        public static Promise<T> NewRejected()
        {
            Promise<T> promise = new Promise<T>();
            promise.Reject("Default rejected promise");
            return promise;
        }

        public static Promise<T> Rejected()
        {
            return rejected;
        }
    }
}
