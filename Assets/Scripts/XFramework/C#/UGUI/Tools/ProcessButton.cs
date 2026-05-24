using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XFramework
{
    public class ProcessButton : Button
    {
        public bool isProcess;
        [SerializeField]
        public float process_timer = 0.15f;

        private float timer = 0;

        private bool isPointerDown = false;

        public UnityEvent OnProcess;

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            isPointerDown = false;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            isPointerDown = true;
        }


        private void Update()
        {
            timer += Time.deltaTime;
            if (isProcess && timer >= process_timer && isPointerDown)
            {
                timer = 0;
                OnProcess?.Invoke();
            }
            else if (!isProcess && isPointerDown)
            {
                OnProcess?.Invoke();
            }
        }
    }
}

