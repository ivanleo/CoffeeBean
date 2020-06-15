using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CoffeeBean
{
    /// <summary>
    /// UI缓存
    /// </summary>
    public static class CUICacher
    {
        /// <summary>
        /// 是否已缓存
        /// </summary>
        private static bool hasCached = false;

        /// <summary>
        /// UI缓存
        /// </summary>
        private static Dictionary<string, GameObject> UIcaches;

        static CUICacher()
        {
            UIcaches = new Dictionary<string, GameObject>();
        }

        /// <summary>
        /// 通过标签缓存资源
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="folderstr"></param>
        public static async Task CacheAllUI()
        {
            if ( hasCached )
            {
                return;
            }

            var ao = Addressables.LoadAssetsAsync<GameObject>( "ui", null );
            await ao.Task;
            var reses = ao.Result;
            for ( int i = 0; i < reses.Count; i++ )
            {
                var key = $"Assets/Prefab/UI/{reses[i].name}.prefab";
                UIcaches.Add( key, reses[i] );
                CLOG.I( "ui", $"cached ui:{key}" );
            }
            Addressables.Release( ao );
        }

        /// <summary>
        /// 获取缓存的UI
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static GameObject GetCacheUISource( string key )
        {
            if ( UIcaches.ContainsKey( key ) )
            {
                return UIcaches[key];
            }

            return null;
        }
    }
}