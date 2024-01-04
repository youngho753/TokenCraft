using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BlankTokenController : TokenController
{
    public int _backgroundOrder;
    public int _spriteType;

    public Stack<TokenController> onTokenStack;
    
    void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false) return false;

        RigidBody = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        CircleCollider2D = GetComponent<CircleCollider2D>();
        return true;
    }

    public override void SettingToken(int groupNum, int idx, bool isMoveTokenStack = false)
    {
        
    }
}