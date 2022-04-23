using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AillieoUtils.EasyAsync.CoroutineExtensions
{
    internal class CoroutineRunner : MonoBehaviour
    {
        //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void CreateInstance()
        {
            if (instance == null)
            {
                GameObject go = new GameObject($"[{nameof(CoroutineRunner)}]");
                instance = go.AddComponent<CoroutineRunner>();
                DontDestroyOnLoad(go);
            }
        }

        private static CoroutineRunner instance;

        internal static CoroutineRunner Instance
        {
            get
            {
                CreateInstance();
                return instance;
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
