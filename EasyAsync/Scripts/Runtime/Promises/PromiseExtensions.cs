using System;
using System.Runtime.CompilerServices;

namespace AillieoUtils.EasyAsync
{
    public static class PromiseExtensions
    {
        public static async void AwaitAndCheck(this Promise promise)
        {
            await promise;
        }

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

        public static async void AwaitAndCheck<T>(this Promise<T> promise)
        {
            await promise;
        }

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
