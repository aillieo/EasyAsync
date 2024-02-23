// -----------------------------------------------------------------------
// <copyright file="PromiseExtensions.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.EasyAsync
{
    /// <summary>
    /// Provides extension methods for <see cref="Promise"/> and <see cref="Promise{T}"/> objects.
    /// </summary>
    public static class PromiseExtensions
    {
        /// <summary>
        /// Awaits the completion of the specified <see cref="Promise"/> object.
        /// </summary>
        /// <param name="promise">The <see cref="Promise"/> object to await.</param>
        public static async void Await(this Promise promise)
        {
            await promise;
        }

        /// <summary>
        /// Awaits the completion of the specified <see cref="Promise"/> object and ignores any exceptions that occur.
        /// </summary>
        /// <param name="promise">The <see cref="Promise"/> object to await and forget.</param>
        public static async void AwaitAndForget(this Promise promise)
        {
            try
            {
                await promise;
            }
            catch
            {
            }
        }

        /// <summary>
        /// Awaits the completion of the specified <see cref="Promise{T}"/> object.
        /// </summary>
        /// <typeparam name="T">The type of the value produced by the <see cref="Promise{T}"/> object.</typeparam>
        /// <param name="promise">The <see cref="Promise{T}"/> object to await.</param>
        public static async void Await<T>(this Promise<T> promise)
        {
            await promise;
        }

        /// <summary>
        /// Awaits the completion of the specified <see cref="Promise{T}"/> object and ignores any exceptions that occur.
        /// </summary>
        /// <typeparam name="T">The type of the value produced by the <see cref="Promise{T}"/> object.</typeparam>
        /// <param name="promise">The <see cref="Promise{T}"/> object to await and forget.</param>
        public static async void AwaitAndForget<T>(this Promise<T> promise)
        {
            try
            {
                await promise;
            }
            catch
            {
            }
        }
    }
}
