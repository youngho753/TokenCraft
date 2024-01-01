using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GameScene : UI_Scene
{
    GameManager _game;

    
   public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _game = Managers.Game;

        GetComponent<Canvas>().worldCamera = Camera.main;
        // 리프레쉬
        
        
        Managers.Resource.LoadAllAsync<Object>("PreLoad", (key, count, totalCount) =>
        {
            Debug.Log($"{key} {count}/{totalCount}");

            if (count == totalCount)
            {
                Managers.Data.Init();
            }
        });
        
        
        return true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
