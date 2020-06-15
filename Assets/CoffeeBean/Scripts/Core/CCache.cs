/********************************************************************
	created:	2019/01/30
	file base:	CCache.cs
	author:		Leo

	purpose:	数据缓存类

    // 示例
    var ui_cache = new CCache<GameObject>();

    // 添加到缓存
    ui_cache.Add("UI_Bag", BagUI);

    // 获取缓存的资源
    var cachedUI = ui_cache.Get("UI_Bag");

*********************************************************************/

using System.Collections.Generic;

using CoffeeBean;

/// <summary>
/// 缓存类
/// </summary>
public class CCache<T>
{
    /// <summary>
    /// 缓存
    /// </summary>
    private Dictionary<string, T> m_Cache;

    public CCache()
    {
        m_Cache = new Dictionary<string, T>();
    }

    /// <summary>
    /// 缓存一个对象
    /// </summary>
    /// <param name="key"></param>
    /// <param name="target"></param>
    public void Add( string key, T target )
    {
        if ( m_Cache.ContainsKey( key ) )
        {
            CLOG.I( "cache", $"the cache key:{key} has res so replace it!" );
            m_Cache[key] = target;
        }
        else
        {
            m_Cache.Add( key, target );
        }
    }

    /// <summary>
    /// 通过key 获得对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public T Get( string key )
    {
        if ( m_Cache.ContainsKey( key ) )
        {
            return m_Cache[key];
        }

        return default( T );
    }

    /// <summary>
    /// 检查是否已经缓存某个对象
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool Has( string key )
    {
        return m_Cache.ContainsKey( key );
    }

    /// <summary>
    /// 释放所有资源
    /// </summary>
    public void ReleaseAll()
    {
        m_Cache.Clear();
    }

    /// <summary>
    /// 通过key来释放缓存
    /// </summary>
    /// <param name="key"></param>
    public bool RemoveByKey( string key )
    {
        return m_Cache.Remove( key );
    }

    /// <summary>
    /// 通过值来释放缓存
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public bool RemoveByValue( T data )
    {
        foreach ( var item in m_Cache )
        {
            if ( item.Value.Equals( data ) )
            {
                return m_Cache.Remove( item.Key );
            }
        }

        return false;
    }
}