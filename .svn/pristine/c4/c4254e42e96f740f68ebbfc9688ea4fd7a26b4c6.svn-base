/********************************************************************
    created:    2017/10/26
    created:    26:10:2017   20:53
    filename:   D:\Work\NanJingMaJiang\trunk\Project\CoffeeBean\Utils\CUtilAssert.cs
    file path:  D:\Work\NanJingMaJiang\trunk\Project\CoffeeBean\Utils
    file base:  CUtilAssert
    file ext:   cs
    author:     Leo

    purpose:    断言工具类
*********************************************************************/

using UnityEngine.Assertions;

namespace CoffeeBean
{
    /// <summary>
    /// 断言类，条件满足中断程序
    /// </summary>
    public static class CAssert
    {
        /// <summary>
        /// 条件满足就中断条件
        /// </summary>
        /// <param name="condition">条件</param>
        public static void AssertIfFalse( bool condition )
        {
            if( condition == false )
            {
                CLOG.E( "Assest.IsTrue Failed" );
            }
            Assert.IsTrue( condition );
        }

        /// <summary>
        /// 条件满足就中断条件，抛出文字内容
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="message">消息</param>
        public static void AssertIfFalse( bool condition, string message )
        {
            if( condition == false )
            {
                CLOG.E( message );
            }

            Assert.IsTrue( condition, message );
        }

        /// <summary>
        /// 为空就中断条件
        /// </summary>
        /// <param name="obj">条件对象</param>
        public static void AssertIfNull( object obj )
        {
            if( obj == null )
            {
                CLOG.E( "{0} is null", obj.ToString() );
            }

            Assert.IsNotNull( obj );
        }

        /// <summary>
        /// 为空就中断条件
        /// </summary>
        /// <param name="obj">条件</param>
        /// /// <param name="message">消息</param>
        public static void AssertIfNull( object obj, string message )
        {
            if( obj == null )
            {
                CLOG.E( "{0} is null msg:{1}", obj.ToString(), message );
            }
            Assert.IsNotNull( obj, message );
        }

    }
}
