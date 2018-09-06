using UnityEngine;
using UnityEngine.UI;

namespace CoffeeBean
{
    /// <summary>
    /// 工具类
    /// </summary>
    public static class CTools
    {
        /// <summary>
        /// 绘制一个Debug点在Canvas上
        /// </summary>
        /// <param name="TargetPos"></param>
        public static void DrawDebugPoint( Vector2 TargetPos )
        {
            Canvas canvas = GameObject.Find( "Canvas" ).GetComponent<Canvas>();
            GameObject temp = new GameObject( "DebugBall" );
            RectTransform RT = temp.AddComponent<RectTransform>();
            RT.sizeDelta = Vector2.one * 5;
            Image img = temp.AddComponent<Image>();
            img.color = Color.red;
            RT.SetParent( canvas.transform );
            RT.anchoredPosition = TargetPos;
        }

        /// <summary>
        /// 绘制一个Debug点在世界里
        /// </summary>
        /// <param name="WorldPos">世界坐标</param>
        public static void DrawDebugPoint( Vector3 WorldPos )
        {
            GameObject temp = GameObject.CreatePrimitive( PrimitiveType.Sphere );
            temp.transform.localScale = Vector3.one / 10f;
            temp.transform.position = WorldPos;
        }


        /// <summary>
        /// 得到屏幕指定点为圆心
        /// 一定范围内的碰撞体
        /// </summary>
        /// <param name="screenPosition">园的中心</param>
        /// <param name="ResultList">半径</param>
        /// <returns>Collider2D碰撞体数组</returns>
        public static Collider2D[] GetObjectsInCircle( Vector2 screenPosition, float radius )
        {
            return Physics2D.OverlapCircleAll( screenPosition, radius );
        }


    }
}
