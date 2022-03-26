using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace AillieoUtils.EasyAsync
{
    public class Promise : AbstractPromise
    {
        public void Resolve()
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

            if(dones != null)
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
        }

        public void Reject()
        {
            if(state != State.Pending)
            {
                throw new Exception("Reject when state is " + state);
            }

            state = State.Rejected;

            if(always != null)
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

            if (thens != null)
            {
                while (thens.Count > 0)
                {
                    thens.Dequeue().Invoke();
                }
                thens = null;
            }

            dones.Clear();
            dones = null;
        }

        public static Promise All(params Promise[] promises)
        {
            return All(promises as IEnumerable<Promise>);
        }

        public static Promise All(IEnumerable<Promise> promises)
        {
            Promise promise = new Promise();
            int count = promises.Count();
            if (count == 0)
            {
                promise.Resolve();
                return promise;
            }

            int rest = count;
            bool success = true;

            foreach(Promise p in promises)
            {
                p.Done(() =>
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
                            promise.Reject();
                        }
                    }
                })
                .Fail(() =>
                {
                    rest--;
                    success = false;
                    if (rest == 0)
                    {
                        promise.Reject();
                    }
                });
            }

            return promise;
        }

    }
}
