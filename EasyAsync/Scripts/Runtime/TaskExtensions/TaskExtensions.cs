using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace AillieoUtils.EasyAsync.TaskExtensions
{
    public static class TaskExtensions
    {
        public static EnumWrapper AsCoroutine(this Task task)
        {
            return new EnumWrapper(task);
        }

        public struct EnumWrapper : IEnumerator
        {
            private readonly Task task;

            public EnumWrapper(Task task)
            {
                this.task = task;
            }

            object IEnumerator.Current => null;

            bool IEnumerator.MoveNext()
            {
                return !task.IsCompleted;
            }

            void IEnumerator.Reset()
            {
                throw new System.NotImplementedException();
            }
        }

        public static async void AwaitAndCheck(this Task task)
        {
            await task;
        }

        public static async void AwaitAndForget(this Task task)
        {
            try
            {
                await task;
            }
            catch
            {
            }
        }
    }
}
