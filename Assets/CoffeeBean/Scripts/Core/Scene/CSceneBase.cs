using CoffeeBean;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 场景基类
/// 提供场景事件管理
/// </summary>
public abstract class CSceneBase
{
    /// <summary>
    /// 场景是否老旧，当进行场景切换时，会先把本值置为false 已避免后续逻辑的执行
    /// </summary>
    public bool IsDirty { get; set; }

    /// <summary>
    /// 主摄像机
    /// </summary>
    public Camera MainCamera { get; set; }

    /// <summary>
    /// 当前场景名字
    /// </summary>
    public string SceneName { get => UnityScene.name.Or( "null" ); }

    /// <summary>
    /// 顶层画布
    /// </summary>
    public Canvas TopCanvas { get; set; }

    /// <summary>
    /// UI 画布
    /// </summary>
    public Canvas UICanvas { get; set; }

    /// <summary>
    /// 当前场景
    /// </summary>
    public Scene UnityScene { get; private set; }

    /// <summary>
    /// 进入场景后
    /// </summary>
    /// <param name="scene"></param>
    public abstract void AfterEnterScene( Scene scene );

    /// <summary>
    /// 离开场景前
    /// </summary>
    /// <param name="scene"></param>
    public abstract void BeforeLeftScene( Scene scene );

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="scene"></param>
    public void BindUnityScene( Scene scene )
    {
        UnityScene = scene;
        IsDirty = false;
    }

    /// <summary>
    /// 场景更新
    /// </summary>
    public abstract void SceneUpdate();
}