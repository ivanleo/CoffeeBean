#pragma warning disable 1591
using UnityEngine;

namespace CoffeeBean
{
    /// <summary>
    /// 大小类型
    /// </summary>
    public struct CSize
    {
        /// <summary>
        /// 宽度
        /// </summary>
        public float Width;

        /// <summary>
        /// 高度
        /// </summary>
        public float Height;

        public static CSize operator * ( CSize Source, float Num )
        {
            CSize ret = new CSize();
            ret.Width = Source.Width * Num;
            ret.Height = Source.Height * Num;
            return ret;
        }

        public static CSize operator / ( CSize Source, float Num )
        {
            CSize ret = new CSize();
            ret.Width = Source.Width / Num;
            ret.Height = Source.Height / Num;
            return ret;
        }
    }

    /// <summary>
    /// 位置类型，负责定义一个物体的位置，旋转，缩放
    /// </summary>
    public struct CPos
    {
        /// <summary>
        /// 位置
        /// </summary>
        public Vector3 m_Position;

        /// <summary>
        /// 旋转四元数
        /// </summary>
        public Quaternion m_Rotation;

        /// <summary>
        /// 本地缩放
        /// </summary>
        public Vector3 m_LocalScale;

    }

}
