local LuaClass = class(nil, {
	transform = nil,
	gameObject = nil
})

function LuaClass:New(go)
	local obj = {}
	setmetatable(obj, self)
	if obj.__ctor then
		obj:__ctor(go)
	end
	return obj
end

function LuaClass:__ctor(go)
	self.gameObject = go
	self.transform  = go.transform
end

return LuaClass