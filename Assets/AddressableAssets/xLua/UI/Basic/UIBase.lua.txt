UnityObjBase = require "UI/Basic/UnityObjBase"
local super = UnityObjBase
local tweenTime = 0.25;
--UI界面的Base类，项目所有UI界面均继承自本类
---@class UIBase:UnityObjBase
UIBase = class(super,{
    isShow = false,
    uiName = nil,
    uiLayer = nil,
    isTween = false,
})

--UI界面打开的方法，供子类重写
function UIBase:Open()
    self.transform:SetAsLastSibling()
    self.isShow = true
    if(not self.gameObject.activeSelf) then
        self.gameObject:SetActive(true)
    end
    
    if self.isTween then
        self.transform.localScale = Vector3.zero;
        self.transform:DOScale(Vector3.one,tweenTime)
    end
end

--UI界面关闭的方法,供子类重写
function UIBase:Close()
    if self.isTween then
        local tween = self.transform:DOScale(Vector3.zero,tweenTime);
        tween:OnComplete(function()
            if(not self.isShow) then
                return
            end
            self.isShow = false
            if(self.gameObject.activeSelf) then
                self.gameObject:SetActive(false)
            end
        end)
    else
        if(not self.isShow) then
            return
        end
        self.isShow = false
        if(self.gameObject.activeSelf) then
            self.gameObject:SetActive(false)
        end
    end
end