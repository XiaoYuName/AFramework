using System;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using XLua;

namespace  XFramework
{
    /// <summary>
    /// Lua端 C# 调用层 UISystem
    /// </summary>
    [CSharpCallLua]
    public class LuaUISystem : MonoOdinSingleton<LuaUISystem>
    {
        [BoxGroup("Events"),HideInInspector]
        public Action<string> openUI = null;

        [BoxGroup("Events"),HideInInspector]
        public Action<string> closeUI = null;

        [BoxGroup("Events"),HideInInspector]
        public Action<string, Action<LuaTable>> openUIAsync;
        
        /// <summary>
        /// 打开Lua UI 界面
        /// </summary>
        /// <param name="uiPage"></param>
        public void OpenUI(string uiPage)
        {
            openUI?.Invoke(uiPage);
        }
        
        /// <summary>
        /// 关闭Lua UI 界面
        /// </summary>
        /// <param name="uiPage"></param>
        public void CloseUI(string uiPage)
        {
            closeUI?.Invoke(uiPage);
        }
        
        /// <summary>
        /// 异步打开Lua UI 界面
        /// </summary>
        /// <param name="uiPage">ui名称</param>
        /// <param name="ui">Lua UI 打开后回调函数</param>
        public void OpenUIAsync(string uiPage, Action<LuaTable> ui)
        {
            openUIAsync?.Invoke(uiPage, ui);
        }


        public void Dispose()
        {
            openUI = null;
            closeUI = null;
            openUIAsync = null;
        }

    }
    
}

