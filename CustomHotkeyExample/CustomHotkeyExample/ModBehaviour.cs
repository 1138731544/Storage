using System.Collections.Generic;
using UnityEngine;
using Duckov.Modding;

namespace CustomHotkeyExample
{
    public class ModBehaviour : Duckov.Modding.ModBehaviour
    {
        #region 需要复制的部分
        private const string YourHotkeyName = "example";
        private const KeyCode DefaultYourHotkey = KeyCode.N;
        private KeyCode yourHotkey = DefaultYourHotkey;
        
        protected override void OnAfterSetup()
        {
            base.OnAfterSetup();
            // 勾选mod时触发热键获取
            GetCustomHotkey();
        }

        private void OnEnable()
        {
            ModManager.OnScan += OnModScan;
        }
        
        private void OnModScan(List<ModInfo> _)
        {
            // 在主流程获取模组信息后触发热键获取
            GetCustomHotkey();
            // 先移除后添加防止反复进出主界面导致的监听溢出
            CustomHotkeyHelper.RemoveEvent2OnCustomHotkeyChangedEvent(GetCustomHotkey);
            CustomHotkeyHelper.AddEvent2OnCustomHotkeyChangedEvent(GetCustomHotkey);
        }

        private void OnDisable()
        {
            ModManager.OnScan -= OnModScan;
            CustomHotkeyHelper.RemoveEvent2OnCustomHotkeyChangedEvent(GetCustomHotkey);
        }
        
        /// <summary>
        /// 获取自定义热键
        /// </summary>
        private void GetCustomHotkey()
        {
            CustomHotkeyHelper.Init();
            KeyCode customTeleportHotkey = CustomHotkeyHelper.GetHotkey(YourHotkeyName);
            yourHotkey = customTeleportHotkey == KeyCode.None ? DefaultYourHotkey : customTeleportHotkey;
            CustomHotkeyHelper.AddNewHotkey(YourHotkeyName, DefaultYourHotkey, "示例热键");
        }
        #endregion
        
        private void Update()
        {
            if (Input.GetKeyDown(yourHotkey))
            {
                CharacterMainControl.Main.PopText("CustomHotkeyExample");
            }
        }
    }
}