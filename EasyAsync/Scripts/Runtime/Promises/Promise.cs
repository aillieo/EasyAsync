using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AillieoUtils.EasyAsync
{
    public sealed partial class Promise : AbstractPromise
    {
        public Promise(Action onFulfilled = null, Action<string> onRejected = null)
        {
            if (onFulfilled != null)
            {
                OnFulfilled(onFulfilled);
            }

            if (onRejected != null)
            {
                OnRejected(onRejected);
            }
        }

        public void Resolve()
        {
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
    }
}
