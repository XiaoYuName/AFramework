using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace XFramework
{
    public class UGUIEndDragLuaBehaviour : 
        BaseLuaBehaviour<UGUIEndDragLuaBehaviour>,IEndDragHandler
    {
        private Action<PointerEventData> luaOnEndDrag;
    
    
        public static UGUIEndDragLuaBehaviour Bind(GameObject go, Action<PointerEventData> func)
        {
            UGUIEndDragLuaBehaviour behaviour = go.AddComponent<UGUIEndDragLuaBehaviour>();
            behaviour.self = null;
            behaviour.luaOnEndDrag += func;
            return behaviour;
        }
    
    
        public void OnEndDrag(PointerEventData eventData)
        {
            luaOnEndDrag?.Invoke(eventData);
        }
    }
}

