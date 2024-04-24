using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NatureProductController : ProductController
{
    public override bool Init()
    {
        if (base.Init() == false) return false;
        
        ObjectType = Define.ObjectType.MaterialToken;
        // TokenType = Define.TokenType.NatureToken;
        
        return true;
    }
}