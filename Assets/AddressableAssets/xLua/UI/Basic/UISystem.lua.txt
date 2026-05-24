-- UISystem为UI管理类，统一管理界面的开关操作,开关界面直接调用ShowUI,CloseUI即可
UISystem = {}

local uiTables = nil;

--创建与UITable对应的映射表
UISystem.UITable = {};
UISystem.UIClient ={};

UILayer ={
    ['Ground'] = 'Ground',
    ['EffectDown'] = 'EffectDown',
    ['Environment'] = 'Player',
    ['EffectUIDown'] = 'EffectTop',
    ['UIDown'] = 'UIDown',
    ['UIPanel'] = 'UIPanel',
    ['UIAutoPanel'] = 'UIAutoPanel',
    ['UIPop'] = 'UIPop',
    ['UITop'] = 'UITop',
    ['EffectUITop'] = 'EffectUITop',
}


function UISystem.Init()
    UISystem.InitUIRoot();
    CSharpCallLua.Init();
end

--UISystem 初始化加载配置表中的lua
function UISystem.InitUIRoot()
    ---@type XFramework.UIPageItem
    local Tables = CSharpUISystem:GetUIPages();
    uiTables = UIHelper:ListToTable(Tables)
end

---@public 打开一个UI界面
---@param uiPage string
function UISystem.OpenUI(uiPage)
    local _openui = UISystem.UITable[uiPage];
    if _openui ~= nil and _openui.gameObject ~= nil then
        _openui.transform.localPosition = Vector3(0, 0, 0)
        _openui:Open()
    else
        UISystem.OpenUIAsync(uiPage);
    end
end


---@public 异步打开一个UI界面
---@param uiPage string
---@param callback function
function UISystem.OpenUIAsync(uiPage,callback)
    local _open = UISystem.UITable[uiPage];
    if _open ~= nil and _open.gameObject ~= nil then
        _open.transform.localPosition = Vector3(0, 0, 0)
        _open:Open()
        if callback ~= nil then
            callback(_open)
        end
    else
        UISystem.LoadUIAsync(uiPage,function(ui)
            ui:Open();
            if callback ~= nil then
                callback(ui)
            end
        end)
    end
end


---@public 获取界面对象
---@param uiPage string
---@return fun():nil
function UISystem.GetUI(uiPage)
    local ui = UISystem.UITable[uiPage];
    if ui ~= nil and ui.gameObject ~= nil then
        return ui
    end
    return nil
end


---@public 异步获取一个UI界面
function UISystem.GetUIAsync(uiPage,callback)
    local ui = UISystem.UITable[uiPage];
    if ui ~= nil and ui.gameObject ~= nil then
        if callback ~= nil then
            callback(ui)
        end
    end
    return nil
end


---@public 关闭一个UI界面
function UISystem.CloseUI(uiPage)
    local ui = UISystem.UITable[uiPage];
    if ui ~= nil and ui.gameObject ~= nil then
        ui:Close();
    end
end


--内部调用 异步加载UI界面
function UISystem.LoadUIAsync(uiPage,callback)
    for i = 1, #uiTables do
        if uiTables[i].PathID == uiPage then
            if uiTables[i].isXLua then
                local function LoadComplete(prefab)
                    UISystem.__InstantiateUIAsync(uiPage,callback,prefab);
                end
                AssetsManager:LoadAssetsAsyncWrapper(uiTables[i].PagePath,LoadComplete)
                break;
            end
        end
    end
end

--内部调用 实例化UI界面
function UISystem.__InstantiateUIAsync(uiPage,callback,prefab)
    local ui = UISystem.UITable[uiPage]
    if ui ~= nil then
        if callback ~= nil then
            callback(ui)
        end
        return
    end
    for i = 1, #uiTables do
        if uiTables[i].PathID == uiPage then
            local parent = CsharpUISystem:GetUILayer(uiTables[i].UICanvas,uiTables[i].UIParent)
            local uiObj  = GameObject.Instantiate(prefab, parent)
            uiObj.transform.localPosition = CS.UnityEngine.Vector3(0, 0, 0)
            uiObj:SetActive(false)

            if package.loaded[uiTables[i].xLuaPath] ~= nil then
                package.loaded[uiTables[i].xLuaPath] = nil
            end
            local xlsScript = require(uiTables[i].xLuaPath)
            local _base = xlsScript(LuaClass(uiObj))
            _base.uiName = uiPage
            _base.isTween = uiTables[i].isTween;
            UISystem.UITable[uiPage] = _base;
            if(callback ~= nil) then
                callback(_base)
            end
            break;
        end
    end
end


---@alias 将UI添加到二级UI界面列表中
---@param uiPage string
function UISystem.AddClient(uiPage)
    table.insert(UISystem.UIClient,uiPage)
end

---@alias 关闭当前所有二级UI界面列表
function UISystem.CloseClient()
    for i = 1, table.getLength(UISystem.UIClient) do
        UISystem.CloseUI(UISystem.UIClient[i])
    end
end

---@alias 清空所有二姐UI界面列表
function UISystem.ClearClient()
    UISystem.UIClient ={};
end
