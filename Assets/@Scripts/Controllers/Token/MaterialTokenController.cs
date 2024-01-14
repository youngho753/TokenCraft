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
        return true;
    }
    
    // Update is called once per frame
    void Update()
    {
    }

    public override void SettingToken(int groupNum, int idx, bool isMoveTokenStack = false)
    {
        base.SettingToken(groupNum, idx, isMoveTokenStack);
    }

 
}