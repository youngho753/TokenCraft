using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class MaterialTokenController : TokenController
{
    void Awake()
    {
        Init();
    }
    public override bool Init()
    {
        base.Init();

        ObjectType = Define.ObjectType.MaterialToken;
        
        return true;
    }
    
    // Update is called once per frame
    void Update()
    {
    }

 

 
}