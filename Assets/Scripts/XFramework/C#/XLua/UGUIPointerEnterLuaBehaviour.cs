using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace XFramework
{
    public class UGUIPointerEnterLuaBehaviour : 
        BaseLuaBehaviour<UGUIPointerEnterLuaBehaviour>,IPointerEnterHandler
    {
        private Action<PointerEventData> luaOnPointerEnter;
    
        public static UGUIPointerEnterLuaBehaviour Bind(GameObject go, Action<PointerEventData> func)
        {
            UGUIPointerEnterLuaBehaviour behaviour = go.AddComponent<UGUIPointerEnterLuaBehaviour>();
            behaviour.self = null;
            behaviour.luaOnPointerEnter += func;
            return behaviour;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            luaOnPointerEnter?.Invoke(eventData);
        }
    }
}

