using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace  XFramework
{
    public class UGUIPointerDownLuaBehaviour :
        BaseLuaBehaviour<UGUIPointerDownLuaBehaviour>,IPointerDownHandler
    {
        private Action<PointerEventData> luaOnPointerDown;
    
        public static UGUIPointerDownLuaBehaviour Bind(GameObject go, Action<PointerEventData> func)
        {
            UGUIPointerDownLuaBehaviour behaviour = go.AddComponent<UGUIPointerDownLuaBehaviour>();
            behaviour.self = null;
            behaviour.luaOnPointerDown += func;
            return behaviour;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            luaOnPointerDown?.Invoke(eventData);
        }
    }
}

