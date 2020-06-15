using UnityEngine;

namespace CoffeeBean
{
    /// <summary>
    /// 一次性粒子自动销毁器
    /// 自动检测粒子的播放状态
    /// 一次性粒子在播放结束后自动销毁自身
    /// </summary>
    [RequireComponent( typeof( ParticleSystem ) )]
    public class COnceParticleAutoDestroy : MonoBehaviour
    {
        /// <summary>
        /// 粒子系统
        /// </summary>
        private ParticleSystem ps;

        /// <summary>
        /// 播放状态
        /// </summary>
        public bool IsPlaying { get; private set; } = false;

        private void Awake()
        {
            ps = GetComponent<ParticleSystem>();
            if ( ps.main.loop )
            {
                CLOG.W( "the particle is loop!! so it won't destroy on play end!" );
                Destroy( this );
            }
        }

        private void Update()
        {
            if ( ps.isPlaying && !IsPlaying )
            {
                IsPlaying = true;
                return;
            }

            if ( IsPlaying && ps.isStopped )
            {
                Destroy( gameObject );
            }
        }
    }
}