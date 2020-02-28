using System.IO;
using UnityEditor;
using UnityEngine;

namespace litefeel.LuaInteractive.Editor
{
    public enum ClearLogMode
    {
        None,
        Previous,
        All,
    }
    public class SettingData
    {
        public string scriptPath;
        public ClearLogMode clearLogMode;
    }
    public static class Settings
    {
        const string SettingPath = "ProjectSettings/LuaInteractive.json";
        private static SettingData settingData;

        [InitializeOnLoadMethod]
        private static void Init()
        {
            settingData = new SettingData();
            LoadData();
        }

        public static ClearLogMode AutoClearLog
        {
            get { return settingData.clearLogMode; }
            set
            {
                if (value != settingData.clearLogMode)
                {
                    settingData.clearLogMode = value;
                    SaveData();
                }
            }
        }

        public static string ScriptPath
        {
            get { return settingData.scriptPath; }
            set
            {
                if (value != settingData.scriptPath)
                {
                    settingData.scriptPath = value;
                    SaveData();
                }
            }
        }

        private static void LoadData()
        {
            try
            {
                var data = File.ReadAllText(SettingPath);
                JsonUtility.FromJsonOverwrite(data, settingData);
            }
            catch { }
        }
        private static void SaveData()
        {
            var json = JsonUtility.ToJson(settingData);
            File.WriteAllText(SettingPath, json);
        }
    }
}


