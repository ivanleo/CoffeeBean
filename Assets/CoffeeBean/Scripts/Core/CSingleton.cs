/********************************************************************
	All Right Reserved By Leo
	Created:	2019/01/14 15:21
	File :      CSingleton.cs
	author:		Leo

	purpose:	单例基类
                包括
                普通单例基类
                Mono单例基类  具备Mono生命周期，最典型就是Update

*********************************************************************/

using UnityEngine;

namespace CoffeeBean
{
    /// <summary>
    /// 单例基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class CSingleton<T> where T : class, new()
    {
        /// <summary>
        /// 泛型单例实例
        /// </summary>
        protected static T m_Inst = null;

        /// <summary>
        /// 单例
        /// </summary>
        public static T Inst
        {
            get
            {
                if ( m_Inst == null )
                {
                    m_Inst = new T();
                }

                return m_Inst;
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
    public abstract class CSingletonMono<T> : MonoBehaviour where T : MonoBehaviour
    {
        /// <summary>
        /// 类的实例
        /// </summary>
        protected static T m_Inst = null;

        /// <summary>
        /// 单例
        /// </summary>
        public static T Inst
        {
            get
            {
                // 没有找到实例
                if ( m_Inst == null )
                {
                    string m_instanceName = typeof ( T ).Name;
                    GameObject temp = new GameObject ( m_instanceName );
                    m_Inst = temp.AddComponent<T>();
                    DontDestroyOnLoad( temp );
                }

                return m_Inst;
            }
        }
    }
}