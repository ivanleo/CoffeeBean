/********************************************************************
   All Right Reserved By Leo
   Created:    2020/6/16 8:48:41
   File: 	   CRes.cs
   Author:     Leo

   Purpose:    资源管理类
*********************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CoffeeBean
{
    /// <summary>
    /// 资源管理类
    /// </summary>
    public class CRes
    {
        /// <summary>
        /// 是否已初始化
        /// </summary>
        public static bool HasInit { get; private set; }

        /// <summary>
        /// 检查资源更新
        /// </summary>
        /// <param name="tags"></param>
        public static async Task CheckUpdate( List<object> tags )
        {
            CLOG.I( "res", "---------- check update ----------" );

            var downloadsize = Addressables.GetDownloadSizeAsync(tags);
            await downloadsize.Task;
            var needDownSize = downloadsize.Result;
            CLOG.I( "res", $"need download:{needDownSize}" );

            Addressables.Release( downloadsize );
            CLOG.I( "res", $"release downloadsize in frame:{Time.frameCount}" );

            if ( needDownSize > 0 )
            {
                CLOG.I( "res", "---------- start download ----------" );
                var downloader = Addressables.DownloadDependenciesAsync(tags,Addressables.MergeMode.None);
                await downloader.Task;
                CLOG.I( "res", $"finish download:{downloader.Result.ToString()}" );
                Addressables.Release( downloader );
                CLOG.I( "res", $"release downloader in frame:{Time.frameCount}" );
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public static async Task Init()
        {
            if ( HasInit )
                return;

            var init = Addressables.InitializeAsync();
            await init.Task;
            Addressables.Release( init );

            HasInit = true;
        }
    }
}