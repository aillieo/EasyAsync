// -----------------------------------------------------------------------
// <copyright file="TaskExtensions.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.EasyAsync.TaskExtensions
{
    using System.Collections;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides extension methods for <see cref="Task"/> objects.
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        /// Converts the specified <see cref="Task"/> object to an <see cref="EnumWrapper"/> object.
        /// </summary>
        /// <param name="task">The <see cref="Task"/> object.</param>
        /// <returns>An <see cref="IEnumerator"/> object representing the specified <see cref="Task"/> object.</returns>
        public static IEnumerator AsCoroutine(this Task task)
        {
            return new EnumWrapper(task);
        }

        /// <summary>
        /// Awaits the completion of the specified <see cref="Task"/> object.
        /// </summary>
        /// <param name="task">The <see cref="Task"/> object to await.</param>
        public static async void Await(this Task task)
        {
            await task;
        }

        /// <summary>
        /// Awaits the completion of the specified <see cref="Task"/> object and ignores any exceptions that occur.
        /// </summary>
        /// <param name="task">The <see cref="Task"/> object to await and forget.</param>
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

        /// <summary>
        /// Represents a wrapper for a <see cref="Task"/> object.
        /// </summary>
        internal readonly struct EnumWrapper : IEnumerator
        {
            private readonly Task task;

            /// <summary>
            /// Initializes a new instance of the <see cref="EnumWrapper"/> struct with the specified <see cref="Task"/> object.
            /// </summary>
            /// <param name="task">The <see cref="Task"/> object.</param>
            internal EnumWrapper(Task task)
            {
                this.task = task;
            }

            /// <inheritdoc/>
            object IEnumerator.Current => null;

            /// <inheritdoc/>
            bool IEnumerator.MoveNext()
            {
                return !this.task.IsCompleted;
            }

            /// <inheritdoc/>
            void IEnumerator.Reset()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
