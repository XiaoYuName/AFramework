-- Tencent is pleased to support the open source community by making xLua available.
-- Copyright (C) 2016 THL A29 Limited, a Tencent company. All rights reserved.
-- Licensed under the MIT License (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
-- http://opensource.org/licenses/MIT
-- Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.

local util = require 'UI/Base/util'

local cs_coroutine_runner = CS.Coroutine_Runner.GetInstance();
if not cs_coroutine_runner then
	local gameobject = CS.UnityEngine.GameObject('Coroutine_Runner')
	CS.UnityEngine.Object.DontDestroyOnLoad(gameobject)
	cs_coroutine_runner = gameobject:AddComponent(typeof(CS.Coroutine_Runner))
end

local function async_yield_return(to_yield, cb)
    cs_coroutine_runner:YieldAndCallback(to_yield, cb)
end

local yield_return = util.async_to_sync(async_yield_return, 2);

function coroutine.start(func)
	local co = coroutine.create(func)
    assert(coroutine.resume(co))
    return co
end

function coroutine.start(func, ...)
	local co = coroutine.create(func)
	local success, result = coroutine.resume(co, ...)
	if not success then
		error(result)  -- 可以替换为更优雅的错误处理
	end
	return co, result
end

function coroutine.step()
	yield_return(nil)
end

function coroutine.wait(t)
	yield_return(CS.UnityEngine.WaitForSeconds(t))
end

function coroutine.www(www)
	yield_return(www)
end

function coroutine.stop(co)
	--coroutine.yield(co);
end

return {
	yield_return = yield_return
}