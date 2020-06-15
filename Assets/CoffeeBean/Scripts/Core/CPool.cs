using System;
using System.Collections.Generic;

namespace CoffeeBean
{
    /// <summary>
    /// 简易对象池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CPool<T>
    {
        /// <summary>
        /// 创建方法
        /// </summary>
        private Func<T> _CreateFunc;

        /// <summary>
        /// 对象池
        /// </summary>
        private Stack<T> _Pool;

        /// <summary>
        /// 私有构造
        /// 请通过 CreatePoolWithCreateFunc 创建池子
        /// </summary>
        private CPool()
        {
            this._Pool = new Stack<T>();
        }

        /// <summary>
        /// 当前对象池可用对象数
        /// </summary>
        public int Size { get => _Pool.Count; }

        /// <summary>
        /// 创建池子
        /// </summary>
        /// <returns></returns>
        public static CPool<T> CreatePoolWithCreateFunc( Func<T> createFunc )
        {
            if ( createFunc == null )
            {
                CLOG.E( "pool", "the pool create func is null" );
                return null;
            }
            var pool = new CPool<T>();
            pool._CreateFunc = createFunc;
            return pool;
        }

        /// <summary>
        /// 清理对象池
        /// </summary>
        public void Clear()
        {
            this._Pool.Clear();
        }

        /// <summary>
        /// 从对象池获得对象
        /// </summary>
        /// <returns></returns>
        public T GetObject()
        {
            if ( _Pool.Count == 0 )
            {
                var obj = _CreateFunc.Invoke();
                return obj;
            }

            return _Pool.Pop();
        }

        /// <summary>
        /// 返回到池子
        /// </summary>
        public void Release( T obj )
        {
            this._Pool.Push( obj );
        }
    }
}