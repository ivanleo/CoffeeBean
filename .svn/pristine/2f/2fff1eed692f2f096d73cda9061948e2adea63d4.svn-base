/********************************************************************
    created:    2018/06/07
    created:    7:6:2018   16:42
    filename:   D:\Work\PushCoin\trunk\PushCoin\Assets\CoffeeBean\Utils\CUtilAutoCombineMesh.cs
    file path:  D:\Work\PushCoin\trunk\PushCoin\Assets\CoffeeBean\Utils
    file base:  CUtilAutoCombineMesh
    file ext:   cs
    author:     Leo

    purpose:    自动网格合并
*********************************************************************/
using UnityEngine;

/// <summary>
/// 自动网格合并
/// </summary>
public class CUtilAutoCombineMesh : MonoBehaviour
{
    /// <summary>
    /// 开始时合并
    /// </summary>
    void Start ()
    {
        MeshFilter Mf = transform.GetComponent<MeshFilter>();
        if ( Mf == null )
        {
            Mf = gameObject.AddComponent<MeshFilter>();
        }

        MeshRenderer Mr = transform.GetComponent<MeshRenderer>();
        if ( Mr == null )
        {
            Mr = gameObject.AddComponent<MeshRenderer>();
        }

        //---------------- 先获取材质 -------------------------
        //获取自身和所有子物体中所有MeshRenderer组件
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();

        //新建材质球数组
        Material[] mats = new Material[meshRenderers.Length];
        for ( int i = 0 ; i < meshRenderers.Length ; i++ )
        {
            //生成材质球数组
            mats[i] = meshRenderers[i].sharedMaterial;
        }

        //---------------- 合并 Mesh -------------------------
        //获取自身和所有子物体中所有MeshFilter组件
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        for ( int i = 0 ; i < meshFilters.Length ; i++ )
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            //矩阵(Matrix)自身空间坐标的点转换成世界空间坐标的点
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive ( false );
        }


        //为新的整体新建一个mesh
        Mf.mesh = new Mesh();
        //合并Mesh. 第二个false参数, 表示并不合并为一个网格, 而是一个子网格列表
        Mf.mesh.CombineMeshes ( combine, false );
        transform.gameObject.SetActive ( true );

        //为合并后的新Mesh指定材质 ------------------------------
        Mr.sharedMaterials = mats;
    }

}
