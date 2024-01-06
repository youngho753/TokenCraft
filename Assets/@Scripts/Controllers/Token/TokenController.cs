using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TokenController : BaseController
{
    [SerializeField]
    public SpriteRenderer SpriteRenderer;
    public Rigidbody2D RigidBody { get; set; }
    public CircleCollider2D CircleCollider2D { get; set; }
    protected Animator Anim;
    public int groupNum;
    public int pkGroupNum;

    public BlankTokenController InBlankToken;

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


    public virtual void MoveToTarget(Vector3 position, float time, bool snapping)
    {
        transform.DOMove(position, time, snapping);
    }
    
    public virtual void SettingToken(int groupNum, int idx, bool isMoveTokenStack = false)
    {
        //마우스로 움직이고 있는 토큰일 경우
        if (isMoveTokenStack)
        {
            CircleCollider2D.isTrigger = true;
            SpriteRenderer.sortingOrder = Constants.StartMouseTokenLayerNum + idx;
        }
        //바닥에 놓여있는 토큰인 경우
        else
        {
            //이 토큰이 최하단인 경우
            if(idx == 0)
            {
                CircleCollider2D.isTrigger = false;   
            }
            //이 토큰이 최하단이 아닌경우
            else
            {
                CircleCollider2D.isTrigger = true;
            }
            
            SpriteRenderer.sortingOrder = Constants.StartTokenLayerNum + idx;
            
            
        }
        
        //모든 공통
        this.groupNum = groupNum;
    }

}