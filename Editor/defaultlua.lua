local function clearfile(path, needRequire, resetGValueOrName)
    path = string.gsub(path, "[\\/]", ".")
    path = string.gsub(path, "%.lua$", "")
    -- print(path)
    package.loaded[path] = nil
    if not needRequire then return end

    local gValName = resetGValueOrName
    if resetGValueOrName == true then
        gValName = string.gsub( path, ".*%.", "")
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

-- do anything

