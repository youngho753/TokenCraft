using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TokenBackgroundController : BaseController
{
    [SerializeField]
    public SpriteRenderer SpriteRenderer;
    public Rigidbody2D RigidBody { get; set; }
    public BoxCollider2D BoxCollider2D { get; set; }
    protected Animator Anim;

    public Dictionary<int, TokenController> BlankTokenDic = new Dictionary<int, TokenController>();
    public Dictionary<int, Stack<TokenController>> BlackOnTokenStackDic = new Dictionary<int, Stack<TokenController>>();

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
            BlankTokenDic[i].transform.position = position + new Vector3(-0.6f + (1.2f * i), 0.2f, 0);
        }
    }
    
    public virtual void AddTokenStack(Stack<TokenController> tokenStack, int key)
    {
        BlackOnTokenStackDic.Add(key, tokenStack);
    }
    
    public virtual void RemoveTokenStack(int key)
    {
        BlackOnTokenStackDic.Remove(key);
    }
    
    

}