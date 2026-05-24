local LuaClass = require "UI/Basic/LuaClass"
---@class UnityObjBase
---@field transform UnityEngine.Transform
local UnityObjBase = class()

--XLua框架中UnityObject的基类，一切UnityObject继承自本类。

function UnityObjBase:New(obj, ...)
    setmetatable(obj, self)
    if obj.__init then
        obj:__init(...)
    end
    return obj
end

function UnityObjBase:get(str, ...)
    if(IsNull(self.transform)) then
        return
    end
    local tf = self.transform:Find(str)

    if not tf then
        CS.UnityEngine.Debug.LogError("can not find child " .. str)
        return
    end

    local obj = LuaClass(tf.gameObject)
    obj = UnityObjBase(obj)
    local typeNames = { ... }
    for _, typeName in pairs(typeNames) do
        local comp = tf:GetComponent(typeName);
        if comp then
            if type(typeName) == "userdata" then
                obj[typeName.Name] = comp
            else
                obj[typeName] = comp
            end
        else
            CS.UnityEngine.Debug.LogError("can not find compnent " .. typeName .. " for " .. str)
        end
    end
    return obj
end

---@param paht string @子级路径
---@param type string @UnityType 组件
function UnityObjBase:Get(paht,type)
    if CS.System.String.IsNullOrEmpty(paht) then
        -- return self.transform:GetComponent(type);
        return CSharpUIHelper.GetComponentName(self.transform,type);
    else
        local tf = self.transform:Find(paht);
        if not tf then
            CS.UnityEngine.Debug.LogError("can not find child " .. type..'path '..paht)
            return
        end
        if UIHelper:IsNullOrEmpty(type) then
            return tf.gameObject;
        end
        return CSharpUIHelper.GetComponentName(tf.transform,type);
       -- return tf.transform:GetComponent(type);
    end
end


return UnityObjBase