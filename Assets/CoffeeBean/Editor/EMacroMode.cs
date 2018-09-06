using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

public static class EMacroMode
{
#if RELEASE
    [MenuItem ( "Tools/设置游戏为 Debug", priority = 0 )]
    private static void SetDebug()
    {
        PlayerSettings.SetScriptingDefineSymbolsForGroup ( BuildTargetGroup.Android, "DEBUG" );
    }
#elif DEBUG
    [MenuItem ( "Tools/设置游戏为 Release", priority = 1 )]
    private static void SetRelease()
    {
        PlayerSettings.SetScriptingDefineSymbolsForGroup ( BuildTargetGroup.Android, "RELEASE" );
    }
#endif
}

