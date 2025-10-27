# 说明
<br>1、这是我为steam版逃离鸭科夫模组《自定义热键(CustomHotkey)》制作的模组使用指南。</br>
<br>2、模组地址<a>https://steamcommunity.com/sharedfiles/filedetails/?id=3594709838</a></br>
<br>3、该模组是为了在开发其他模组时方便快捷的使用契合游戏内输入绑定界面的自定义快捷键方式而制作的工具模组。</br>

# 使用流程
<br>1、复制CustomHotkeyHelper.cs到你的mod文件夹，修改该脚本的命名空间和ModName字段。</br>
<br>2、复制ModBehaviour.cs中使用#region包裹的代码到你的脚本，修改其中的一些命名和注释。</br>
<br>......</br>
<br>测试完成并上传后，记得在你的steam模组页面添加本模组为前置模组</br>

# 注意点
<br>1、如果出现无法引用的命名空间，可以参考CustomHotkeyExample.csproj进行对照</br>
<br>2、用户需要先激活该模组，然后再启用后置模组。如果启用时仍无法生效，尝试重启游戏</br>
<br>3、避免产生依赖，你的模组需要在关闭本模组的情况下仍能正常运行。这应该比较好做到，因为本模组只提供了改键功能，你只要和示例中一样在获取失败时使用默认按键即可。</br>
