using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NatureTokenController : ProductTokenController
{
    public override bool Init()
    {
        if (base.Init() == false) return false;
        
        ObjectType = Define.ObjectType.MaterialToken;
        
        return true;
    }
}