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

    public bool isEnabled = false;
    public int controllerType = 2;
    
    public Dictionary<int, Stack<TokenController>> TokenStackDic = new Dictionary<int, Stack<TokenController>>();

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
        isEnabled = false;
        return true;
    }


    public virtual void MoveToTarget(Vector3 position, float time, bool snapping)
    {
        transform.DOMove(position, time, snapping);
    }
    
    public virtual void SettingToken(int groupNum, int idx, bool isMoveTokenStack = false)
    {
        //마우스로 움직이고 있는 토큰일 경우
        if (isMoveTokenStack)
        {
            BoxCollider2D.isTrigger = true;
            SpriteRenderer.sortingOrder = Constants.StartMouseTokenLayerNum + idx;
        }
        //바닥에 놓여있는 토큰인 경우
        else
        {
            //이 토큰이 최하단인 경우
            if(idx == 0)
            {
                BoxCollider2D.isTrigger = false;   
            }
            //이 토큰이 최하단이 아닌경우
            else
            {
                BoxCollider2D.isTrigger = true;
            }
            
            SpriteRenderer.sortingOrder = Constants.StartTokenLayerNum + idx;
            
            
        }
    }

    public virtual void AddTokenStack(Stack<TokenController> tokenStack)
    {
        TokenStacks.Add(tokenStack);
    }
    
    public virtual void RemoveTokenStack(Stack<TokenController> tokenStack)
    {
        TokenStacks.Remove(tokenStack);
    }
    
    

}