using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class MaterialController : TokenController
{
    public override bool Init()
    {
        base.Init();
        return true;
    }
    
    // Update is called once per frame
    void Update()
    {
    }
    
    /**
     * @desc 토큰을 토큰위에 쌓게될때 아래의 토큰x값, 아래의토큰 y-0.1 로 포지셔닝 
     */
    public virtual void Positioning(TokenController token)
    {
        transform.DOMove(token.transform.position, 100f, false);
    }
}
