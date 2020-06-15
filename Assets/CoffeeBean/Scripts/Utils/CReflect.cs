/********************************************************************
	All Right Reserved By Leo
	Created:	2019/01/09 23:08
	File base:	CFunction.cs
	author:		Leo

	purpose:	反射工具类
                通过反射来实现字符串函数调用
*********************************************************************/

using System;
using System.Reflection;

using UnityEngine.Assertions;

namespace CoffeeBean
{
    /// <summary>
    /// 反射工具类
    /// </summary>
    public static class CReflect
    {
        /// <summary>
        /// 反射实例化
        /// 通过无参构造函数创建
        /// </summary>
        /// <param name="ObjectType">目标类型</param>
        /// <returns></returns>
        public static object CreateInstance ( Type ObjectType )
        {
            return Activator.CreateInstance ( ObjectType );
        }

        /// <summary>
        /// 反射实例化
        /// 通过无参构造函数创建
        /// </summary>
        /// <param name="ObjectType">目标类型</param>
        /// <returns></returns>
        public static object CreateInstance ( Type ObjectType, params object[] parms )
        {
            return Activator.CreateInstance ( ObjectType, parms );
        }

        /// <summary>
        /// 在一个对象身上通过字符串调用公开的方法
        /// </summary>
        /// <param name="Target">目标对象</param>
        /// <param name="FunctionName">方法名</param>
        /// <param name="Param">参数列表</param>
        /// <returns>函数的返回值</returns>
        public static object CallFunction ( object Target, string FunctionName, params object[] Param )
        {
            Assert.IsNull ( FunctionName );
            Assert.IsNull ( Target );

            // 获取方法
            MethodInfo func = Target.GetType().GetMethod ( FunctionName, BindingFlags.Public );
            Assert.IsNull ( func );

            return func.Invoke ( Target, Param );
        }

        /// <summary>
        /// 在一个对象身上通过字符串调用公开的泛型方法
        /// 支持单个泛型
        /// 形如函数
        /// Test<T>(.......)
        /// 的可以使用本方法反射调用
        /// </summary>
        /// <param name="Target">调用目标</param>
        /// <param name="FunctionName">方法名称</param>
        /// <param name="GenericType">泛型数组类型</param>
        /// <param name="Param">参数列表</param>
        /// <returns>方法的返回值</returns>
        public static object CallSingleGenericFunction ( object Target, string FunctionName, Type GenericType, params object[] Param )
        {
            Assert.IsNull ( FunctionName );
            Assert.IsNull ( Target );
            Assert.IsNull ( GenericType );

            // 获取方法
            MethodInfo func = Target.GetType().GetMethod ( FunctionName, BindingFlags.Public );
            Assert.IsNull ( func );

            func = func.MakeGenericMethod ( GenericType );
            return func.Invoke ( Target, Param );
        }

        /// <summary>
        /// 在一个对象身上通过字符串调用公开的泛型方法
        /// 支持多泛型
        /// 形如函数
        /// Test<T1,T2...>(.......)
        /// 的可以使用本方法反射调用
        /// </summary>
        /// <param name="Target">调用目标</param>
        /// <param name="FunctionName">方法名称</param>
        /// <param name="GenericType">泛型数组类型</param>
        /// <param name="Param">参数列表</param>
        /// <returns>方法的返回值</returns>
        public static object CallMutiGenericFunction ( object Target, string FunctionName, Type[] GenericType, params object[] Param )
        {
            Assert.IsNull ( FunctionName );
            Assert.IsNull ( Target );
            Assert.IsNull ( GenericType );

            // 获取方法
            MethodInfo func = Target.GetType().GetMethod ( FunctionName, BindingFlags.Public );
            Assert.IsNull ( func );

            func = func.MakeGenericMethod ( GenericType );
            return func.Invoke ( Target, Param );
        }


        /// <summary>
        /// 在一个类型里上通过字符串调用公开的静态方法
        /// </summary> <param name="FunctionName">方法名</param>
        /// <param name="TargetClass">目标类型</param>
        /// <param name="FunctionName">静态函数名</param>
        /// <param name="Param">参数列表</param>
        /// <returns>函数的返回值</returns>
        public static object CallStaticFunction ( Type TargetClass, string FunctionName, params object[] Param )
        {
            Assert.IsNull ( FunctionName );
            Assert.IsNull ( TargetClass );

            // 获取方法
            MethodInfo func = TargetClass.GetMethod ( FunctionName, BindingFlags.Public | BindingFlags.Static );
            Assert.IsNull ( func );

            return func.Invoke ( null, Param );
        }

        /// <summary>
        /// 在一个类型里上通过字符串调用公开的静态泛型方法
        /// 只支持一个泛型类型
        /// 形如函数
        /// void Test<T>(.......)
        /// 的可以使用本方法反射调用
        /// </summary>
        /// <param name="TargetClass">目标类型</param>
        /// <param name="FunctionName">方法名</param>
        /// <param name="GenericType">泛型类型</param>
        /// <param name="Param">参数列表</param>
        /// <returns>函数的返回值</returns>
        public static object CallStaticSingleGenericFunction ( Type TargetClass, string FunctionName, Type GenericType, params object[] Param )
        {
            Assert.IsNull ( FunctionName );
            Assert.IsNull ( TargetClass );
            Assert.IsNull ( GenericType );

            // 获取方法
            MethodInfo func = TargetClass.GetMethod ( FunctionName, BindingFlags.Public | BindingFlags.Static );

            Assert.IsNull ( func );

            func = func.MakeGenericMethod ( GenericType );
            return func.Invoke ( null, Param );
        }

        /// <summary>
        /// 在一个类型里上通过字符串调用公开的静态泛型方法
        /// 只支持多个泛型类型
        /// 形如函数
        /// void Test<T1, T2>(.......)
        /// 的可以使用本方法反射调用
        /// </summary>
        /// <param name="TargetClass">目标类型</param>
        /// <param name="FunctionName">方法名</param>
        /// <param name="GenericType">泛型类型</param>
        /// <param name="Param">参数列表</param>
        /// <returns>函数的返回值</returns>
        public static object CallStaticMutiGenericFunction ( Type TargetClass, string FunctionName, Type[] GenericType, params object[] Param )
        {
            Assert.IsNull ( FunctionName );
            Assert.IsNull ( TargetClass );
            Assert.IsNull ( GenericType );

            // 获取方法
            MethodInfo func = TargetClass.GetMethod ( FunctionName, BindingFlags.Public | BindingFlags.Static );

            Assert.IsNull ( func );

            func = func.MakeGenericMethod ( GenericType );
            return func.Invoke ( null, Param );
        }
    }
}