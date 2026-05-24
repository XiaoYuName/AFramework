using System;
using XLua;

namespace XFramework
{
    public class UpdateLuaBehaviour : BaseLuaBehaviour<UpdateLuaBehaviour>
    {
        private Action<LuaTable> luaUpdate;

        public override void Init()
        {
            self.Get("Update", out luaUpdate);
        }

        void OnEnable()
        {
            UpdateManager.Instance.AddHandler(DoUpdate, this);
        }

        void OnDisable()
        {
            if(UpdateManager.IsInitialized)
                UpdateManager.Instance.RemoveHandler(DoUpdate);
        }

        private void DoUpdate()
        {
            if (luaUpdate != null)
            {
                luaUpdate(self);
            } 
        }
    }
}
