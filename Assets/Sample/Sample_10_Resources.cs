using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CoffeeBean;


public class Sample_10_Resources : MonoBehaviour
{
    private void OnGUI()
    {
        Rect rt = new Rect ( 20, 20, 400, 60 );
        var gs = GUI.skin.button;
        gs.fontSize = 28;

        if ( GUI.Button ( rt, "加载预制体", gs ) )
        {
            //CResourcesManager.LoadPrefab ( "Resources/Cube" );
            //CResourcesManager.LoadPrefab ( "Resources/Cube.prefab" );
            //CResourcesManager.LoadPrefab ( "resources/Cube.prefab" );
            //CResourcesManager.LoadPrefab ( "Cube.prefab" );

            //上面4种加载方式与本条等价
            GameObject go = CResourcesManager.LoadPrefab ( "Cube" );
            Debug.Log ( go.ToString() + " loaded" );

            //对于反复创建的物体可使用来缓存加载的资源
            //下次创建时效率会快很多
            //CResourcesManager.LoadPrefab ( "Cube", true );
        }


        rt.y += 80f;
        if ( GUI.Button ( rt, "加载预制体并创建", gs ) )
        {
            //CResourcesManager.CreatePrefab ( "Resources/Cube" );
            //CResourcesManager.CreatePrefab ( "Resources/Cube.prefab" );
            //CResourcesManager.CreatePrefab ( "resources/Cube.prefab" );
            //CResourcesManager.CreatePrefab ( "Cube.prefab" );

            //上面4种加载方式与本条等价
            GameObject go = CResourcesManager.CreatePrefab ( "Cube" );
            Debug.Log ( go.ToString() + " created" );

            //对于反复创建的物体可使用来缓存加载的资源
            //下次创建时效率会快很多
            //CResourcesManager.CreatePrefab ( "Cube", true );
        }

        rt.y += 80f;
        if ( GUI.Button ( rt, "加载声音", gs ) )
        {
            //CResourcesManager.LoadAudio ( "Resources/getcoin" );
            //CResourcesManager.LoadAudio ( "Resources/getcoin.mp3" );
            //CResourcesManager.LoadAudio ( "resources/getcoin.mp3" );
            //CResourcesManager.LoadAudio ( "getcoin.mp3" );

            //上面4种加载方式与本条等价
            AudioClip ac = CResourcesManager.LoadAudio ( "getcoin" );
            Debug.Log ( ac.ToString() + " loaded" );
        }

        rt.y += 80f;
        if ( GUI.Button ( rt, "加载散图精灵", gs ) )
        {
            //CResourcesManager.LoadSprite ( "Resources/22" );
            //CResourcesManager.LoadSprite ( "Resources/22.png" );
            //CResourcesManager.LoadSprite ( "resources/22.png" );
            //CResourcesManager.LoadSprite ( "22.png" );

            //上面4种加载方式与本条等价
            Sprite sp = CResourcesManager.LoadSprite ( "22" );
            Debug.Log ( sp.ToString() + " loaded" );
        }

        rt.y += 80f;
        if ( GUI.Button ( rt, "加载图集精灵", gs ) )
        {
            //CResourcesManager.LoadAltasSprite ( "Resources/sps" , "4");
            //CResourcesManager.LoadAltasSprite ( "Resources/sps.spritealtas", "4" );
            //CResourcesManager.LoadAltasSprite ( "resources/sps.spritealtas" , "4");
            //CResourcesManager.LoadAltasSprite ( "sps.spritealtas", "4" );

            //上面4种加载方式与本条等价
            Sprite sp = CResourcesManager.LoadAltasSprite ( "sps", "4" );
            Debug.Log ( sp.ToString() + " loaded" );
        }

        rt.y += 80f;
        if ( GUI.Button ( rt, "加载材质", gs ) )
        {
            //CResourcesManager.LoadMaterial ( "Resources/Red" );
            //CResourcesManager.LoadMaterial ( "Resources/Red.mat");
            //CResourcesManager.LoadMaterial ( "resources/Red.mat");
            //CResourcesManager.LoadMaterial ( "Red.mat" );

            //上面4种加载方式与本条等价
            Material mr = CResourcesManager.LoadMaterial ( "Red" );
            Debug.Log ( mr.ToString() + " loaded" );
        }

        rt.y += 80f;
        if ( GUI.Button ( rt, "加载文本", gs ) )
        {
            //CResourcesManager.LoadMaterial ( "Resources/str" );
            //CResourcesManager.LoadMaterial ( "Resources/str.txt");
            //CResourcesManager.LoadMaterial ( "resources/str.txt");
            //CResourcesManager.LoadMaterial ( "str.txt" );

            //上面4种加载方式与本条等价

            string txt = CResourcesManager.LoadText ( "str" );
            Debug.Log ( txt );
        }
    }

}
