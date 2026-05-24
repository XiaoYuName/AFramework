function IsNull(unity_object)
    if unity_object == nil then
        return true
    end

    if type(unity_object) == "userdata" and unity_object.IsNull ~= nil then
        return unity_object:IsNull()
    end

    return false
end

function NotNull(unity_object)
    return not IsNull(unity_object)
end

---@alias 绑定一个UnityButton事件
---@param Btn UnityEngine.UI.Button
---@param fun function @回调函数
---@param audio string @播放的音效ID
function ButtonBind(Btn,fun,audio)
    if(Btn ~= nil) then
        Btn.onClick:RemoveAllListeners();
        local callback = function()
            if fun then
                fun()
            end
            AudioManager:PlayAudio(audio)
        end
        Btn.onClick:AddListener(callback)
    end
end

---@alias 增加绑定一个UnityButton事件
---@param
function AddButtonBind(btn,fun)
    if btn ~= nil then
        btn.onClick:AddListener(fun)
    end
end

---@alias 减少一个UnityButton事件
function RemoveSingletonBind(btn,fun)
    if(btn ~= nil) then
        btn.onClick:RemoveListener(fun)
    end
end


---@alias 绑定一个UnityToggle事件
---@param toggle UnityEngine.UI.Toggle
---@param fun function
---@param audio string
function ToggleBind(toggle,fun,audio)
    if (toggle ~= nil) then
        toggle.onValueChanged:RemoveAllListeners();
        local callback = function(isOn)
            if fun then
                fun(isOn)
            end
            AudioManager:PlayAudio(audio)
        end
        toggle.onValueChanged:AddListener(callback)
    end
end

---@alias 增加一个Toggle绑定事件
function AddToggleBind(toggle,fun)
    if toggle ~= nil then
        toggle.onValueChanged:AddListener(fun)
    end
end

---@alias 减少一个Toggle绑定事件
function RemoveSingletonToggleBind(toggle,fun)
    if (toggle ~= nil) then
        toggle.onValueChanged:RemoveListener(fun)
    end
end

---@alias 绑定一个UnitySlider事件
---@param slider UnityEngine.UI.Slider
---@param fun function
function SliderBind(slider,fun)
    if (slider ~= nil) then
        slider.onValueChanged:RemoveAllListeners();
        local callback = function(value)
            if fun then
                fun(value)
            end
        end
        slider.onValueChanged:AddListener(callback)
    end
end

function RemoveSliderBind(slider)
    if (slider ~= nil) then
        slider.onValueChanged:RemoveAllListeners();
    end
end


function RemoveToggleBind(toggle)
    if (toggle ~= nil) then
        toggle.onValueChanged:RemoveAllListeners();
    end
end

function RemoveButtonBind(Btn)
    if(Btn ~= nil) then
        Btn.onClick:RemoveAllListeners();
    end
end

function TMP_InputFiledBind(inputField,fun)
    if (inputField ~= nil) then
        inputField.onValueChanged:RemoveAllListeners();
        local callback = function(value)
            if fun then
                fun(value)
            end
        end
        inputField.onValueChanged:AddListener(callback)
    end
end


