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
            Debug.Log("1");
            yield return Task.Delay(1000).AsCoroutine();
            Debug.Log("2");
            yield return Task.Delay(1000).AsCoroutine();
            Debug.Log("3");
        }
    }
}
