using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using UnityEngine;

namespace CoffeeBean
{
    /// <summary>
    /// 多重UI，每次创建都会创建一个新的UI实例
    /// </summary>
    public class CUIMuti<T> : CUIBase where T : CUIMuti<T>
    {
        /// <summary>
        /// UI实例
        /// </summary>
        private static List<T> _uinsts = new List<T>();

        /// <summary>
        /// UI序号
        /// </summary>
        private static int uindex = 0;

        /// <summary>
        /// UI实例
        /// </summary>
        public static List<T> UInsts => _uinsts;

        /// <summary>
        /// 创建UI
        /// </summary>
        /// <param name="parent">父节点，为空则自动创建到画布下</param>
        /// <returns></returns>
        public static T CreateUI( Transform parent = null )
        {
            // 解析特性
            ParseBindInfo<T>();

            // 创建UI
            var ui = CreateUIObject<T>( bindInfo.Prefab, parent );
            _uinsts.Add( ui );
            var uiname = CFile.GetFileName( bindInfo.Prefab );
            ui.gameObject.name = $"{uiname}_{uindex++}";
            return ui;
        }

        /// <summary>
        /// <para>创建UI</para>
        /// <para>包含动画</para>
        /// <para>此操作是一个异步过程</para>
        /// <para>await UI_Test.CreateUIWithAnim( );</para>
        /// <para>将在动画执行完毕后返回调用点</para>
        /// </summary>
        /// <param name="InAnim">入场动画</param>
        /// <param name="parent">父节点，为空则自动创建到画布下</param>
        public static async Task<T> CreateUIWithAnim( CUIAnimIn InAnim, Transform parent = null )
        {
            var ui = CreateUI(parent);

            // 播放入场动画
            await CUIAnimtaion.PlayInAnim( ui.rectTransform, InAnim );

            return ui;
        }

        /// <summary>
        /// 干掉所有UI
        /// </summary>
        /// <param name="inOnFrame">是否在一帧内干掉所有</param>
        public static void DestroyAllUI( bool inOnFrame = false )
        {
            // 需要在一帧内干掉所有
            if ( inOnFrame )
            {
                for ( int i = 0; i < _uinsts.Count; i++ )
                {
                    DestroyImmediate( _uinsts[i].gameObject );
                }
            }
            else
            {
                for ( int i = 0; i < _uinsts.Count; i++ )
                {
                    Destroy( _uinsts[i].gameObject );
                }
            }

            _uinsts.Clear();
        }

        /// <summary>
        /// 干掉UI界面
        /// 需要把指定界面传递进来
        /// </summary>
        /// <param name="target">要干掉的目标</param>
        public static void DestroyUI( T target )
        {
            if ( target != null && target.gameObject != null )
            {
                if ( _uinsts.Contains( target ) )
                {
                    _uinsts.Remove( target );
                    Destroy( target.gameObject );
                }
            }
        }

        /// <summary>
        /// <para>干掉UI界面</para>
        /// <para>包含动画</para>
        /// <para>此操作是一个异步过程</para>
        /// <para>await UI_Test.DestroyUIWithAnim(this );</para>
        /// <para>将在动画执行完毕后返回调用点</para>
        /// </summary>
        /// <param name="target">要干掉的目标</param>
        /// <param name="OutAnim">退出动画</param>
        public static async Task DestroyUIWithAnim( CUIAnimOut OutAnim, T target )
        {
            if ( target != null && target.gameObject != null )
            {
                // 播完动画再销毁
                await CUIAnimtaion.PlayOutAnim( target.rectTransform, OutAnim );
                DestroyUI( target );
            }
        }

        /// <summary>
        /// 是否存在至少一个多重UI
        /// </summary>
        /// <returns></returns>
        public static bool HasUI()
        {
            return _uinsts != null && _uinsts.Count > 0;
        }

        /// <summary>
        /// 销毁时
        /// </summary>
        protected void OnDestroy()
        {
            if ( _uinsts.Contains( this as T ) )
            {
                Destroy( this );
                _uinsts.Remove( this as T );
            }
        }
    }
}