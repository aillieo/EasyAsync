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

        public class EnumWrapper : IEnumerator
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
    }
}
