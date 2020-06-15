using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class EQuickTool : IGameTools
{
    public void PerpareData()
    {
    }

    public void Draw()
    {
        string lastFileName = null;

        GUILayout.BeginVertical( EditorStyles.helpBox );
        GUILayout.Label( "打开操作" );

        /******************************************************/
        /*                打开最后一个LOG文件                 */
        /******************************************************/
        GUILayout.BeginHorizontal();

        if ( GUILayout.Button( "最后的Log文件", GUILayout.Height( 25 ) ) )
        {
            DirectoryInfo d    = new DirectoryInfo( "Log" );
            DateTime      time = new DateTime( 0 );

            foreach ( FileInfo fi in d.GetFiles() )
            {
                if ( fi.Extension.ToUpper() == ".txt".ToUpper() )
                {
                    if ( fi.CreationTime > time )
                    {
                        time         = fi.CreationTime;
                        lastFileName = fi.FullName;
                    }
                }
            }

            System.Diagnostics.Process.Start( "notepad", lastFileName );
        }

        /******************************************************/
        /*                    打开LOG文件夹                   */
        /******************************************************/
        if ( GUILayout.Button( "Log文件夹", GUILayout.Height( 25 ) ) )
        {
            DirectoryInfo d = new DirectoryInfo( "Log" );

            if ( d.Exists )
            {
                System.Diagnostics.Process.Start( "explorer.exe", d.FullName );
            }
            else
            {
                EditorUtility.DisplayDialog( "命令无效", "Log文件夹不存在", "知道了" );
            }
        }

        GUILayout.EndHorizontal();
        /******************************************************/
        /*                 打开StreamData文件夹               */
        /******************************************************/
        GUILayout.BeginHorizontal();

        if ( GUILayout.Button( "StreamData 文件夹", GUILayout.Height( 25 ) ) )
        {
            DirectoryInfo d = new DirectoryInfo( "Assets/StreamingAssets" );

            if ( d.Exists )
            {
                System.Diagnostics.Process.Start( "explorer.exe", d.FullName );
            }
            else
            {
                EditorUtility.DisplayDialog( "命令无效", "Log文件夹不存在", "知道了" );
            }
        }

        /******************************************************/
        /*               打开PersistentData文件夹             */
        /******************************************************/
        if ( GUILayout.Button( "PersistentData文件夹", GUILayout.Height( 25 ) ) )
        {
            var path = Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData );
            path = Path.Combine( path, "../LocalLow/DefaultCompany/Sumeru/" );

            DirectoryInfo d = new DirectoryInfo( path );

            if ( d.Exists )
            {
                System.Diagnostics.Process.Start( "explorer.exe", d.FullName );
            }
            else
            {
                EditorUtility.DisplayDialog( "命令无效", "Log文件夹不存在", "知道了" );
            }
        }

        GUILayout.EndHorizontal();
        GUILayout.EndVertical();


        GUILayout.Space( 10 );


        GUILayout.BeginVertical( EditorStyles.helpBox );
        GUILayout.Label( "清理操作" );
        GUILayout.BeginHorizontal();

        /******************************************************/
        /*                    清理 PlayerPrefs                */
        /******************************************************/
        if ( GUILayout.Button( "清理 PlayerPrefs", GUILayout.Height( 25 ) ) )
        {
            PlayerPrefs.DeleteAll();
            EditorUtility.DisplayDialog( "提示", "清理完成", "好的" );
        }

        /******************************************************/
        /*                  清理 StreamingAssets              */
        /******************************************************/
        if ( GUILayout.Button( "清理 StreamingAssets", GUILayout.Height( 25 ) ) )
        {
            var path = Application.streamingAssetsPath;
            DeleteFolderFile( path );

            EditorUtility.DisplayDialog( "提示", "清理完成", "好的" );
        }

        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();

        /******************************************************/
        /*                  清理 PersistentData               */
        /******************************************************/
        if ( GUILayout.Button( "清理 PersistentData", GUILayout.Height( 25 ) ) )
        {
            var path = Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData );
            path = Path.Combine( path, "../LocalLow/DefaultCompany/Sumeru/" );
            DeleteFolderFile( path + "assetbundle" );
            DeleteFolderFile( path + "download" );
            DeleteFolderFile( path + "texture" );

            EditorUtility.DisplayDialog( "提示", "清理完成", "好的" );
        }

        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }


    /// <summary>
    /// 删除文件夹下的文件
    /// </summary>
    /// <param name="path"></param>
    public static void DeleteFolderFile( string path )
    {
        string[] array = Directory.GetFileSystemEntries( path );

        for ( int i = 0; i < array.Length; i++ )
        {
            string file = array[i];

            if ( File.Exists( file ) )
            {
                FileInfo fi = new FileInfo( file );

                if ( fi.Attributes.ToString().IndexOf( "ReadOnly" ) != -1 )
                {
                    fi.Attributes = FileAttributes.Normal;
                }

                File.Delete( file ); //直接删除其中的文件
            }
            else
            {
                DirectoryInfo d1 = new DirectoryInfo( file );

                if ( d1.GetFiles().Length != 0 )
                {
                    DeleteFolder( d1.FullName ); ////递归删除子文件夹
                }

                Directory.Delete( file );
            }
        }
    }

    /// 删除文件夹及其内容
    /// </summary>
    /// <param name="dir"></param>
    public static void DeleteFolder( string dir )
    {
        foreach ( string data in Directory.GetFileSystemEntries( dir ) )
        {
            if ( File.Exists( data ) )
            {
                FileInfo fi = new FileInfo( data );

                if ( fi.Attributes.ToString().IndexOf( "ReadOnly" ) != -1 )
                {
                    fi.Attributes = FileAttributes.Normal;
                }

                File.Delete( data ); //直接删除其中的文件
            }
            else
            {
                DeleteFolder( data );
            } ////递归删除子文件夹

            Directory.Delete( data );
        }
    }
}