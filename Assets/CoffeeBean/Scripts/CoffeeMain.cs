/********************************************************************
	All Right Reserved By Leo
	Created:	2019/01/09 23:24
	File base:	CMain.cs
	author:		Leo

	purpose:	Coffee Bean 框架入口
*********************************************************************/

using UnityEngine;

namespace CoffeeBean
{
    /// <summary>
    /// 框架主入口
    /// </summary>
    public static class CoffeeMain
    {
        /// <summary>
        /// 是否已经初始化
        /// </summary>
        private static bool HasInit = false;

        /// <summary>
        /// 设计分辨率
        /// </summary>
        public static Vector2 DesignResolution { get; private set; }

        /**********************************************************************************/
        /*                                    游戏入口                                    */
        /**********************************************************************************/

        /// <summary>
        /// 入口
        /// 初始化框架
        /// Init在游戏场景加载前执行
        /// </summary>
        public static void Init()
        {
            if ( HasInit )
            {
                return;
            }

            Debug.Log( "init [CoffeeBean]" );

            // 启动应用程序
            CApp.Inst.Init();

            // 主循环
            CApp.Inst.Looper += MainUpdate;

            // 激活LOG系统
            CLOG.Init();

            // 初始化场景管理器
            CSceneManager.Init();

#if DEBUG
            // 启动帧率显示
            CFPS.Toggle();
#endif

            HasInit = true;
        }

        /// <summary>
        /// 设置设计分辨率
        /// </summary>
        /// <param name="designX"></param>
        /// <param name="designY"></param>
        public static void SetDesignSize( float designX, float designY )
        {
            // 设计分辨率
            DesignResolution = new Vector2( designX, designY );
            // 设计宽高比
            float _DesignWHRatio = designX / designY;
            // 设计宽高比下的摄像机尺寸
            float _DesignSize = Mathf.Max( designX, designY ) / 200f;
            // 真实宽高比
            float _RealWHRatio = ( float ) Screen.width / ( float ) Screen.height;
            // 真实宽高比下的摄像机尺寸
            CSceneManager.CameraSize = _DesignSize / _RealWHRatio * _DesignWHRatio;
        }

        /// <summary>
        /// 框架主循环
        /// </summary>
        private static void MainUpdate()
        {
            if ( Input.GetKeyDown( KeyCode.F12 ) )
            {
                CFPS.Toggle();
            }
        }
    }
}