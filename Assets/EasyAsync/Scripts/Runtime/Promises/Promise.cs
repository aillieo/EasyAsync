using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace AillieoUtils.EasyAsync
{
    public sealed partial class Promise : AbstractPromise
    {
        public void Resolve()
        {
            Assert.AreEqual(state, State.Pending);

            state = State.Fulfilled;

            if(callbacks != null)
            {
                while (callbacks.Count > 0)
                {
                    Callback callback = callbacks.Dequeue();
                    if ((callback.flag & state) != 0)
                    {
                        callback.action?.Invoke();
                    }
                }
                callbacks = null;
            }
        }

        public void Reject(string reason)
        {
            Assert.AreEqual(state, State.Pending);

            this.reason = reason;
            state = State.Rejected;

            if(callbacks != null)
            {
                while (callbacks.Count > 0)
                {
                    Callback callback = callbacks.Dequeue();
                    if ((callback.flag & state) != 0)
                    {
                        callback.action?.Invoke();
                    }
                }

                callbacks = null;
            }
        }
    }
}
