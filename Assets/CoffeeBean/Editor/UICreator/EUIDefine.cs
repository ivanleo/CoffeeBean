using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// 导出方式
/// </summary>
public enum EExportType
{
    /// <summary>
    /// 只导出定义
    /// </summary>
    EXPORT_DEFINE,

    /// <summary>
    /// 导出所有（定义+实现)
    /// </summary>
    EXPORT_ALL
}

/// <summary>
/// UI类型
/// </summary>
public enum EUIType
{
    /// <summary>
    /// 单一UI
    /// </summary>
    SINGLE,

    /// <summary>
    /// 多元UI
    /// </summary>
    MUTI,
}

/// <summary>
/// UI组件
/// </summary>
[Serializable]
public class UIComp
{
    /// <summary>
    /// 程序集
    /// </summary>
    public string assembly;

    /// <summary>
    /// 组件类型
    /// </summary>
    public string compType;

    /// <summary>
    /// 是否导出
    /// </summary>
    public bool export;

    /// <summary>
    /// 完整类型
    /// </summary>
    public string fullType;
}

/// <summary>
/// UI信息
/// </summary>
[Serializable]
public class UIInfo
{
    /// <summary>
    /// 资源保存路径
    /// </summary>
    public string      assetPath;

    /// <summary>
    /// 导出模式
    /// </summary>
    public EExportType exportType;

    /// <summary>
    /// 是否模态UI
    /// </summary>
    public bool isModel;

    /// <summary>
    /// 模态背景色
    /// </summary>
    public Color modelbackcolor = new Color(0f,0f,0f,0.6f);

    /// <summary>
    /// 预制体存放路径
    /// </summary>
    public string      prefab_path;

    /// <summary>
    /// UI类型
    /// </summary>
    public EUIType       uiType;
}

/// <summary>
/// UI节点
/// </summary>
[Serializable]
public class UINode
{
    /// <summary>
    /// 节点组件清单
    /// </summary>
    public List<UIComp> comps;

    /// <summary>
    /// 注释
    /// </summary>
    public string desc;

    /// <summary>
    /// 是否导出
    /// </summary>
    public bool export;

    /// <summary>
    /// 层级
    /// </summary>
    public int level;

    /// <summary>
    /// 节点名
    /// </summary>
    public string node_name;

    /// <summary>
    /// 节点路径，相对于根节点
    /// </summary>
    public string path;
}

/// <summary>
/// UI根节点
/// </summary>
[Serializable]
public class UIRoot
{
    /// <summary>
    /// UI信息
    /// </summary>
    public UIInfo info = new UIInfo();

    /// <summary>
    /// 节点清单
    /// </summary>
    public List<UINode> nodes = new List<UINode>();
}