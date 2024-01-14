using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class TokenBackgroundController : BaseController
{
    [SerializeField]
    public SpriteRenderer SpriteRenderer;
    public Rigidbody2D RigidBody { get; set; }
    public BoxCollider2D BoxCollider2D { get; set; }
    protected Animator Anim;

    public Dictionary<int, BlankTokenController> BlankTokenDic = new Dictionary<int, BlankTokenController>();

    public int maxTokenCnt;
    
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
        BoxCollider2D = GetComponent<BoxCollider2D>();
        maxTokenCnt = 2;

        
        
        return true;
    }


    public void MoveToTarget(Vector3 position)
    {
        transform.position = position;

        for (int i = 0; i < BlankTokenDic.Count; i++)
        {
            BlankTokenDic[i].MoveToTarget(position + new Vector3(-0.6f + (1.2f * i), 0.2f, 0), 0f,false);
        }
    }
    
    public virtual void AddTokenStack(int key, Stack<TokenController> tokenStack)
    {
        Util.SettingTokenStack(tokenStack);
        BlankTokenDic.GetValueOrDefault(key).onTokenStack = tokenStack;

    }
    
    public virtual void RemoveTokenStack(int key)
    {
        BlankTokenDic.GetValueOrDefault(key).onTokenStack = null;
    }

    public void RemoveOnToken(TokenController removeToken)
    {
        if (!Managers.Game._tokenStackDic.ContainsKey(removeToken.groupNum)) return;
        
        removeToken.InBlankToken.RemoveOnToken(removeToken);
    }
        
    
    

}