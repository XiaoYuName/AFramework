using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace XFramework
{
    public class UGUIPointerExitLuaBehaviour : 
        BaseLuaBehaviour<UGUIPointerExitLuaBehaviour>,IPointerExitHandler
    {
        private Action<PointerEventData> luaOnPointerExit;
    
        public static UGUIPointerExitLuaBehaviour Bind(GameObject go, Action<PointerEventData> func)
        {
            UGUIPointerExitLuaBehaviour behaviour = go.AddComponent<UGUIPointerExitLuaBehaviour>();
            behaviour.self = null;
            behaviour.luaOnPointerExit += func;
            return behaviour;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            luaOnPointerExit?.Invoke(eventData);
        }
    }
}

