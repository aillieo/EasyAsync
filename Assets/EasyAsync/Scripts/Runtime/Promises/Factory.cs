// -----------------------------------------------------------------------
// <copyright file="Factory.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.EasyAsync
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a promise that can be fulfilled or rejected.
    /// </summary>
    public partial class Promise
    {
        private static readonly Promise resolved = NewResolved();

        private static readonly Promise rejected = NewRejected();

        /// <summary>
        /// Returns a promise that resolves when all of the specified promises have resolved,
        /// or rejects if any of the promises reject.
        /// </summary>
        /// <param name="promises">The promises to wait for.</param>
        /// <returns>A promise that resolves when all of the specified promises have resolved,
        /// or rejects if any of the promises reject.</returns>
        public static Promise All(params AbstractPromise[] promises)
        {
            return All(promises as IEnumerable<AbstractPromise>);
        }

        /// <summary>
        /// Returns a promise that resolves when all of the specified promises have resolved,
        /// or rejects if any of the promises reject.
        /// </summary>
        /// <param name="promises">The promises to wait for.</param>
        /// <returns>A promise that resolves when all of the specified promises have resolved,
        /// or rejects if any of the promises reject.</returns>
        public static Promise All(IEnumerable<AbstractPromise> promises)
        {
            int count = promises.Count();
            if (count == 0)
            {
                return Resolved();
            }

            Promise promise = new Promise();

            int rest = count;
            bool success = true;

            foreach (AbstractPromise p in promises)
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

        /// <summary>
        /// Returns a promise that resolves or rejects when all of the specified promises have settled
        /// (either resolved or rejected).
        /// </summary>
        /// <param name="promises">The promises to wait for.</param>
        /// <returns>A promise that resolves or rejects when all of the specified promises have settled
        /// (either resolved or rejected).</returns>
        public static Promise AllSettled(params AbstractPromise[] promises)
        {
            return AllSettled(promises as IEnumerable<AbstractPromise>);
        }

        /// <summary>
        /// Returns a promise that resolves or rejects when all of the specified promises have settled
        /// (either resolved or rejected).
        /// </summary>
        /// <param name="promises">The promises to wait for.</param>
        /// <returns>A promise that resolves or rejects when all of the specified promises have settled
        /// (either resolved or rejected).</returns>
        public static Promise AllSettled(IEnumerable<AbstractPromise> promises)
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

            foreach (AbstractPromise p in promises)
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

        /// <summary>
        /// Returns a promise that resolves when any of the specified promises resolves.
        /// </summary>
        /// <param name="promises">The promises to wait for.</param>
        /// <returns>A promise that resolves when any of the specified promises resolves.</returns>
        public static Promise Any(params AbstractPromise[] promises)
        {
            return Any(promises as IEnumerable<AbstractPromise>);
        }

        /// <summary>
        /// Returns a promise that resolves when any of the specified promises resolves.
        /// </summary>
        /// <param name="promises">The promises to wait for.</param>
        /// <returns>A promise that resolves when any of the specified promises resolves.</returns>
        public static Promise Any(IEnumerable<AbstractPromise> promises)
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

            foreach (AbstractPromise p in promises)
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

        /// <summary>
        /// Returns a promise that resolves or rejects when any of the specified promises resolves or rejects.
        /// </summary>
        /// <param name="promises">The promises to wait for.</param>
        /// <returns>A promise that resolves or rejects when any of the specified promises resolves or rejects.</returns>
        public static Promise Race(params AbstractPromise[] promises)
        {
            return Race(promises as IEnumerable<AbstractPromise>);
        }

        /// <summary>
        /// Returns a promise that resolves or rejects when any of the specified promises resolves or rejects.
        /// </summary>
        /// <param name="promises">The promises to wait for.</param>
        /// <returns>A promise that resolves or rejects when any of the specified promises resolves or rejects.</returns>
        public static Promise Race(IEnumerable<AbstractPromise> promises)
        {
            int count = promises.Count();
            if (count == 0)
            {
                return Resolved();
            }

            Promise promise = new Promise();

            bool first = false;

            foreach (AbstractPromise p in promises)
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

        /// <summary>
        /// Creates a new resolved promise.
        /// </summary>
        /// <returns>A new resolved promise.</returns>
        public static Promise NewResolved()
        {
            Promise promise = new Promise();
            promise.Resolve();
            return promise;
        }

        /// <summary>
        /// Returns a resolved promise.
        /// </summary>
        /// <returns>A resolved promise.</returns>
        public static Promise Resolved()
        {
            return resolved;
        }

        /// <summary>
        /// Returns a rejected promise with the specified reason.
        /// </summary>
        /// <param name="reason">The reason for rejecting the promise.</param>
        /// <returns>A rejected promise with the specified reason.</returns>
        public static Promise Rejected(string reason)
        {
            Promise promise = new Promise();
            promise.Reject(reason);
            return promise;
        }

        /// <summary>
        /// Returns a rejected promise with the specified exception.
        /// </summary>
        /// <param name="exception">The exception for rejecting the promise.</param>
        /// <returns>A rejected promise with the specified exception.</returns>
        public static Promise Rejected(Exception exception)
        {
            Promise promise = new Promise();
            promise.Reject(exception);
            return promise;
        }

        /// <summary>
        /// Creates a new rejected promise.
        /// </summary>
        /// <returns>A new rejected promise.</returns>
        public static Promise NewRejected()
        {
            Promise promise = new Promise();
            promise.Reject("Default rejected promise");
            return promise;
        }

        /// <summary>
        /// Returns a rejected promise.
        /// </summary>
        /// <returns>A rejected promise.</returns>
        public static Promise Rejected()
        {
            return rejected;
        }
    }

    /// <summary>
    /// Represents a promise that can be fulfilled or rejected with a value of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the value produced by the promise.</typeparam>
    public partial class Promise<T>
    {
        /// <summary>
        /// A static readonly Promise object that represents a rejected promise.
        /// </summary>
        private static readonly Promise<T> rejected = NewRejected();

        /// <summary>
        /// Creates a new resolved promise with the specified value.
        /// </summary>
        /// <param name="value">The value for resolving the promise.</param>
        /// <returns>A new resolved promise with the specified value.</returns>
        public static Promise<T> Resolved(T value)
        {
            Promise<T> promise = new Promise<T>();
            promise.Resolve(value);
            return promise;
        }

        /// <summary>
        /// Returns a rejected promise with the specified reason.
        /// </summary>
        /// <param name="reason">The reason for rejecting the promise.</param>
        /// <returns>A rejected promise with the specified reason.</returns>
        public static Promise<T> Rejected(string reason)
        {
            Promise<T> promise = new Promise<T>();
            promise.Reject(reason);
            return promise;
        }

        /// <summary>
        /// Returns a rejected promise with the specified exception.
        /// </summary>
        /// <param name="exception">The exception for rejecting the promise.</param>
        /// <returns>A rejected promise with the specified exception.</returns>
        public static Promise<T> Rejected(Exception exception)
        {
            Promise<T> promise = new Promise<T>();
            promise.Reject(exception);
            return promise;
        }

        /// <summary>
        /// Creates a new rejected promise.
        /// </summary>
        /// <returns>A new rejected promise.</returns>
        public static Promise<T> NewRejected()
        {
            Promise<T> promise = new Promise<T>();
            promise.Reject("Default rejected promise");
            return promise;
        }

        /// <summary>
        /// Returns a rejected promise.
        /// </summary>
        /// <returns>A rejected promise.</returns>
        public static Promise<T> Rejected()
        {
            return rejected;
        }
    }
}
