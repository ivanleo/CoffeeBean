/********************************************************************
    created:    2017/10/26
    created:    26:10:2017   20:40
    filename:   D:\Work\NanJingMaJiang\trunk\Project\CoffeeBean\Core\CApp.cs
    file path:  D:\Work\NanJingMaJiang\trunk\Project\CoffeeBean\Core
    file base:  CApp
    file ext:   cs
    author:     Leo

    purpose:    单例基类
*********************************************************************/
using UnityEngine;

namespace CoffeeBean
{
    /// <summary>
    /// 单例基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class CSingleton<T> where T : CSingleton<T>, new()
    {
        /// <summary>
        /// 泛型单例实例
        /// </summary>
        protected static T m_Instance = null;

        /// <summary>
        /// 单例
        /// </summary>
        public static T Instance
        {
            get
            {
                if( m_Instance == null )
                {
                    m_Instance = new T();
                }

                return m_Instance;
            }
        }

    }


    /// <summary>
    /// 具备MonoBehaviour生命周期的单例对象
    /// 该单例一经调用，会在当前场景创建一个GameObject
    /// 该单例代表该GameObject对象身上的脚本组件实例
    /// !!!
    /// 继承 CSingletonMono 的单例对象可以使用协程
    /// 继承 CSingleton     的单例对象不能使用协程
    /// </summary>
    /// <typeparam name="T">泛型 T ,必须继承本类</typeparam>
    public abstract class CSingletonMono<T> : MonoBehaviour where T : CSingletonMono<T>
    {
        /// <summary>
        /// 类的实例
        /// </summary>
        protected static T m_Instance = null;

        /// <summary>
        /// 单例
        /// </summary>
        public static T Instance
        {
            get
            {
                // 没有找到实例
                if ( m_Instance == null )
                {
                    string m_instanceName = typeof( T ).Name;
                    GameObject temp = new GameObject( m_instanceName );
                    m_Instance = temp.AddComponent<T>();
                    DontDestroyOnLoad( temp );
                }
                return m_Instance;
            }
        }

        /// <summary>
        /// 销毁时
        /// </summary>
        protected virtual void OnDestroy()
        {
            m_Instance = null;
        }
    }
}
