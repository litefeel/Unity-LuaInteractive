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
        const string OldPath = "ProjectSettings/LuaInteractive.json";
        const string SettingPath = "UserSettings/LuaInteractive.json";
        private static SettingData settingData;

        [InitializeOnLoadMethod]
        private static void Init()
        {
            settingData = new SettingData();
            MoveSettingFile();
            LoadData();
        }

        private static void MoveSettingFile()
        {
            if (File.Exists(OldPath) && !File.Exists(SettingPath))
            {
                var dir = Path.GetDirectoryName(SettingPath);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                File.Move(OldPath, SettingPath);
            }
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
            var dir = Path.GetDirectoryName(SettingPath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            var json = JsonUtility.ToJson(settingData);
            File.WriteAllText(SettingPath, json);
        }
    }
}


