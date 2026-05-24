require('UI/EditorDebug/LuaEditorDebug')
require('UI/Base/LuaRequires')

local function Init()
    CS.UnityEngine.Debug.Log("LuaMainStar......");
    UISystem:Init();
end

local function Main()
    Init()
end
Main()