# LuaInteractive


[![](https://img.shields.io/github/release/litefeel/Unity-LuaInteractive.svg?label=latest%20version)](https://github.com/litefeel/Unity-LuaInteractive/releases)
[![](https://img.shields.io/github/license/litefeel/Unity-LuaInteractive.svg)](https://github.com/litefeel/Unity-LuaInteractive/blob/master/LICENSE.md)
[![Donate](https://img.shields.io/badge/Donate-PayPal-green.svg)](https://paypal.me/litefeel)

[Lua Interactive][LuaInteractive] is just perfect Unity editor plugin to excute lua on play mode.

## Feature list

- Free
- Excute lua on play mode
- No runtime resources required
- No scripting required



## Install

#### Using npm (Ease upgrade in Package Manager UI)**Recommend**

Find the manifest.json file in the Packages folder of your project and edit it to look like this:
``` js
{
  "scopedRegistries": [
    {
      "name": "My Registry",
      "url": "https://registry.npmjs.org",
      "scopes": [
        "com.litefeel"
      ]
    }
  ],
  "dependencies": {
    "com.litefeel.luainteractive": "1.1.0",
    ...
  }
}
```

#### Using git

Find the manifest.json file in the Packages folder of your project and edit it to look like this:
``` js
{
  "dependencies": {
    "com.litefeel.luainteractive": "https://github.com/litefeel/Unity-LuaInteractive.git#1.1.0",
    ...
  }
}
```

#### Using .zip file (for Unity 5.0+)

1. Download `Source code` from [Releases](https://github.com/litefeel/Unity-LuaInteractive/releases)
2. Extract the package into your Unity project


## How to use?

1. Select `Edit > Project Settings… > Lua Interactive` from the menu
2. Input a lua file path
3. Press `Create defualt lua script`
4. Play game
5. Press Ctrl + Shift + R
6. Excute the lua script


## Support

- Create issues by [issues][issues] page
- Send email to me: <litefeel@gmail.com>


[LuaInteractive]: https://github.com/litefeel/Unity-LuaInteractive (LuaInteractive)
[issues]: https://github.com/litefeel/Unity-LuaInteractive/issues (LuaInteractive issues)
