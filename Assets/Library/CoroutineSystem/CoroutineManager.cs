using System;
using System.Collections;
using System.Collections.Generic;
using Library.Patterns.Singleton;
using UnityEngine;

namespace Library.CoroutineSystem
{
    public class CoroutineManager : MonoSingleton<CoroutineManager>
    {
        public static Coroutine StartChildCoroutine(IEnumerator method)
        {
            return Instance.StartCoroutine(method);
        }

        public static void StartChildCoroutine(string method)
        {
            Instance.StartCoroutine(method);
        }

        public static void StopChildCoroutine(Coroutine method)
        {
            if (method != null)
            {
                Instance.StopCoroutine(method);
            }

            method = null;
        }

        public static void StopChildCoroutine(string method)
        {
            Instance.StopCoroutine(method);
        }

        public static void DoAfterFixedUpdate(Action actionToInvoke)
        {
            Instance.StartCoroutine(Wait(Time.fixedDeltaTime, actionToInvoke));
        }

        public static Coroutine DoAfterGivenTime(float waitTime, Action actionToInvoke)
        {
            return Instance.StartCoroutine(Wait(waitTime, actionToInvoke));
        }

        public static Coroutine DoAfterGivenUnscaledTime(float waitTime, Action actionToInvoke)
        {
            return Instance.StartCoroutine(WaitUnscaled(waitTime, actionToInvoke));
        }

        public IEnumerator ProcessMultipleCoroutines(IEnumerable<IEnumerator> coroutines, Action actionToInvoke = null)
        {
            foreach (var enumerator in coroutines)
            {
                yield return StartCoroutine(enumerator);
            }

            actionToInvoke?.Invoke();
        }

        private static IEnumerator Wait(float time, Action actionToInvoke)
        {
            yield return new WaitForSeconds(time);

            actionToInvoke.Invoke();
        }        
        
        private static IEnumerator WaitUnscaled(float time, Action actionToInvoke)
        {
            yield return new WaitForSecondsRealtime(time);

            actionToInvoke.Invoke();
        }
    }
}
