using UnityEngine;
using CoffeeBean;
using System.Collections.Generic;
using UnityEngine.U2D;
using System;

/// <summary>
/// 资源管理器
/// </summary>
public static class CResourcesManager
{
    /// <summary>
    /// 预制体缓存
    /// </summary>
    private static Dictionary<string, GameObject> m_PrefabCache = new Dictionary<string, GameObject>();

    /// <summary>
    /// 精灵缓存
    /// </summary>
    private static Dictionary<string, Sprite> m_SpriteCache = new Dictionary<string, Sprite>();

    /// <summary>
    /// 声音缓存
    /// </summary>
    private static Dictionary<string, AudioClip> m_AudioCache = new Dictionary<string, AudioClip>();

    /// <summary>
    /// 材质缓存
    /// </summary>
    private static Dictionary<string, Material> m_MaterialCache = new Dictionary<string, Material>();


    /// <summary>
    /// 加载对象
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    /// <param name="PrefabPath">路径</param>
    /// <param name="IsPersistent">是否持久化</param>
    /// <param name="Dic">存储容器</param>
    /// <returns></returns>
    private static T LoadObject<T> ( string PrefabPath, bool IsPersistent, Dictionary<string, T> Dic ) where T : UnityEngine.Object
    {
        string Path = HandleResourcePath ( PrefabPath );

        if ( Dic.ContainsKey ( Path ) )
        {
            return Dic[Path];
        }

        T ob = Resources.Load<T> ( Path );
        if ( IsPersistent )
        {
            Dic.Add ( Path, ob );
        }

        return ob;
    }

    /// <summary>
    /// 加载一个预制体，并创建其对象
    /// </summary>
    /// <param name="PrefabPath">加载路径</param>
    /// <param name="IsPersistent">是否在内存中保留镜像持久化</param>
    /// <returns></returns>
    public static GameObject LoadPrefab ( string PrefabPath, bool IsPersistent = false )
    {
        return LoadObject<GameObject> ( PrefabPath, IsPersistent, m_PrefabCache );
    }

    /// <summary>
    /// 创建一个预制体的实例
    /// </summary>
    /// <param name="PrefabPath">预制体路径</param>
    /// <param name="IsPersistent">是否持久化</param>
    /// <returns></returns>
    public static GameObject CreatePrefab ( string PrefabPath, bool IsPersistent = false )
    {
        GameObject ob = GameObject.Instantiate ( LoadPrefab ( PrefabPath, IsPersistent ) );
        int start = PrefabPath.LastIndexOf ( '/' );
        int end = PrefabPath.LastIndexOf ( '.' );
        if ( start == -1 ) { start = 0; }
        if ( end == -1 ) { end = PrefabPath.Length; }

        ob.name = PrefabPath.Substring ( start + 1, end - start - 1 );
        return ob;
    }

    /// <summary>
    /// 加载一个图集精灵
    /// </summary>
    /// <param name="AltasPath">精灵图集路径</param>
    /// <param name="SpriteName">精灵文件名</param>
    /// <param name="IsPersistent">是否持久化</param>
    /// <returns></returns>
    public static Sprite LoadAltasSprite ( string AltasPath, string SpriteName, bool IsPersistent = false )
    {
        string Path = HandleResourcePath ( AltasPath );

        string SP_Key = Path + "_" + SpriteName;

        if ( m_SpriteCache.ContainsKey ( SP_Key ) )
        {
            return m_SpriteCache[SP_Key];
        }

        var sp = Resources.Load<SpriteAtlas> ( Path ).GetSprite ( SpriteName );

        if ( IsPersistent )
        {
            m_SpriteCache.Add ( SP_Key, sp );
        }

        return sp;
    }

    /// <summary>
    /// 加载一个散图精灵
    /// </summary>
    /// <param name="SpritePath">精灵路径</param>
    /// <param name="IsPersistent">是否持久化</param>
    /// <returns></returns>
    public static Sprite LoadSprite ( string SpritePath, bool IsPersistent = false )
    {
        return LoadObject<Sprite> ( SpritePath, IsPersistent, m_SpriteCache );
    }

    /// <summary>
    /// 读取一个材质
    /// </summary>
    /// <param name="MaterialPath">材质路径</param>
    /// <param name="IsPersistent">是否持久化</param>
    /// <returns></returns>
    public static Material LoadMaterial ( string MaterialPath, bool IsPersistent = false )
    {
        return LoadObject<Material> ( MaterialPath, IsPersistent, m_MaterialCache );
    }

    /// <summary>
    /// 声音缓存
    /// </summary>
    /// <param name="AudioPath"></param>
    /// <param name="IsPersistent"></param>
    /// <returns></returns>
    public static AudioClip LoadAudio ( string AudioPath, bool IsPersistent = false )
    {
        return LoadObject<AudioClip> ( AudioPath, IsPersistent, m_AudioCache );
    }

    /// <summary>
    /// 加载文本
    /// </summary>
    /// <param name="TextPath"></param>
    /// <returns></returns>
    public static string LoadText ( string TextPath )
    {
        string Path = HandleResourcePath ( TextPath );
        return Resources.Load<TextAsset> ( Path ).text;
    }

    /// <summary>
    /// 处理路径
    /// 去除 Resources/
    /// 去除后缀 .*
    /// </summary>
    /// <param name="SourcePath">源路径</param>
    /// <returns></returns>
    private static string HandleResourcePath ( string SourcePath )
    {
        if ( SourcePath.StartsWith ( "Resources" ) ||
                SourcePath.StartsWith ( "resources" ) )
        {
            SourcePath = SourcePath.Substring ( 10 );
        }

        int PointPos = SourcePath.LastIndexOf ( '.' );
        if ( PointPos > 0 )
        {
            SourcePath = SourcePath.Substring ( 0, PointPos );
        }
        return SourcePath;
    }
}
