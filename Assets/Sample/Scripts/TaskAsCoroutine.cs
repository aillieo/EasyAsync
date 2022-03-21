using System.Collections;
using System.Threading.Tasks;
using AillieoUtils.EasyAsync.TaskExtensions;
using UnityEngine;

namespace AillieoUtils.EasyAsync.Sample
{
    public class TaskAsCoroutine : MonoBehaviour
    {
        private IEnumerator Start()
        {
            yield return Task.Delay(1000).AsCoroutine();
            UnityEngine.Debug.LogError("1");
            yield return Task.Delay(1000).AsCoroutine();
            UnityEngine.Debug.LogError("2");
            yield return Task.Delay(1000).AsCoroutine();
            UnityEngine.Debug.LogError("3");
        }
    }
}
