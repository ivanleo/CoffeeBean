using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CoffeeBean
{
    /// <summary>
    /// 裁剪区域粒子
    /// 负责计算剪裁区域并
    /// </summary>
    [RequireComponent( typeof( ParticleSystem ) )]
    public class CRectParticle : MonoBehaviour
    {
        /// <summary>
        /// 设置容器rect
        /// </summary>
        public void SetRect( RectTransform m_RectTrans )
        {
            var UICanvasTransform = GameObject.Find( "Canvas" ).transform;

            //计算容器宽高的一半，值得注意的是要乘cnavas的缩放比例
            var halfWidth = m_RectTrans.sizeDelta.x * 0.5f * UICanvasTransform.localScale.x;
            var halfHeight = m_RectTrans.sizeDelta.y * 0.5f * UICanvasTransform.localScale.x;

            Vector4 area = CalculateArea(m_RectTrans.position , halfWidth , halfHeight);

            var pss = transform.GetComponentsInChildren<ParticleSystem>();
            for ( int i = 0; i < pss.Length; i++ )
            {
                //给Shader _Area属性赋值
                pss[i].GetComponent<Renderer>().material.SetVector( "_Area", area );
            }
        }

        /// <summary>
        /// 计算容器在世界坐标的Vector4，xz为左右边界的值，yw为下上边界值
        /// </summary>
        /// <param name="position"></param>
        /// <param name="halfW"></param>
        /// <param name="halfH"></param>
        /// <returns></returns>
        private Vector4 CalculateArea( Vector3 position, float halfW, float halfH )
        {
            return new Vector4() {
                x = position.x - halfW,
                y = position.y - halfH,
                z = position.x + halfW,
                w = position.y + halfH
            };
        }
    }
}