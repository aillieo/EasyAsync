using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace AillieoUtils.EasyAsync
{
    public abstract class AbstractPromise
    {
        protected internal enum State
        {
            Pending = 0,
            Rejected = 1,
            Resolved = 2,
        }

        protected State state = State.Pending;
        protected Queue<Action> always;
        protected Queue<Action> dones;
        protected Queue<Action> fails;
        protected Queue<Action> thens;

        public AbstractPromise Done(Action func)
        {
            if (state == State.Pending)
            {
                this.dones = this.dones ?? new Queue<Action>();
                this.dones.Enqueue(func);
            }
            else if (state == State.Resolved)
            {
                func();
            }
            return this;
        }

        public AbstractPromise Fail(Action func)
        {
            if (state == State.Pending)
            {
                this.fails = this.fails ?? new Queue<Action>();
                this.fails.Enqueue(func);
            }
            else if (state == State.Rejected)
            {
                func();
            }
            return this;
        }

        public AbstractPromise Always(Action func)
        {
            if(state == State.Pending)
            {
                this.always = this.always ?? new Queue<Action>();
                this.always.Enqueue(func);
            }
            else
            {
                func();
            }
            return this;
        }

        public AbstractPromise Then(Func<Promise> func)
        {
            Promise newPromise = new Promise();
            if (state == State.Pending)
            {
                this.thens = this.thens ?? new Queue<Action>();
                this.thens.Enqueue(() => func()?
                    .Done(() => newPromise.Resolve())
                    .Fail(() => newPromise.Reject()));
            }
            else
            {
                return func();
            }
            return newPromise;
        }
    }
}
