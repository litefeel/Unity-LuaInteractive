#if UNITY_EDITOR
using litefeel.LuaInteractive.Editor;
#endif
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace litefeel.LuaInteractive
{
    public class LuaRunner : MonoBehaviour
    {
        private static Type LuaState;
        private static MethodInfo GetLuaState;
        private static MethodInfo DoString;
        private object luaState;

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                LuaState = assembly.GetType("LuaInterface.LuaState");
                if (LuaState != null)
                    break;
            }

            if (LuaState != null)
            {
                GetLuaState = GetMethod(LuaState, "Get", new object[] { typeof(IntPtr) });
                DoString = GetMethod(LuaState, "DoString", new object[] { typeof(string), typeof(string) });
            }
            if (LuaState == null)
                Debug.LogError($"Cannot get type LuaInterface.LuaState");
            if (GetLuaState == null)
                Debug.LogError($"Cannot get method LuaInterface.LuaState:Get(IntPtr)");
            if (DoString == null)
                Debug.LogError($"Cannot get method LuaInterface.LuaState:DoString(string, string)");

            var debuger = new GameObject("_LuaRunner");
            DontDestroyOnLoad(debuger);
            debuger.AddComponent<LuaRunner>();
        }

        private static MethodInfo GetMethod(Type type, string name, object[] args)
        {
            foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
            {
                if (!method.IsGenericMethod && method.Name == name)
                    return method;
            }
            return null;
        }

        // Start is called before the first frame update
        void Start()
        {
            if (GetLuaState != null && DoString != null)
                StartCoroutine(WaitLuaState());
        }

        private IEnumerator WaitLuaState()
        {
            var args = new object[] { IntPtr.Zero };
            while (GetLuaState.Invoke(null, args) == null)
                yield return null;

            yield return new WaitForSeconds(1);

            luaState = GetLuaState.Invoke(null, args);
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R) && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftControl))
            {
                var path = Settings.LuaPath;
                if(!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    var content = File.ReadAllText(path);
                    DoString?.Invoke(luaState, new object[] { content, null });
                }
//                const string chunk =
//    @"package.loaded['mydebug'] = nil
//require('mydebug')";
//                DoString?.Invoke(luaState, new object[] { chunk, null });
                //luaState.DoString(chunk);

                if (Settings.AutoClearLog)
                {
                    Assembly assembly = Assembly.GetAssembly(typeof(SceneView));
                    Type type = assembly.GetType("UnityEditor.LogEntries");
                    MethodInfo method = type.GetMethod("Clear");
                    method.Invoke(new object(), null);
                }
            }
        }
#endif
    }
}
