using System;
using System.IO;
using UnityEditor;
using UnityEngine;


namespace litefeel.LuaInteractive.Editor
{
    public static class PreferencesGUI
    {
#if UNITY_2018_3_OR_NEWER
        private class MyPrefSettingsProvider : SettingsProvider
        {
            public MyPrefSettingsProvider(string path, SettingsScope scopes = SettingsScope.Project)
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
            return new MyPrefSettingsProvider("Project/Lua Interactive");
        }
#else
        [PreferenceItem("Lua Interactive")]
#endif
        public static void OnGUI()
        {
            Settings.AutoClearLog = (ClearLogMode)EditorGUILayout.EnumPopup("Auto Clear Log", Settings.AutoClearLog);

            EditorGUILayout.BeginHorizontal();
            Settings.ScriptPath = EditorGUILayout.TextField("Lua Script File", Settings.ScriptPath);
            if (GUILayout.Button("Browse", EditorStyles.miniButton, GUILayout.Width(80)))
                BrowseScriptFile();
            EditorGUILayout.EndHorizontal();
            if (!string.IsNullOrEmpty(Settings.ScriptPath) && !File.Exists(Settings.ScriptPath))
                EditorGUILayout.HelpBox("The file not exits", MessageType.Warning);

            using (new EditorGUI.DisabledScope(string.IsNullOrEmpty(Settings.ScriptPath)))
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
                Settings.ScriptPath = relativePath;
            }
        }

        private static void CreateDefaultScript()
        {
            var path = Settings.ScriptPath;
            if (string.IsNullOrEmpty(path)) return;

            if (File.Exists(path))
            {
                if (!EditorUtility.DisplayDialog("Save File",
                    "the file already exists, do you want to overwrite it?\n" + path,
                    "Overwrite", "Cancel"))
                {
                    return;
                }
            }

            var srcfile = GetDefultLuaFile();
            File.Copy(srcfile, path, true);
        }

        /// <summary>
        /// 获得调用函数的类名和方法。
        /// </summary>
        /// <returns></returns>
        public static string GetDefultLuaFile()
        {
            var st = new System.Diagnostics.StackTrace(1, true).GetFrame(0);
            var dir = System.IO.Path.GetDirectoryName(st.GetFileName());
            return Path.Combine(dir, "defaultlua.lua");
        }
    }
}
