using UnityEngine;
namespace CoffeeBean
{
    /// <summary>
    /// 调试器
    /// </summary>
    public class CFPS : CSingletonMono<CFPS>
    {
        //每隔2秒刷新一次
        private float m_FPSInteval = 1.0f;

        //已用时
        private float m_TimePassed = 0f;

        //过去帧数
        private int m_FrameCount = 0;

        //显示帧率
        private string m_FPS = null;

        /// <summary>
        /// 开始
        /// </summary>
        public void Begin()
        {
        }

        /// <summary>
        /// 更新操作
        /// </summary>
        private void Update()
        {
            m_FrameCount ++;
            m_TimePassed += Time.deltaTime;

            if( m_TimePassed > m_FPSInteval )
            {
                m_FPS = ( m_FrameCount / m_TimePassed ).ToString( "0.00" );

                m_TimePassed = 0f;
                m_FrameCount = 0;
            }
        }

        /// <summary>
        /// GUI绘制
        /// </summary>
        private void OnGUI()
        {
            GUI.Box( new Rect( 0, 0, 100, 24 ), "FPS: " + m_FPS );
        }
    }
}