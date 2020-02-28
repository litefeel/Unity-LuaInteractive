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
            Settings.AutoClearLog = (Settings.ClearLog)EditorGUILayout.EnumPopup("Auto Clear Log", Settings.AutoClearLog);

            EditorGUILayout.BeginHorizontal();
            Settings.LuaPath = EditorGUILayout.TextField("Lua Script File", Settings.LuaPath);
            if (GUILayout.Button("Browse", EditorStyles.miniButton, GUILayout.Width(80)))
                BrowseScriptFile();
            EditorGUILayout.EndHorizontal();
            if (!string.IsNullOrEmpty(Settings.LuaPath) && !File.Exists(Settings.LuaPath))
                EditorGUILayout.HelpBox("The file not exits", MessageType.Warning);

            using (new EditorGUI.DisabledScope(string.IsNullOrEmpty(Settings.LuaPath)))
            {
                if (GUILayout.Button("Create defualt lua script"))
                    CreateDefaultScript();
            }
        }

        private static void BrowseScriptFile()
        {
            var root = Directory.GetCurrentDirectory();
            var path = EditorUtility.SaveFilePanel("Save File", root, "", "lua");

            if (!string.IsNullOrEmpty(path))
            {
                Uri file = new Uri(path);
                Uri folder = new Uri(root + "/");
                string relativePath =
                Uri.UnescapeDataString(
                    folder.MakeRelativeUri(file)
                        .ToString()
                    );
                Settings.LuaPath = relativePath;
            }
        }

        private static void CreateDefaultScript()
        {
            var path = Settings.LuaPath;
            if (string.IsNullOrEmpty(path)) return;

            if (File.Exists(path))
            {
                if (!EditorUtility.DisplayDialog("Save File",
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
