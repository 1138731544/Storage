using System;
using System.Collections.Generic;
using Duckov.Modding;
using UnityEngine;
using System.Reflection;
using Object = UnityEngine.Object;

// * 改为你的命名空间
namespace CustomHotkeyExample
{
    public static class CustomHotkeyHelper
    {
        // * 改为你的模组名
        private const string ModName = "YourModName";

        private static Duckov.Modding.ModBehaviour? customHotkey;
        
        private static MethodInfo? addNewHotkeyMethod;
        private static MethodInfo? removeHotkeyMethod;
        private static MethodInfo? getHotkeyMethod;
        private static EventInfo? onCustomHotkeyChangedEvent;
        
        /// <summary>
        /// 尝试初始化
        /// </summary>
        /// <remarks>该方法需要首先调用，用来缓存一些反射用的变量</remarks>
        public static void TryInit()
        {
            if (customHotkey != null)
                return;
            (bool isFind, ModInfo modInfo) findResult = TryGetCustomHotkeyModInfo();
            if (!findResult.isFind)
            {
                //Debug.Log($"{ModName}：未找到CustomHotkey模组信息");
                return;
            }
            
            if (!ModManager.IsModActive(findResult.modInfo, out customHotkey))
            {
                //Debug.Log($"{ModName}：CustomHotkey模组未激活");

                // 处理特殊情况下未成功从ModManager获取CustomHotkey实例的情况
                customHotkey = TryGetCustomHotkey();
                if (customHotkey == null) 
                    return;
            }

            Type customHotkeyType = customHotkey.GetType();
            addNewHotkeyMethod = customHotkeyType.GetMethod("AddNewHotkey", BindingFlags.Public | BindingFlags.Instance);
            removeHotkeyMethod = customHotkeyType.GetMethod("RemoveHotkey", BindingFlags.Public | BindingFlags.Instance);
            getHotkeyMethod = customHotkeyType.GetMethod("GetHotkey", BindingFlags.Public | BindingFlags.Instance);
            onCustomHotkeyChangedEvent = customHotkeyType.GetEvent("OnCustomHotkeyChanged", BindingFlags.Public | BindingFlags.Static);
        }

        /// <summary>
        /// 添加新的自定义热键
        /// </summary>
        /// <param name="saveName">保存的热键名</param>
        /// <param name="defaultHotkey">默认热键值</param>
        /// <param name="showName">显示的热键名</param>
        public static void AddNewHotkey(string saveName, KeyCode defaultHotkey, string showName)
        {
            if (customHotkey == null)
            {
                //Debug.Log($"{ModName}：未找到CustomHotkey模组实例");
                return;
            }
            addNewHotkeyMethod?.Invoke(customHotkey, new object[] { ModName, saveName, defaultHotkey, showName });
        }
        
        /// <summary>
        /// 移除自定义热键
        /// </summary>
        /// <param name="saveName">保存的热键名</param>
        public static void RemoveHotkey(string saveName)
        {
            if (customHotkey == null)
            {
                //Debug.Log($"{ModName}：未找到CustomHotkey模组实例");
                return;
            }
            removeHotkeyMethod?.Invoke(customHotkey, new object[] { ModName, saveName });
        }
        
        /// <summary>
        /// 获取自定义按键值
        /// </summary>
        /// <param name="saveName">保存的热键名</param>
        public static KeyCode GetHotkey(string saveName)
        {
            if (customHotkey == null)
            {
                //Debug.Log($"{ModName}：未找到CustomHotkey模组实例");
                return KeyCode.None;
            }

            object? result = getHotkeyMethod?.Invoke(customHotkey, new object[] { ModName, saveName });
            if (result == null)
                return KeyCode.None;
            return Enum.TryParse(result.ToString(), out KeyCode keyCode) ? keyCode : KeyCode.None;
        }
        
        /// <summary>
        /// 尝试添加当热键修改时的回调
        /// </summary>
        /// <remarks>多次调用并不会重复添加回调</remarks>
        public static void TryAddEvent2OnCustomHotkeyChangedEvent(Action callback)
        {
            if (onCustomHotkeyChangedEvent == null)
                return;
            onCustomHotkeyChangedEvent.RemoveEventHandler(null, callback);
            onCustomHotkeyChangedEvent.AddEventHandler(null, callback);
        }
        
        /// <summary>
        /// 移除当热键修改时的回调
        /// </summary>
        public static void RemoveEvent2OnCustomHotkeyChangedEvent(Action callback)
        {
            onCustomHotkeyChangedEvent?.RemoveEventHandler(null, callback);
        }

        
        /// <summary>
        /// 尝试获取自定义按键模组信息
        /// </summary>
        private static (bool isFind, ModInfo modInfo) TryGetCustomHotkeyModInfo()
        {
            List<ModInfo>? modInfos = ModManager.modInfos;
            if (modInfos == null || modInfos.Count == 0)
                return (false, default);
            foreach (ModInfo modInfo in modInfos)
            {
                if (modInfo.publishedFileId == 3594709838)
                    return (true, modInfo);
            }
            return (false, default);
        }
        
        /// <summary>
        /// 尝试从所有程序集中获取自定义按键
        /// </summary>
        private static Duckov.Modding.ModBehaviour? TryGetCustomHotkey()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                if (!assembly.FullName.Contains("CustomHotkey"))
                    continue;
                var type = assembly.GetType("CustomHotkey.ModBehaviour");
                if (type == null)
                    return null;
                return Object.FindObjectOfType(type) as Duckov.Modding.ModBehaviour;
            }
            return null;
        }
    }
}

