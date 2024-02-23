// -----------------------------------------------------------------------
// <copyright file="CoroutineRunner.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.EasyAsync.CoroutineExtensions
{
    using UnityEngine;

    internal class CoroutineRunner : MonoBehaviour
    {
        private static CoroutineRunner instance;

        internal static CoroutineRunner Instance
        {
            get
            {
                CreateInstance();
                return instance;
            }
        }

        // [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void CreateInstance()
        {
            if (instance == null)
            {
                GameObject go = new GameObject($"[{nameof(CoroutineRunner)}]");
                instance = go.AddComponent<CoroutineRunner>();
                DontDestroyOnLoad(go);
            }
        }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this);
            }
        }
    }
}
