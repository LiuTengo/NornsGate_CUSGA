using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// VFX 本质是给sprite设置材质。
// 需要一个函数：
public class SpriteVFXController : MonoBehaviour
{
    [Header("Material List")] 
    public Material shine;
    public Material strongtint;

    [Header("AVGCharacters")]
    public List<GameObject> gameObjectsWithTag;

    private void Start()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("AVGCharacters");
        foreach (GameObject obj in gameObjects)
        {
            gameObjectsWithTag.Add(obj);
        }

        ClearAllSpritesVFX();
    }

    // 启动某个特效：实际上是给某个Sprite设置材质。
    public void UseSpriteVFX(Image obj, string vfxname)
    {
        //Debug.Log( "target:" + obj.name + "is using VFX:" + vfxname);
        switch (vfxname)
        {
            case "shine":
                obj.material = shine;
                break;
            case "strongtint":
                obj.material = strongtint;
                break;
        }
    }

    public void ClearSpriteVFX(Image obj)
    {
        if (obj.material != null)
        {
            obj.material = null;
        }
        
    }

    // 结束所有的材质shader效果
    public void ClearAllSpritesVFX()
    {
        for (int i = 0; i < gameObjectsWithTag.Count; i++)
        {
            Debug.Log(gameObjectsWithTag[i].name);
            // 在这里删除所有的材质
            gameObjectsWithTag[i].GetComponent<Image>().material = null;
        }
        
    }
    
    
}
