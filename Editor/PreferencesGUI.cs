using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityScript.Lang;


namespace litefeel.LuaInteractive.Editor
{
    public static class PreferencesGUI
    {
#if UNITY_2018_3_OR_NEWER
        private class MyPrefSettingsProvider : SettingsProvider
        {
            public MyPrefSettingsProvider(string path, SettingsScope scopes = SettingsScope.User)
            : base(path, scopes)
            { }

            public override void OnGUI(string searchContext)
            {
                PreferencesGUI.OnGUI();
            }
        }

        [SettingsProvider]
        static SettingsProvider NewPreferenceItem()
        {
            return new MyPrefSettingsProvider("Preferences/Lua Interactive");
        }
#else
        [PreferenceItem("Lua Interactive")]
#endif
        public static void OnGUI()
        {
            Settings.AutoClearLog = EditorGUILayout.ToggleLeft("Auto Clear Log", Settings.AutoClearLog);
            Settings.LuaPath = EditorGUILayout.TextField("Lua Script File", Settings.LuaPath);
            using(new EditorGUI.DisabledScope(string.IsNullOrEmpty(Settings.LuaPath)))
            {
                if (GUILayout.Button("Create defualt lua script"))
                    CreateDefaultScript();
            }
        }

        private static void CreateDefaultScript()
        {
            var path = Settings.LuaPath;
            if (string.IsNullOrEmpty(path)) return;

            if (File.Exists(path))
            {
                if(!EditorUtility.DisplayDialog("Save File",
                    $"the file already exists, do you want to overwrite it?\n{path}",
                    "Overwrite", "Cancel"))
                {
                    return;
                }
            }

            var srcfile = "Packages/com.litefeel.luainteractive/Editor/defaultlua.lua";
            File.Copy(srcfile, path, true);
        }
    }
}
