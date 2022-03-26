using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

namespace AillieoUtils.EasyAsync
{
    public class Promise<T> : AbstractPromise
    {
        private T cachedArg;

        public Promise<T> Done(Action<T> func)
        {
            if (state == State.Pending)
            {
                this.dones = this.dones ?? new Queue<Action>();
                this.dones.Enqueue(() => func(cachedArg));
            }
            else if (state == State.Resolved)
            {
                func(cachedArg);
            }
            return this;
        }

        public Promise<T> Fail(Action<T> func)
        {
            if (state == State.Pending)
            {
                this.fails = this.fails ?? new Queue<Action>();
                this.fails.Enqueue(() => func(cachedArg));
            }
            else if (state == State.Rejected)
            {
                func(cachedArg);
            }
            return this;
        }

        public Promise<T> Always(Action<T> func)
        {
            if(state == State.Pending)
            {
                this.always = this.always ?? new Queue<Action>();
                this.always.Enqueue(() => func(cachedArg));
            }
            else
            {
                func(cachedArg);
            }
            return this;
        }

        public Promise<T> Then(Func<Promise<T>> func)
        {
            Promise<T> newPromise = new Promise<T>();
            if (state == State.Pending)
            {
                this.thens = this.thens ?? new Queue<Action>();
                this.thens.Enqueue(() => func()?
                    .Done(value => newPromise.Resolve(value))
                    .Fail(value => newPromise.Reject(value)));
            }
            else
            {
                return func();
            }
            return newPromise;
        }

        public void Resolve(T arg)
        {
            if (state != State.Pending)
            {
                throw new Exception("Resolve when state is " + state);
            }

            state = State.Resolved;

            if(always != null)
            {
                while (always.Count > 0)
                {
                    always.Dequeue().Invoke();
                }
                always = null;
            }

            if (dones != null)
            {
                while (dones.Count > 0)
                {
                    dones.Dequeue().Invoke();
                }
                dones = null;
            }

            if(thens != null)
            {
                while (thens.Count > 0)
                {
                    thens.Dequeue().Invoke();
                }
                thens = null;
            }

            fails.Clear();
            fails = null;

            cachedArg = arg;
        }

        public void Reject(T arg)
        {
            if(state != State.Pending)
            {
                throw new Exception("Reject when state is " + state);
            }

            state = State.Rejected;

            if (always != null)
            {
                while (always.Count > 0)
                {
                    always.Dequeue().Invoke();
                }
                always = null;
            }

            if(fails != null)
            {
                while (fails.Count > 0)
                {
                    fails.Dequeue().Invoke();
                }
                fails = null;
            }

            if(thens != null)
            {
                while (thens.Count > 0)
                {
                    thens.Dequeue().Invoke();
                }
                thens = null;
            }

            dones.Clear();
            dones = null;

            cachedArg = arg;

        }

        public static Promise<IEnumerable<T>> All(params Promise<T>[] promises)
        {
            return All(promises as IEnumerable<Promise<T>>);
        }

        public static Promise<IEnumerable<T>> All(IEnumerable<Promise<T>> promises)
        {
            Promise<IEnumerable<T>> promise = new Promise<IEnumerable<T>>();
            int count = promises.Count();
            if (count == 0)
            {
                promise.Resolve(null);
                return promise;
            }

            List<T> args = new List<T>(count);

            int rest = count;
            bool success = true;

            int index = 0;
            foreach (Promise<T> p in promises)
            {
                p.Done((arg) =>
                {
                    args[index] = arg;
                    rest--;
                    if (rest == 0)
                    {
                        if (success)
                        {
                            promise.Resolve(args);
                        }
                        else
                        {
                            promise.Reject(args);
                        }
                    }
                })
                .Fail((arg) =>
                {
                    args[index] = arg;
                    rest--;
                    success = false;
                    if (rest == 0)
                    {
                        promise.Reject(args);
                    }
                });
                index++;
            }

            return promise;
        }
    }
}
