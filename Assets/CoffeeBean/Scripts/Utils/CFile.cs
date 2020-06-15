/********************************************************************
   All Right Reserved By Leo
   Created:    2020/6/6 19:09:35
   File: 	   CFile.cs
   Author:     Leo

   Purpose:    文件辅助类
*********************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeBean
{
    public class CFile
    {
        /// <summary>
        /// <para>当字符串代表文件路径时</para>
        /// <para>提取文件名</para>
        /// <para>例如</para>
        /// <para>var str = "Assets/Resources/Prefab/Player/Player_1.prefab"</para>
        /// <para>str = str.GetFileName()</para>
        /// <para>Debug.Log(str)</para>
        /// <para></para>
        /// <para>输出：Player_1</para>
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFileName( string path )
        {
            int start = path.LastIndexOf ( '/' );
            int end   = path.LastIndexOf ( '.' );

            if ( start == -1 )
            { start = 0; }

            if ( end == -1 )
            { end = path.Length; }

            return path.Substring( start + 1, end - start - 1 );
        }

        /// <summary>
        /// <para>当字符串代表文件路径时</para>
        /// <para>提取包含后缀名的文件名</para>
        /// <para>例如</para>
        /// <para>var str = "Assets/Resources/Prefab/Player/Player_1.prefab"</para>
        /// <para>str = str.GetFileFullName()</para>
        /// <para>Debug.Log(str)</para>
        /// <para></para>
        /// <para>输出：Player_1.prefab</para>
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFileNameWithType( string path )
        {
            int last = path.LastIndexOf( '/' );
            return path.Substring( last + 1 );
        }

        /// <summary>
        /// <para>当字符串代表文件路径时</para>
        /// <para>提取后缀名</para>
        /// <para>例如</para>
        /// <para>var str = "Assets/Resources/Prefab/Player/Player_1.prefab"</para>
        /// <para>str = str.GetFileType()</para>
        /// <para>Debug.Log(str)</para>
        /// <para></para>
        /// <para>输出：prefab</para>
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFileType( string path )
        {
            int lp = path.LastIndexOf( '.' );
            if ( lp != -1 )
                return path.Substring( lp + 1 );
            else
                return "";
        }

        /// <summary>
        /// 获取文件夹下所有文件
        /// </summary>
        /// <param name="FolderPath">文件夹路径</param>
        /// <param name="SearchPattern">文件过滤器 默认*.*所有文件</param>
        /// <param name="SO">查找模式 递归 或 非递归 默认递归查找文件</param>
        public static List<string> GetFolderFiles( string FolderPath, string SearchPattern = "*.*", SearchOption SO = SearchOption.AllDirectories )
        {
            List<string> allPaths = new List<string>();
            DirectoryInfo DI = new DirectoryInfo ( FolderPath );

            if ( !DI.Exists )
            {
                return allPaths;
            }

            var files = DI.GetFiles ( SearchPattern, SO );

            for ( int i = 0; i < files.Length; i++ )
            {
                var path = files[i].FullName.Replace ( "\\", "/" );
                allPaths.Add( path );
            }

            return allPaths;
        }

        /// <summary>
        /// <para>当字符串代表文件路径时</para>
        /// <para>提取文件夹名</para>
        /// <para>例如</para>
        /// <para>var str = "Assets/Resources/Prefab/Player/Player_1.prefab"</para>
        /// <para>str = str.GetFolderName()</para>
        /// <para>Debug.Log(str)</para>
        /// <para></para>
        /// <para>输出：Player</para>
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFolderName( string path )
        {
            var fi       = new FileInfo( path );
            var dname    = fi.DirectoryName.Replace( "\\", "/" );
            var lastgang = dname.LastIndexOf( "/" ) + 1;

            return dname.Substring( lastgang );
        }

        /// <summary>
        /// <para>提取文件路径中的文件夹名</para>
        /// <para>例如</para>
        /// <para>var str = "Assets/Resources/Prefab/Player/Player_1.prefab"</para>
        /// <para>str = str.GetFolder()</para>
        /// <para>Debug.Log(str)</para>
        /// <para></para>
        /// <para>输出：Assets/Resources/Prefab/Player</para>
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFolderPath( string path )
        {
            int last      = path.LastIndexOf( '/' );
            return path.Substring( 0, last );
        }

        /// <summary>
        /// 检查文件是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsFileExist( string path )
        {
            return File.Exists( path );
        }

        /// <summary>
        /// 检查文件夹是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsFolderExist( string path )
        {
            return Directory.Exists( path );
        }
    }
}