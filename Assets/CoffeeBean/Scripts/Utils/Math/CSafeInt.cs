/********************************************************************
	All Right Reserved By Leo
	Created:	2019/01/08 10:35
	Filee: 	    CEncryptInt.cs
	Author:		Leo

	Purpose:	加密整形数字
                可作为游戏中基本的类型使用
                提供数据防篡改的功能
*********************************************************************/

using System;
using System.Text;
using UnityEngine;

namespace CoffeeBean
{
    /// <summary>
    /// 字符串整形
    /// 一个自创的加密算法
    /// 使用加密后的字符串来存储整形数字
    /// 内存中只有字符串值
    /// 代码中可以和int一样使用
    /// 可以有效防止金手指查询定位
    /// </summary>
    public class CSafeInt
    {
        private string m_Data = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Num"></param>
        public CSafeInt( int Num )
        {
            Encryption( Num );
        }

        /// <summary>
        /// 得到加密后的字符串
        /// </summary>
        public string String => m_Data;

        /// <summary>
        /// 得到原文
        /// </summary>
        /// <returns></returns>
        public int Value => Decryption();

        /// <summary>
        /// 重置归0
        /// </summary>
        public void Reset()
        {
            Encryption( 0 );
        }

        /// <summary>
        /// 解密一个数字
        /// </summary>
        /// <param name="number"></param>
        private int Decryption()
        {
            var value = 0;
            for ( int i = 0; i < m_Data.Length; i++ )
            {
                if ( (byte)m_Data[i] >= 97 )
                {
                    return value;
                }

                value <<= 4;
                var data = m_Data[i] - 64;
                value |= data;
            }
            return value;
        }

        /// <summary>
        /// 加密一个数字
        /// </summary>
        /// <param name="number"></param>
        private void Encryption( int number )
        {
            var sb = new StringBuilder(8);
            while ( number > 0 )
            {
                var temp = number & 0x0F;
                temp += 64;
                number >>= 4;
                sb.Append( (char)temp );
            }

            // 随机补充小写字母
            while ( sb.Length < 8 )
            {
                sb.Append( (char)UnityEngine.Random.Range( 97, 123 ) );
            }

            m_Data = sb.ToString();
        }

        #region Operation

        public static implicit operator CSafeInt( int number )
        {
            return new CSafeInt( number );
        }

        public static CSafeInt operator -( CSafeInt lhs, int rhs )
        {
            CSafeInt v = new CSafeInt ( lhs.Decryption() - rhs );
            return v;
        }

        public static CSafeInt operator -( CSafeInt lhs, CSafeInt rhs )
        {
            CSafeInt v = new CSafeInt ( lhs.Decryption() - rhs.Decryption() );
            return v;
        }

        public static CSafeInt operator --( CSafeInt lhs )
        {
            lhs.Encryption( lhs.Decryption() - 1 );
            return lhs;
        }

        public static bool operator !=( CSafeInt lhs, int rhs )
        {
            return lhs.Decryption() != rhs;
        }

        public static bool operator !=( CSafeInt lhs, CSafeInt rhs )
        {
            return lhs.Decryption() != rhs.Decryption();
        }

        public static CSafeInt operator %( CSafeInt lhs, int rhs )
        {
            CSafeInt v = new CSafeInt ( lhs.Decryption() % rhs );
            return v;
        }

        public static CSafeInt operator %( CSafeInt lhs, CSafeInt rhs )
        {
            CSafeInt v = new CSafeInt ( lhs.Decryption() % rhs.Decryption() );
            return v;
        }

        public static CSafeInt operator &( CSafeInt lhs, int rhs )
        {
            CSafeInt v = new CSafeInt ( lhs.Decryption() & rhs );
            return v;
        }

        public static CSafeInt operator &( CSafeInt lhs, CSafeInt rhs )
        {
            CSafeInt v = new CSafeInt ( lhs.Decryption() & rhs.Decryption() );
            return v;
        }

        public static CSafeInt operator *( CSafeInt lhs, int rhs )
        {
            CSafeInt v = new CSafeInt ( lhs.Decryption() * rhs );
            return v;
        }

