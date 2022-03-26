using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AillieoUtils.EasyAsync
{
    public partial class Promise
    {
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
            string reason = null;

            foreach(Promise p in promises)
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
    }

    public partial class Promise<T>
    {
    }
}
