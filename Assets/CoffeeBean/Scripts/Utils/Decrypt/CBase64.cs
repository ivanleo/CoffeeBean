using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace CoffeeBean
{
    /// <summary>
    /// Base64 图片
    /// </summary>
    public class CBase64
    {
        /// <summary>
        /// 16x16白色小球
        /// </summary>
        public const string BASE64_Circle_16x16 = @"iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAALEgAACxIB0t1+/AAAABZ0RVh0Q3JlYXRpb24gVGltZQAxMS8xMy8xOTsS4/EAAAAcdEVYdFNvZnR3YXJlAEFkb2JlIEZpcmV3b3JrcyBDUzbovLKMAAAAb0lEQVQ4jc2TwQ0AERBFZ1WgRKUpYUugA6Xo4O3FJMs6iHHYlziI+Q/JjAAyrADcfLnbWVf/3nigTIIjpdV2Ag/UhbBSVaKClZtnLxH98y5BgGQQpAtADDhL+D+CbMhnJyLRIIhHGulIK5uHaXucH284VeiHmF9nAAAAAElFTkSuQmCC";

        /// <summary>
        /// 2x2矩形小方块
        /// </summary>
        public const string BASE64_Rect_2x2 = @"iVBORw0KGgoAAAANSUhEUgAAAAIAAAACCAYAAABytg0kAAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAALEgAACxIB0t1+/AAAABZ0RVh0Q3JlYXRpb24gVGltZQAxMS8xMy8xOTsS4/EAAAAcdEVYdFNvZnR3YXJlAEFkb2JlIEZpcmV3b3JrcyBDUzbovLKMAAAAFUlEQVQImWP8////fwYGBgYmBigAAD34BADZfIBGAAAAAElFTkSuQmCC";

        /// <summary>
        /// 缓存的资源
        /// </summary>
        private static CCache<Sprite> cache = new CCache< Sprite>();

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string BASE64Decrypt( string target )
        {
            return Encoding.Default.GetString( Convert.FromBase64String( target ) );
        }

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string BASE64Encrypt( string source )
        {
            return Convert.ToBase64String( Encoding.Default.GetBytes( source ) );
        }

        /// <summary>
        /// 生成的图片尺寸
        /// </summary>
        /// <param name="base64Str"></param>
        public static Sprite Base64ToSprite( string base64Str )
        {
            if ( cache.Has( base64Str ) )
            {
                return cache.Get( base64Str );
            }

            byte[]    bytes  = Convert.FromBase64String(base64Str);
            Texture2D tex2D  = new Texture2D(1024, 1024);
            tex2D.LoadImage( bytes );
            Sprite sp = Sprite.Create(tex2D, new Rect(0, 0, tex2D.width, tex2D.height), new Vector2(0.5f, 0.5f));

            // 缓存精灵
            cache.Add( base64Str, sp );
            return sp;
        }
    }
}