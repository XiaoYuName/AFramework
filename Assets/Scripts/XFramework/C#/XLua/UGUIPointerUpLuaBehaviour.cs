using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace XFramework
{
    public class UGUIPointerUpLuaBehaviour : 
        BaseLuaBehaviour<UGUIPointerUpLuaBehaviour>,IPointerUpHandler
    {
        private Action<PointerEventData> luaOnPointerUp;
    
        public static UGUIPointerUpLuaBehaviour Bind(GameObject go, Action<PointerEventData> func)
        {
            UGUIPointerUpLuaBehaviour behaviour = go.AddComponent<UGUIPointerUpLuaBehaviour>();
            behaviour.self = null;
            behaviour.luaOnPointerUp += func;
            return behaviour;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            luaOnPointerUp?.Invoke(eventData);
        }
    }
}

