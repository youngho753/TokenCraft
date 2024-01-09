using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BlankTokenController : TokenController
{
    public int _backgroundOrder;
    public int _spriteType;

    public ProductTokenController productToken;
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

    public override void MoveToTarget(Vector3 position, float time, bool snapping)
    {
        transform.position = position;

        Stack<TokenController> tokenStack = new Stack<TokenController>();
        tokenStack.Push(this);

        Stack<TokenController> concatTokenStack = Util.ConcatTokenStack(tokenStack, onTokenStack);
        
        Util.SettingTokenStack(concatTokenStack);
    }

    public void RemoveOnToken(TokenController removeToken)
    {
        if (this.onTokenStack == null) return;
        
        Stack<TokenController> copyTokenStack = Util.DeepCopy(this.onTokenStack);

        Stack<TokenController> onTokenStack = new Stack<TokenController>();
        Stack<TokenController> underTokenStack = new Stack<TokenController>();
        
        foreach (TokenController tc in copyTokenStack)
        {
            if (tc.pkGroupNum == removeToken.pkGroupNum)
            {
                break;

            }
            
            onTokenStack.Push(copyTokenStack.Pop());
        }
        copyTokenStack.Pop();
        
        onTokenStack = Util.ReverseStack(onTokenStack);

        underTokenStack = copyTokenStack;

        Util.ConcatTokenStack(underTokenStack, onTokenStack, true);
    }
}