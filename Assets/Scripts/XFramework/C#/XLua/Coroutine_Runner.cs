using UnityEngine;
using System.Collections;
using System;

namespace XFramework
{
    public class Coroutine_Runner : MonoBehaviour
    {
        private static Coroutine_Runner _instance;

        public static Coroutine_Runner GetInstance()
        {
            return _instance;
        }

        private void Awake()
        {
            _instance = this;
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
            _instance = null;
        }

        public void YieldAndCallback(object to_yield, Action callback)
        {
            StartCoroutine(CoBody(to_yield, callback));
        }

        private IEnumerator CoBody(object to_yield, Action callback)
        {
            if (to_yield is IEnumerator)
                yield return StartCoroutine((IEnumerator)to_yield);
            else
                yield return to_yield;

            if (callback != null)
                callback();
        }
    }
}

