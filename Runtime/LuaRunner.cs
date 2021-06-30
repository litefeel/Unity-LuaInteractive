#if UNITY_EDITOR || LUA_RUNNER_RUNTIME
#if UNITY_EDITOR
using litefeel.LuaInteractive.Editor;
#endif
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace litefeel.LuaInteractive
{
    public class LuaRunner : MonoBehaviour
    {
        enum LuaType
        {
            None,
            ToLua,
            XLua,
        }
        private static Type LuaState;
        private static MethodInfo GetLuaState;
        private static MethodInfo DoString;
        private object luaState;
        private static LuaType luaType;


        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            luaType = LuaType.None;
            if (IsToLua(out GetLuaState, out DoString))
                luaType = LuaType.ToLua;
            else if (IsXLua(out GetLuaState, out DoString))
                luaType = LuaType.XLua;

            //Debug.LogError("LuaRunner.Init");
            var debuger = new GameObject("_LuaRunner");
            debuger.hideFlags = HideFlags.DontSave;
            DontDestroyOnLoad(debuger);
            debuger.AddComponent<LuaRunner>();
        }



        void Start()
        {
            if (luaType != LuaType.None)
                StartCoroutine(WaitLuaState());
        }

        private IEnumerator WaitLuaState()
        {
            //Debug.LogError("LuaRunner.WaitLuaState");
            if (luaType == LuaType.ToLua)
            {
                var args = new object[] { IntPtr.Zero };
                while (GetLuaState.Invoke(null, args) == null)
                    yield return null;

                yield return new WaitForSeconds(1);

                luaState = GetLuaState.Invoke(null, args);
            }
            else if (luaType == LuaType.XLua)
            {
                var args = new object[] { };
                while (GetLuaState.Invoke(null, args) == null)
                    yield return null;

                yield return new WaitForSeconds(1);

                luaState = GetLuaState.Invoke(null, args);
            }
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.R) && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftControl))
#else
            if (Input.GetKeyDown(KeyCode.F8))
#endif
            {
#if UNITY_EDITOR
                if (Settings.AutoClearLog == ClearLogMode.Previous)
                    ClearLog();
                var path = Settings.ScriptPath;
#else
                var path = Path.Combine(Application.persistentDataPath, "_luarunner.lua");
#endif

                //Debug.LogError("LuaRunner.Update ");
                if (luaState != null && !string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    
                    var content = File.ReadAllText(path);
                    switch (luaType)
                    {
                        case LuaType.ToLua:
                            DoString.Invoke(luaState, new object[] { content, null });
                            break;
                        case LuaType.XLua:
                            DoString.Invoke(luaState, new object[] { content, null, null });
                            Debug.LogError("LuaRunner.Update xlua");
                            break;
                    }
                }
#if UNITY_EDITOR
                if (Settings.AutoClearLog == ClearLogMode.All)
                    ClearLog();
#endif
            }
        }

        private void ClearLog()
        {
#if UNITY_EDITOR
            Assembly assembly = Assembly.GetAssembly(typeof(UnityEditor.SceneView));
            Type type = assembly.GetType("UnityEditor.LogEntries");
            MethodInfo method = type.GetMethod("Clear");
            method.Invoke(new object(), null);
#endif
        }

        private static bool IsToLua(out MethodInfo GetLuaState, out MethodInfo DoString)
        {
            GetLuaState = null;
            DoString = null;
            Type LuaState = TryGetType("LuaInterface.LuaState");
            if (LuaState == null)
                return false;

            GetLuaState = GetMethod(LuaState, "Get", new Type[] { typeof(IntPtr) });
            if (GetLuaState == null)
                return false;

            DoString = GetMethod(LuaState, "DoString", new Type[] { typeof(string), typeof(string) });
            if (DoString == null)
                return false;

            return true;
        }

        private static bool IsXLua(out MethodInfo GetLuaState, out MethodInfo DoString)
        {
            GetLuaState = null;
            DoString = null;
            Type LuaManager = TryGetType("LuaManager");
            if (LuaManager == null)
                return false;

            GetLuaState = GetMethod(LuaManager, "GetLuaEnv", new Type[0]);
            if (GetLuaState == null)
                return false;

            var LuaTable = TryGetType("XLua.LuaTable");
            if (LuaTable == null)
                return false;
            var LuaEnv = TryGetType("XLua.LuaEnv");
            if (LuaEnv == null)
                return false;
            DoString = GetMethod(LuaEnv, "DoString", new Type[] { typeof(string), typeof(string), LuaTable });
            if (DoString == null)
                return false;

            return true;
        }


        private static Type TryGetType(string fullname)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var type = assembly.GetType(fullname);
                if (type != null)
                    return type;
            }
            return null;
        }

        private static MethodInfo GetMethod(Type type, string name, Type[] args)
        {
            return type.GetMethod(name, args);

            //foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
            //{
            //    if (method.IsGenericMethod)
            //        continue;
            //    if (method.Name != name)
            //        continue;
            //    var margs = method.GetParameters(); 
            //        return method;
            //}
            //return null;
        }
    }
}
#endif
