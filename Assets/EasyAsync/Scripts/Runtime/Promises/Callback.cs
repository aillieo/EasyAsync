// -----------------------------------------------------------------------
// <copyright file="Callback.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.EasyAsync
{
    using System;

    internal readonly struct Callback
    {
        public readonly Action action;
        public readonly Promise.Status mask;

        public Callback(Promise.Status mask, Action action)
        {
            this.mask = mask;
            this.action = action;
        }
    }
}
