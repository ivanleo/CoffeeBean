using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CoffeeBean;


public class Sample_12_HttpImage : MonoBehaviour
{
    private Image Pic_1 = null;
    private Image Pic_2 = null;
    private Image Pic_3 = null;
    private Image Pic_4 = null;

    private string p1 = "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1534852542891&di=1032afe0c248ed904639a5d8781ba092&imgtype=0&src=http%3A%2F%2Fpic31.nipic.com%2F20130731%2F2929309_171251222135_2.jpg";
    private string p2 = "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1534852542891&di=738cba82471e9740409d35192fbbe401&imgtype=0&src=http%3A%2F%2Fdynamic-image.yesky.com%2F1080x-%2FuploadImages%2F2014%2F294%2F16%2F9A8A9A7R5AZ0.jpg";
    private string p3 = "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1534852542889&di=1bf1451526425b7a72b5a73eda23c4be&imgtype=0&src=http%3A%2F%2Fpic9.photophoto.cn%2F20081230%2F0036036349856159_b.jpg";
    private string p4 = "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1534852542889&di=231eb28f734302f3faeff5902eefe85c&imgtype=0&src=http%3A%2F%2Fimage.tianjimedia.com%2FuploadImages%2F2015%2F085%2F28%2FG2AT81N0I2LD.jpg";

    private void Awake()
    {
        Pic_1 = transform.FindChildComponent<Image> ( "Pic_1" );
        Pic_2 = transform.FindChildComponent<Image> ( "Pic_2" );
        Pic_3 = transform.FindChildComponent<Image> ( "Pic_3" );
        Pic_4 = transform.FindChildComponent<Image> ( "Pic_4" );
    }

    private void OnGUI()
    {
        Rect rt = new Rect ( 20, 20, 400, 60 );
        var gs = GUI.skin.button;
        gs.fontSize = 28;

        if ( GUI.Button ( rt, "加载图片", gs ) )
        {
            Pic_1.LoadURLImage ( p1, true );

            Pic_2.LoadURLImage ( p2, false,
                                 isSuccess => Debug.Log ( "2号图片加载完毕" ) );

            Pic_3.LoadURLImage ( p3, false,
                                 null,
                                 HttpObject => Debug.Log ( "3号图片加载进度:" + HttpObject.progress ) );

            Pic_4.LoadURLImage ( p4, true,
                                 isSuccess => Debug.Log ( "4号图片加载完毕" ),
                                 HttpObject => Debug.Log ( "4号图片加载进度:" + HttpObject.progress ) );

        }

    }
}