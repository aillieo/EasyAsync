using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AillieoUtils.EasyAsync.CoroutineExtensions;
using UnityEngine;

namespace AillieoUtils.EasyAsync.Sample
{
    public class CoroutineAsTask : MonoBehaviour
    {
        private async void Start()
        {
            Debug.LogError("begin");
            await new WaitForSeconds(2);
            Debug.LogError("222");
            await StartCoroutine(CustomCoroutine());
            Debug.LogError("end");
            await CustomCoroutine();
            Debug.LogError("end 2");
        }

        private IEnumerator CustomCoroutine()
        {
            UnityEngine.Debug.LogError("CustomCoroutine 0");
            yield return new WaitForSeconds(2);
            UnityEngine.Debug.LogError("CustomCoroutine 1");
            yield return new WaitForSeconds(2);
            UnityEngine.Debug.LogError("CustomCoroutine 2");
        }
    }
}
