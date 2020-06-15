/********************************************************************
	All Right Reserved By Leo
	Created:	2019/01/09 17:27
	File base:	CExpandImage.cs
	author:		Leo

	purpose:	图片扩展
*********************************************************************/

using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace CoffeeBean
{
    /// <summary>
    /// <para>Image的URL Loader</para>
    /// <para>加载的图片会自动以MD5的形式保存本地</para>
    /// <para>下次加载会直接读取本地文件提高速度</para>
    /// <para>例如</para>
    /// <code>
    ///     <para>Image image = .......</para>
    ///     <para>image.LoadURLImage( "http://61.183.69.235:7001/hx/uploadFiles/default/subject/20151019081139869433.jpg" );</para>
    /// </code>
    /// </summary>
    public static class CExpandImage
    {
        /// <summary>
        /// Image this扩展，读取图片
        /// </summary>
        /// <param name="image">Imagte组件</param>
        /// <param name="URL">网络URL头像地址</param>
        /// <param name="autoSize">自动尺寸，为true,image的尺寸会缩放为图片尺寸，false 图片缩放为目标尺寸</param>
        /// <param name="progress">加载进度委托</param>
        public static async Task LoadHttpImage( this Image image, string URL, bool autoSize = false, Action<UnityWebRequest> progress = null )
        {
            if ( URL.IsNullOrEmpty() )
            {
                return;
            }

            try
            {
                // 得到文件类型
                string TargetFileType = CFile.GetFileType ( URL );

                // 得到要生成的文件名
                string TargetFileName = URL.MD5() + "." + TargetFileType;

                // 本地文件名，用于判断文件是否存在
                string LoacalFileName = CApp.Texture_Path + TargetFileName;

                // 判断本地文件是否存在
                FileInfo FI = new FileInfo ( LoacalFileName );

                // 加载完成的图片
                Texture2D loadedTexture = null;

                // 本地文件存在，则自动加载本地文件
                if ( FI.Exists )
                {
                    // 加载网络头像
                    loadedTexture = await CHttp.LoadTexture( LoacalFileName, progress );
                }
                else  // 本地文件不存在，则读取远程URL
                {
                    loadedTexture = await CHttp.LoadTexture( URL, progress );
                }

                // 加载成功，但是头像已销毁，啥也不做
                if ( image == null )
                {
                    return;
                }

                // 未加载到头像资源
                if ( loadedTexture == null )
                {
                    return;
                }

                // 自动尺寸的设置
                if ( autoSize )
                {
                    image.rectTransform.sizeDelta = new Vector2( loadedTexture.width, loadedTexture.height );
                }

                // 设置Image图片精灵
                image.sprite = Sprite.Create( loadedTexture, new Rect( 0, 0, loadedTexture.width, loadedTexture.height ), Vector2.zero );

                // 本地文件不存在，加载完毕后自动保存
                if ( !FI.Exists )
                {
                    // 得到字节流
                    byte[] ImageData = null;

                    if ( TargetFileType == "jpg" || TargetFileType == "jpeg" )
                    {
                        ImageData = loadedTexture.EncodeToJPG();
                    }
                    else if ( TargetFileType == "png" )
                    {
                        ImageData = loadedTexture.EncodeToPNG();
                    }

                    // 保存路径
                    CLOG.I( "read http remote data complete , Start save image to {0}!", LoacalFileName );

                    // 保存文件
                    if ( ImageData != null )
                    {
                        File.WriteAllBytes( LoacalFileName, ImageData );
                    }
                    else
                    {
                        CLOG.E( "Write file {0} error!", LoacalFileName );
                    }
                }
            }
            catch ( Exception ex )
            {
                CLOG.I( "**********  don't warry it's OK  ************" );
                CLOG.E( ex.ToString() );
                CLOG.I( "*********************************************" );
            }
        }

        /// <summary>
        /// 设置透明度
        /// </summary>
        /// <param name="target"></param>
        /// <param name="Alpha"></param>
        public static void SetAlpha( this Graphic target, float Alpha )
        {
            var color = target.color;
            color.a = Alpha;
            target.color = color;
        }
    }
}