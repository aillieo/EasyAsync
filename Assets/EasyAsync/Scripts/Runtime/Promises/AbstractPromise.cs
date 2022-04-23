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

        public AbstractPromise OnFulfilled(Action onFulfilled)
        {
            if (state == State.Pending)
            {
                this.callbacks ??= new Queue<Callback>();
                this.callbacks.Enqueue(new Callback(onFulfilled, State.Fulfilled));
            }
            else if (state == State.Fulfilled)
            {
                onFulfilled();
            }
            return this;
        }

        public AbstractPromise OnRejected(Action onRejected)
        {
            if (state == State.Pending)
            {
                this.callbacks ??= new Queue<Callback>();
                this.callbacks.Enqueue(new Callback(onRejected, State.Rejected));
            }
            else if (state == State.Rejected)
            {
                onRejected();
            }
            return this;
        }

        public AbstractPromise OnRejected(Action<string> onRejected)
        {
            if (state == State.Pending)
            {
                this.callbacks ??= new Queue<Callback>();
                this.callbacks.Enqueue(new Callback(() => onRejected(reason), State.Rejected));
            }
            else if (state == State.Rejected)
            {
                onRejected(reason);
            }
            return this;
        }

        public AbstractPromise Then(Action onFulfilled, Action onRejected = null)
        {
            if (state == State.Pending)
            {
                Promise newPromise = new Promise();
                this.callbacks ??= new Queue<Callback>();
                this.callbacks.Enqueue(new Callback(
                    onFulfilled,
                    State.Fulfilled));
                this.callbacks.Enqueue(new Callback(
                    onRejected,
                    State.Rejected));
            }
            else if (state == State.Fulfilled)
            {
                onFulfilled();
            }
            else if (state == State.Rejected)
            {
                onRejected();
            }

            return this;
        }

        public AbstractPromise Then(Action onFulfilled, Action<string> onRejected = null)
        {
            if (state == State.Pending)
            {
                this.callbacks ??= new Queue<Callback>();
                this.callbacks.Enqueue(new Callback(
                    onFulfilled,
                    State.Fulfilled));
                this.callbacks.Enqueue(new Callback(
                    () => onRejected(this.reason),
                    State.Rejected));
            }
            else if (state == State.Fulfilled)
            {
                onFulfilled();
            }
            else if (state == State.Rejected)
            {
                onRejected(this.reason);
            }

            return this;
        }

        public AbstractPromise Then(Func<Promise> onFulfilled, Func<AbstractPromise> onRejected = null)
        {
            if (state == State.Pending)
            {
                Promise newPromise = new Promise();
                this.callbacks ??= new Queue<Callback>();
                this.callbacks.Enqueue(new Callback(
                    () => onFulfilled()?
                        .OnFulfilled(() => newPromise.Resolve()),
                    State.Fulfilled));
                this.callbacks.Enqueue(new Callback(
                    () => onRejected()?
                        .OnRejected(rsn => newPromise.Reject(rsn)),
                    State.Rejected));
                return newPromise;
            }
            else if (state == State.Fulfilled)
            {
                return onFulfilled();
            }
            else if (state == State.Rejected)
            {
                return onRejected();
            }

            throw new Exception();
        }

        public void Reject(string reason)
        {
            this.reason = reason;
            this.exception = new Exception(reason);
            state = State.Rejected;

            if (callbacks != null)
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

        public void Reject(Exception exception)
        {
            this.exception = exception;
            this.reason = exception.Message;
            state = State.Rejected;

            if (callbacks != null)
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
