using System;
using XLua;

namespace XFramework
{
    public class MonoLuaBehaviour : BaseLuaBehaviour<MonoLuaBehaviour>
    {
        private Action<LuaTable> luaStart;
        private Action<LuaTable> luaOnEnable;
        private Action<LuaTable> luaOnDisable;
        private Action<LuaTable> luaOnDestroy;

        public override void Init()
        {
            self.Get("Start", out luaStart);
            self.Get("OnEnable", out luaOnEnable);
            self.Get("OnDisable", out luaOnDisable);
            self.Get("OnDestroy", out luaOnDestroy);
        }

        void Start()
        {
            if (luaStart != null)
                luaStart(self);
        }

        void OnEnable()
        {
            if (luaOnEnable != null)
                luaOnEnable(self);
        }

        void OnDisable()
        {
            if(!XLuaManager.IsInitialized)return;
            if (luaOnDisable != null)
                luaOnDisable(self);
        }

        void OnDestroy()
        {
            if (luaOnDestroy != null && XLuaManager.IsInitialized)
                luaOnDestroy(self);
        }
    }
}
