﻿using CoffeeBean;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/********************************************************************
created:    2018/05/31
created:    31:5:2018   11:26
filename:   D:\Work\PushCoin\trunk\PushCoin\Assets\CoffeeBean\Data\CUserData.cs
file path:  D:\Work\PushCoin\trunk\PushCoin\Assets\CoffeeBean\Data
file base:  CUserData
file ext:   cs
author:     Leo

purpose:    用户数据，这里存储吉祥美基本数据
*********************************************************************/


/// <summary>
/// 吉祥美大厅返回用户数据结构
/// </summary>
[Serializable]
public class JXMDataInfo
{
    //进程ID
    public string session_id;
    //用户ID
    public string user_id;
    //用户名
    public string user_name;
    //用户等级
    public string user_grade;
    //用户性别
    public string user_sex;
    //用户头像
    public string user_icon;
    //用户吉祥美积分
    public string score;
    //用户VIP等级'
    public string vip_grade;
    //游戏ID
    public string game_id;
    //会场名
    public string local;
    //会场ID
    public string localID;
}


/// <summary>
/// 用户数据
/// </summary>
public partial class UserData : CSingleton<UserData>
{
    //吉祥美数据
    public JXMDataInfo JXMData { get; set; }

    /// <summary>
    /// 构造函数用于设定默认值
    /// </summary>
    public UserData()
    {
        JXMData = new JXMDataInfo();

#if UNITY_EDITOR
        JXMData = CJsonLoader.LoadJsonFromResources<JXMDataInfo> ( "Resources/Config/Account.json" );
#endif
    }

}
