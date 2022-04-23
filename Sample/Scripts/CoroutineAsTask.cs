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
            Debug.Log("Start 0");
            await new WaitForSeconds(2);
            Debug.Log("Start 1");
            await StartCoroutine(CustomCoroutine());
            Debug.Log("Start 2");
            await CustomCoroutine();
            Debug.Log("Start 3");
        }

        private IEnumerator CustomCoroutine()
        {
            Debug.Log("CustomCoroutine 0");
            yield return new WaitForSeconds(2);
            Debug.Log("CustomCoroutine 1");
            yield return new WaitForSeconds(2);
            Debug.Log("CustomCoroutine 2");
        }
    }
}