        public static CSafeInt operator *( CSafeInt lhs, CSafeInt rhs )
        {
            CSafeInt v = new CSafeInt ( lhs.Decryption() * rhs.Decryption() );
            return v;
        }

        public static CSafeInt operator /( CSafeInt lhs, int rhs )
        {
            CSafeInt v = new CSafeInt ( lhs.Decryption() / rhs );
            return v;
        }

        public static CSafeInt operator /( CSafeInt lhs, CSafeInt rhs )
        {
            CSafeInt v = new CSafeInt ( lhs.Decryption() / rhs.Decryption() );
            return v;
        }

        public static CSafeInt operator ^( CSafeInt lhs, int rhs )
        {
            CSafeInt v = new CSafeInt ( lhs.Decryption() ^ rhs );
            return v;
        }

        public static CSafeInt operator ^( CSafeInt lhs, CSafeInt rhs )
        {
            CSafeInt v = new CSafeInt ( lhs.Decryption() ^ rhs.Decryption() );
            return v;
        }

        public static CSafeInt operator |( CSafeInt lhs, int rhs )
        {
            CSafeInt v = new CSafeInt ( lhs.Decryption() | rhs );
            return v;
        }

        public static CSafeInt operator |( CSafeInt lhs, CSafeInt rhs )
        {
            CSafeInt v = new CSafeInt ( lhs.Decryption() | rhs.Decryption() );
            return v;
        }

        public static CSafeInt operator +( CSafeInt lhs, int rhs )
        {
            CSafeInt v = new  CSafeInt ( lhs.Decryption() + rhs );
            return v;
        }

        public static CSafeInt operator +( CSafeInt lhs, CSafeInt rhs )
        {
            CSafeInt v = new CSafeInt ( lhs.Decryption() + rhs.Decryption() );
            return v;
        }

        public static CSafeInt operator ++( CSafeInt lhs )
        {
            lhs.Encryption( lhs.Decryption() + 1 );
            return lhs;
        }

        public static bool operator <( CSafeInt lhs, int rhs )
        {
            return lhs.Decryption() < rhs;
        }

        public static bool operator <( CSafeInt lhs, CSafeInt rhs )
        {
            return lhs.Decryption() < rhs.Decryption();
        }

        public static CSafeInt operator <<( CSafeInt lhs, int rhs )
        {
            CSafeInt v = new CSafeInt ( lhs.Decryption() << rhs );
            return v;
        }

        public static bool operator <=( CSafeInt lhs, int rhs )
        {
            return lhs.Decryption() <= rhs;
        }

        public static bool operator <=( CSafeInt lhs, CSafeInt rhs )
        {
            return lhs.Decryption() <= rhs.Decryption();
        }

        public static bool operator ==( CSafeInt lhs, int rhs )
        {
            return lhs.Decryption() == rhs;
        }

        public static bool operator ==( CSafeInt lhs, CSafeInt rhs )
        {
            return lhs.Decryption() == rhs.Decryption();
        }

        public static bool operator >( CSafeInt lhs, int rhs )
        {
            return lhs.Decryption() > rhs;
        }

        public static bool operator >( CSafeInt lhs, CSafeInt rhs )
        {
            return lhs.Decryption() > rhs.Decryption();
        }

        public static bool operator >=( CSafeInt lhs, int rhs )
        {
            return lhs.Decryption() >= rhs;
        }

        public static bool operator >=( CSafeInt lhs, CSafeInt rhs )
        {
            return lhs.Decryption() >= rhs.Decryption();
        }

        public static CSafeInt operator >>( CSafeInt lhs, int rhs )
        {
            CSafeInt v = new CSafeInt ( lhs.Decryption() >> rhs );
            return v;
        }

        public override bool Equals( System.Object obj )
        {
            if ( obj == null )
            {
                return false;
            }

            if ( obj is CSafeInt )
            {
                return Decryption() == ( obj as CSafeInt ).Decryption();
            }

            if ( obj is int )
            {
                return Decryption() == (int)obj;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Decryption();
        }

        public override string ToString()
        {
            return Decryption().ToString();
        }

        #endregion Operation
    }
}