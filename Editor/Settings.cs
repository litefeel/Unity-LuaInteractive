using UnityEditor;
using UnityEngine;

namespace litefeel.LuaInteractive.Editor
{
    public static class Settings
    {
        private const string AutoClearLogKey = "litefeel.LuaInteractive.AutoClearLog";
        private const string LuaPathKey = "litefeel.LuaInteractive.LuaPath";

        public enum ClearLog
        {
            None,
            Previous,
            All,
        }


        [InitializeOnLoadMethod]
        private static void Init()
        {
            _AutoClearLog = (ClearLog)EditorPrefs.GetInt(AutoClearLogKey, 0);
            LuaPath = EditorPrefs.GetString(LuaPathKey, "");
        }

        private static ClearLog _AutoClearLog;
        public static ClearLog AutoClearLog
        {
            get { return _AutoClearLog; }
            set
            {
                if (value != _AutoClearLog)
                {
                    _AutoClearLog = value;
                    EditorPrefs.SetInt(AutoClearLogKey, (int)value);
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


