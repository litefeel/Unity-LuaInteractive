local function clearfile(path, needRequire)
    path = string.gsub(path, "[\\/]", ".")
    path = string.gsub(path, "%.lua$", "")
    print(path)
    package.loaded[path] = nil
    if needRequire then
        require(path)
    end
end

-- do anything

