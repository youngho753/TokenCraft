using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class MaterialTokenController : TokenController
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
     * @desc 토큰을 토큰위에 쌓게될때 아래의 토큰x값, 아래의토큰 y-0.2 로 포지셔닝 
     */
    public override void MoveToTarget(Vector3 position, float time, bool snapping)
    {
        base.MoveToTarget(position, time, snapping);
    }

    public override void SettingToken(int groupNum, int idx, bool isMoveTokenStack = false)
    {
        base.SettingToken(groupNum, idx, isMoveTokenStack);
    }
}