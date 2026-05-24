UIHelper = {}


---@alias 将C#的List转成Lua的Table
---@param CSharpList System.Collections.Generic
function UIHelper:ListToTable(CSharpList)
    if(CSharpList == nil) then
        return;
    end 
    
    --将C#的List转成Lua的Table
    local t = {};
    for i = 0, CSharpList.Count - 1 do
        table.insert(t, CSharpList[i]);
    end
    return t;
end

---@alias 将Lua的表倒转
function UIHelper:TableReverse(t)
    local newTable = {}
    local n =table.getLength(t)
    for i = n, 1, -1 do
        table.insert(newTable, t[i])
    end
    return newTable
end

---@alias 控制gameObject对象的显示隐藏
function UIHelper:ActiveGameObject(gameObject,active)
    if gameObject ~= nil then
        gameObject.gameObject:SetActive(active)
    end
end

---@type UnityEngine.Component
function UIHelper:SetEnable(component,value)
    if component ~= nil then
        component.enabled = value;
    end
end

---@alias 设置Image的图片样式
---@param img UnityEngine.UI.Image
---@param icon UnityEngine.Sprite 
function UIHelper:SetImage(img,icon)
    if img ~= nil and NotNull(img) and icon ~= nil then
        img.sprite = icon;
    end
end

---@param img UnityEngine.UI.Image
function UIHelper:SetIamgeNativeSize(img)
    if img ~= nil and NotNull(img) then
        img:SetNativeSize();
    end
end

---@alias 设置Toggle的Group
---@param toggle UnityEngine.UI.Toggle
---@param group UnityEngine.UI.ToggleGroup
function UIHelper:SetToggleGroup(toggle,group)
    if NotNull(toggle) then
        toggle.group = group;
    end
end

---@alias 设置Toggle的选中状态
---@param toggle UnityEngine.UI.Toggle
---@param isOn boolean
function UIHelper:SetToggle(toggle,isOn)
    if NotNull(toggle) then
        toggle.isOn = isOn;
    end
end

---@alias 控制按钮的点击状态
---@param button UnityEngine.UI.Button
---@param value boolean
function UIHelper:SetButtonInteractable(button,value)
    if NotNull(button) then
        button.interactable = value;
    end
end

---@alias 设置TexMeshProUGUI的文本内容
---@param textUI TMPro.TextMeshProUGUI
---@param str string 
function UIHelper:SetTexMeshProTex(textUI,str)
    if NotNull(textUI) then
        textUI.text = str
    end
end

---@alias 设置TexMeshProUGUI的文本颜色非富文本
---@param textUI TMPro.TextMeshProUGUI
---@param Color TMPro.TMP_ColorGradient
function UIHelper:SetTexMeshPorColor(textUI,Color)
    if NotNull(textUI) then 
        textUI.enableVertexGradient = true;
        textUI.colorGradientPreset = Color;
    end
end

---@alias 判断字符串是否为空
---@param value string
function UIHelper:IsNullOrEmpty(value)
    if value == nil then
        return true;
    end
    return CS.System.String.IsNullOrEmpty(value)
end

---@alias 初始化Slider的最大滑动值
---@param slider UnityEngine.UI.Slider
---@param maxValue System.Int32
function UIHelper:InitSlider(slider,maxValue)
    if NotNull(slider) then
        slider.maxValue = maxValue
    end
end

---@alias 设置Slider组件的滑动值
---@param slider UnityEngine.UI.Slider
---@param value System.Int32
function UIHelper:SetSlider(slider,value)
    if NotNull(slider) then
        slider.value = value
    end
end

---@alias 设置Spine动画组件的动画并且播放一个动画
---@param spineUI Spine.Unity.SkeletonGraphic @Spine控制器组件
---@param dataAssets Spine.Unity.SkeletonDataAsset  @Spine资源文件
---@param playName string @要播放的动画
---@param isLoop boolean 是否循环播放
function UIHelper:SetSpineUI(spineUI,dataAssets,playName,isLoop)
    if(NotNull(spineUI)) and NotNull(dataAssets) then
        spineUI.skeletonDataAsset = dataAssets;
        spineUI.AnimationState:ClearTrack()
        spineUI:Initialize(true)
        UIHelper:SetSpineAnimation(spineUI,0,playName,isLoop);
    end
end

---@alias 设置Spine动画组件为空
---@param spineUI Spine.Unity.SkeletonGraphic @Spine控制器组件
function UIHelper:SetEmptySpineUI(spineUI)
    if(NotNull(spineUI)) then
        spineUI.skeletonDataAsset = nil;
        if NotNull(spineUI.AnimationState) then
            spineUI.AnimationState:ClearTrack()
        end
        spineUI:Initialize(true)
    end
end

---@alias 启用UIEffect
---@param uieffect Coffee.UIEffects.UIEffect
---@param value boolean
function UIHelper:ActiveUIEffect(uieffect,value)
    if(NotNull(uieffect)) then
        uieffect.enabled = value;
    end
end


---@alias 控制Spine播放动画
---@param spineUI Spine.Unity.SkeletonGraphic @Spine控制器组件
---@param trackindex System.Int32
---@param playName string @要播放的动画
---@param isLoop boolean 是否循环播放
function UIHelper:SetSpineAnimation(spineUI,trackindex,playName,isLoop)
    if(NotNull(spineUI)) then 
        spineUI.AnimationState:SetAnimation(trackindex,playName,isLoop)
    end
end

---@alias 控制Spine播放动画
---@param spineUI Spine.Unity.SkeletonGraphic @Spine控制器组件
---@param trackindex System.Int32
---@param playName string @要播放的动画
---@param isLoop boolean 是否循环播放
---@param delay number @过度时长
function UIHelper:AddSpineAnimation(spineUI,trackindex,playName,isLoop,delay)
    if(NotNull(spineUI)) then
        spineUI.AnimationState:AddAnimation(trackindex,playName,isLoop,delay)
    end
end


---@alias 将数值转为N0格式
function UIHelper:Tonumber(value)
    return CSharpUIHelper.ToNumber(value);
end

---@alias 获取组件的RectTransform组件
---@param gameobj UnityEngine.GameObject
function UIHelper:GetRect(gameobj)
    if(NotNull(gameobj)) then
        local rectTransform = gameobj.transform:GetComponent('RectTransform');
        if(NotNull(rectTransform)) then
            return rectTransform;
        else
            return nil;
        end
        
    end
end


