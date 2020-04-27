local clearfile
local path2chunkname
local mt = {}
local globalT = setmetatable({}, mt)
mt.__index = function(t, k)
    if k == "clearfile" then
        return clearfile
    elseif k == "path2chunkname" then
        return path2chunkname
    else
        return _G[k]
    end
end

path2chunkname = function(path)
    path = string.gsub(path, "[\\/]", ".")
    path = string.gsub(path, "%.lua$", "")
    return path
end

clearfile = function(path, needRequire, resetGValueOrName)
    path = path2chunkname(path)
    -- print(path)
    package.loaded[path] = nil
    if not needRequire then return end

    local gValName = resetGValueOrName
    if resetGValueOrName == true then
        gValName = string.gsub(path, ".*%.", "")
    end
    local gValue = gValName and _G[gValName]

    require(path)

    if gValue and type(gValue) == "table" then
        local newGVal = _G[gValName]
        if type(newGVal) == "table" then
            for k, v in pairs(newGVal) do
                if type(v) ~= "function" then
                    newGVal[k] = v
                end
            end
        end
    end
end

local function _loadfile(path)
    local chunk = loadfile(path)
    if chunk then
        setfenv(chunk, globalT)()
    end
end

-- do anything

