using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Assertions;

namespace AillieoUtils.EasyAsync
{
    public abstract class AbstractPromise
    {
        [Flags]
        public enum State : byte
        {
            Pending = 0b00,
            Fulfilled = 0b01,
            Rejected = 0b10,
        }

        protected struct Callback
        {
            public Action action;
            public State flag;

            public Callback(Action action, State flag)
            {
                this.action = action;
                this.flag = flag;
            }
        }

        private State s = State.Pending;
        public State state
        {
            get { return s;}
            protected set
            {
                Assert.AreEqual(s, State.Pending);
                Assert.AreNotEqual(value, State.Pending);
                s = value;
            }
        }

        public string reason { get; protected set; }
        public Exception exception { get; protected set; }

        protected Queue<Callback> callbacks;

        public AbstractPromise OnFulfilled(Action func)
        {
            if (state == State.Pending)
            {
                this.callbacks = this.callbacks ?? new Queue<Callback>();
                this.callbacks.Enqueue(new Callback(func, State.Fulfilled));
            }
            else if (state == State.Fulfilled)
            {
                func();
            }
            return this;
        }

        public AbstractPromise OnRejected(Action func)
        {
            if (state == State.Pending)
            {
                this.callbacks = this.callbacks ?? new Queue<Callback>();
                this.callbacks.Enqueue(new Callback(func, State.Rejected));
            }
            else if (state == State.Rejected)
            {
                func();
            }
            return this;
        }

        public AbstractPromise OnRejected(Action<string> func)
        {
            if (state == State.Pending)
            {
                this.callbacks = this.callbacks ?? new Queue<Callback>();
                this.callbacks.Enqueue(new Callback(() => func(reason), State.Rejected));
            }
            else if (state == State.Rejected)
            {
                func(reason);
            }
            return this;
        }

        public AbstractPromise OnComplete(Action func)
        {
            if(state == State.Pending)
            {
                this.callbacks = this.callbacks ?? new Queue<Callback>();
                this.callbacks.Enqueue(new Callback(func, State.Fulfilled | State.Rejected));
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
                this.callbacks = this.callbacks ?? new Queue<Callback>();
                this.callbacks.Enqueue(new Callback(
                    () => func()?
                        .OnFulfilled(() => newPromise.Resolve())
                        .OnRejected(rsn => newPromise.Reject(rsn)),
                    State.Fulfilled | State.Rejected));
            }
            else
            {
                return func();
            }
            return newPromise;
        }
    }
}
