local super = UIBase
---@type UnityEngine.GameObject
local CommonUI = class(super,{

})

function CommonUI:__init()
    ---@type PropertyBarUI
    self.propertyBarUI = self:Get('UIMask/PropertyBarUI','PropertyBarUI')
    self.propertyBarUI:Init();
    
    self.StartGameButton = self:Get('UIMask/MenuButtons/StartGameButton','UnityEngine.UI.Button');
    --self.ShopButton = self:Get('UIMask/MenuButtons/ShopButton','UnityEngine.UI.Button');
    --self.ExitButton = self:Get('UIMask/MenuButtons/ExitButton','UnityEngine.UI.Button');

    ButtonBind(self.StartGameButton,functional.bind1(self.StartGame,self),"");
    --ButtonBind(self.ShopButton,functional.bind1(self.OpenShop,self),"");
    --ButtonBind(self.ExitButton,functional.bind1(self.ExitGame,self),"");
end

function CommonUI:StartGame()
    GameManager:EnterGunmanGame();
end

function CommonUI:OpenShop()
end

function CommonUI:ExitGame()
    GameManager:QuitGunmanGame();
end



return CommonUI