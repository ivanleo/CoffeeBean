using UnityEngine;

namespace CoffeeBean
{
    /// <summary>
    /// 摄像机显示模式
    /// </summary>
    public enum ECameraShowType
    {
        /// <summary>
        /// 显示所有
        /// </summary>
        SHOW_ALL,
        /// <summary>
        /// 没有黑边
        /// </summary>
        NO_BLACK
    }

    /// <summary>
    /// 自动相机
    /// </summary>
    [RequireComponent ( typeof ( Camera ) )]
    public class CUtilAuto2DCamera: MonoBehaviour
    {
        /// <summary>
        /// 设计分辨率
        /// </summary>
        [SerializeField]
        private Vector2 DesignSize = new Vector2 ( 720f, 1280f );

        /// <summary>
        /// 摄像机
        /// </summary>
        private Camera _camera = null;

        /// <summary>
        /// 显示模式
        /// </summary>
        [SerializeField]
        private ECameraShowType _ShowType = ECameraShowType.SHOW_ALL;

        /// <summary>
        /// 设计尺寸
        /// </summary>
        private float _DesignSize;

        /// <summary>
        /// 设计宽高比
        /// </summary>
        private float _DesignWHRatio;

        /// <summary>
        /// 实际宽高比
        /// </summary>
        private float _RealWHRatio;

        /// <summary>
        /// 苏醒时
        /// </summary>
        private void Awake()
        {
            _camera = GetComponent<Camera>();
            _DesignWHRatio = DesignSize.x / DesignSize.y;
            _DesignSize = Mathf.Max ( DesignSize.x, DesignSize.y ) / 200f;

            CalucationSize();
        }

#if UNITY_EDITOR
        private void Update()
        {
            CalucationSize();
        }
#endif

        /// <summary>
        /// 计算尺寸
        /// </summary>
        private void CalucationSize()
        {
            _RealWHRatio = ( float ) Screen.width / ( float ) Screen.height;
            switch ( _ShowType )
            {
                case ECameraShowType.SHOW_ALL:
                    _camera.orthographicSize = _DesignSize / _RealWHRatio * _DesignWHRatio;
                    break;
                case ECameraShowType.NO_BLACK:
                    _camera.orthographicSize = _DesignSize;
                    break;
            }
        }
    }
}
