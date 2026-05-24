using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace XFramework
{
    [LuaCallCSharp]
    public class CSharpCallLua : MonoSingleton<CSharpCallLua>
    {
        
        /// <summary>
        /// 释放到对LuaEnv 内lua代码的delegate 引用
        /// </summary>
        public void Dispose()
        {
            
        }
    }
}

