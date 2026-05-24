local string = string
local table = table
enum ={}

-- 打印错误信息
function __TRACKBACK__(errmsg)
    local track_text = debug.traceback(tostring(errmsg))
    logError(track_text)
    return false;
end

--重新require一个lua文件，替代系统文件。
function reimport(name)
    local package = package
    package.loaded[name] = nil
    package.preload[name] = nil
    return require(name)    
end

function default_ctor(self, ...)
	local obj = {}
	setmetatable(obj, self)
    if obj.__ctor then
        obj:__ctor(...)
    end
    if obj.__init then
        obj:__init(...)
    end
	return obj
end

function class(super, _cls)
	local cls = {}
    if _cls then 
		for k,v in pairs(_cls) do
			cls[k] = v
		end
	end

    local mt = {}
    if super then setmetatable(mt, super) end
    mt.__index = mt
    mt.__call = function(self, ...)
        if self.New then
            return self:New(...)
        else
            return default_ctor(self, ...)
        end
    end
    mt.__super = super
    setmetatable(cls, mt)
    cls.__index = cls
    return cls
end

functional = {}

function functional.bind(func, count, ...)
    local args_origin = {...}
    return function(...)
        local args = {...}
        local num = table.maxn(args)
        for i=num, 1, -1 do
            args[i + count] = args[i]
        end
        for i=1,count do
            args[i] = args_origin[i]
        end
        return func(table.unpack(args))
    end
end

function functional.bindself(self, fname)
    return functional.bind1(self[fname], self)
end

function functional.bind1(func, _1)
    return function(...)
        return func(_1, ...)
    end
end

function functional.bind2(func, _1, _2)
    return function(...)
        return func(_1, _2, ...)
    end
end

function functional.bind3(func, _1, _2, _3)
    return function(...)
        return func(_1, _2, _3, ...)
    end
end

function functional.bind4(func, _1, _2, _3, _4)
    return function(...)
        return func(_1, _2, _3, _4, ...)
    end
end

function functional.bind1_1(func, _1)
    return function(_s1, ...)
        return func(_1, ...)
    end
end

function functional.bind2_1(func, _1, _2)
    return function(_s1, ...)
        return func(_1, _2, ...)
    end
end

function functional.bind3_1(func, _1, _2, _3)
    return function(_s1, ...)
        return func(_1, _2, _3, ...)
    end
end

function functional.bind4_1(func, _1, _2, _3, _4)
    return function(_s1, ...)
        return func(_1, _2, _3, _4, ...)
    end
end

function functional.bind1_2(func, _1)
    return function(_s1, _s2, ...)
        return func(_1, ...)
    end
end

function functional.bind2_2(func, _1, _2)
    return function(_s1, _s2, ...)
        return func(_1, _2, ...)
    end
end

function functional.bind3_2(func, _1, _2, _3)
    return function(_s1, _s2, ...)
        return func(_1, _2, _3, ...)
    end
end

function functional.bind4_2(func, _1, _2, _3, _4)
    return function(_s1, _s2, ...)
        return func(_1, _2, _3, _4, ...)
    end
end

function table.maxn(tbl)
    local max = nil
    local count = 0
    for k, v in pairs(tbl) do
        if type(k) == "number" then
            if max then
                if k >= 0 and k > max then max = k end
            else
                if k >= 0 then max = k end
            end
        end
        count = count + 1
    end
    if count == 0 then
        max = 0
    end
    return max
end

function table.contains(tbl, value)
    for k,v in pairs(tbl) do
        if v == value then
            return k
        end
    end
end

function table.containsKey(tbl, key)
    for k,v in pairs(tbl) do
        if k == key then
            return true
        end
    end
    return false
end
-- added by wb
-- 遍历读(后面无需for .. pairs)
function table.walk(tb, func)
    for k,v in pairs(tb) do
        func(k, v)
    end
end

function table.getLength(_table)
    local length = 0
    if type(_table)~='table' then
        logError('_table不是table类型,Error')
        return 0
    end
    for _k,_v in pairs(_table)do
        length=length+1
    end
    return length
end

-- 重要的事情说三遍 SafePack与SafeUnpack要成对使用 SafePack与SafeUnpack要成对使用  SafePack与SafeUnpack要成对使用
-- 解决原生pack的nil截断问题
function table.SafePack(...)
	local params = {...}
	params.lens = select('#', ...)
	return params
end

-- 解决原生unpack的nil截断问题
function table.SafeUnpack(safe_pack_tb)
	return unpack(safe_pack_tb, 1, safe_pack_tb.lens)
end

-- 按指定的排序方式遍历
function table.walksort(tb, sort_func, walk_func)
	local keys = table.keys(tb)
	table.sort(keys, function(lkey, rkey)
		return sort_func(lkey, rkey)
	end)
	for i = 1, table.length(keys) do
		walk_func(keys[i], tb[keys[i]])
	end
end

-- 筛选出符合条件的项
function table.choose(tb, func)
	local choose = {}
    for k, v in pairs(tb) do
        if func(k, v) then 
			choose[k] = v
		end
    end
	return choose
end

-- 过滤掉不符合条件的项
function table.filter(tb, func)
	local filter = {}
    for k, v in pairs(tb) do
        if not func(k, v) then 
			filter[k] = v
		end
    end
	return filter
end

function string.split(input, delimiter)
    input = tostring(input)
    delimiter = tostring(delimiter)
    if (delimiter=='') then return false end
    local pos,arr = 0, {}
    -- for each divider found
    for st,sp in function() return string.find(input, delimiter, pos, true) end do
        table.insert(arr, string.sub(input, pos, st - 1))
        pos = sp + 1
    end
    table.insert(arr, string.sub(input, pos))
    return arr
end

---@param _table
---@param value System.Int32
function enum.GetVal(_table,value)
    for key, val in pairs(_table) do
        if val == value then
            return key
        end
    end
    return nil -- 如果找不到对应的键
end 