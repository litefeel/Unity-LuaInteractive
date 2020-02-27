using UnityEditor;
using UnityEngine;

namespace litefeel.LuaInteractive.Editor
{
    public static class Settings
    {
        private const string AutoClearLogKey = "litefeel.LuaInteractive.AutoClearLog";
        private const string LuaPathKey = "litefeel.LuaInteractive.LuaPath";


        [InitializeOnLoadMethod]
        private static void Init()
        {
            _AutoClearLog = EditorPrefs.GetBool(AutoClearLogKey, false);
            LuaPath = EditorPrefs.GetString(LuaPathKey, "");
        }

        private static bool _AutoClearLog;
        public static bool AutoClearLog
        {
            get { return _AutoClearLog; }
            set
            {
                if (value != _AutoClearLog)
                {
                    _AutoClearLog = value;
                    EditorPrefs.SetBool(AutoClearLogKey, value);
                }
            }
        }

        private static string _LuaPath;
        public static string LuaPath
        {
            get { return _LuaPath; }
            set
            {
                if (value != _LuaPath)
                {
                    _LuaPath = value;
                    EditorPrefs.SetString(LuaPathKey, value);
                }
            }
        }
    }
}


