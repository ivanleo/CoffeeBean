﻿
using UnityEngine;

namespace CoffeeBean
{
    /// <summary>
    /// 自动垃圾回收
    /// 每隔60帧垃圾回收1次
    /// </summary>
    public class CUtilAutoGC : CSingletonMono<CUtilAutoGC>
    {
        //开始自动垃圾回收
        public void Begin()
        {

        }

        /// <summary>
        /// 多少帧自动垃圾回收
        /// </summary>
        public const int GarbageCollectorFrameCount = 60;

        /// <summary>
        /// 多少帧清理未使用资源
        /// </summary>
        public const int UnLoadUseResource = 3600;
        private void Update()
        {
            if ( Time.frameCount % GarbageCollectorFrameCount == 0 )
            {
                System.GC.Collect();
            }

            if ( Time.frameCount % UnLoadUseResource == 0 )
            {
                Resources.UnloadUnusedAssets();
            }
        }
    }

}