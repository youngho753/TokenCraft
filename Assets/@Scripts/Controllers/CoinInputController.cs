using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class CoinInputController : BaseController
{
    
    void Start()
    {
            
    }
    
    public override bool Init()
    {
        if (base.Init() == false) return false;
        
        return true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    #region Input로직()
    
    public void InCoinInput()
    {
        Managers.Object.SpawnToken<MaterialTokenController>(new Vector3(), 100000, "MaterialToken", true);
    }
    
    #endregion

}