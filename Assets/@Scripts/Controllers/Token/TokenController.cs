using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class TokenController : BaseController
{
    [SerializeField]
    public SpriteRenderer SpriteRenderer;
    public Rigidbody2D RigidBody { get; set; }
    public CircleCollider2D CircleCollider2D { get; set; }
    protected Animator Anim;

    private TokenController OnThisToken;
    private TokenController UnderThisToken;

    // //마우스로 움직이고 있는 토큰일 경우
// if (isMoveTokenStack)
// {
//     CircleCollider2D.isTrigger = true;
//     SpriteRenderer.sortingOrder = Constants.StartMouseTokenLayerNum + idx;
// }
// //바닥에 놓여있는 토큰인 경우
// else
// {
//     //이 토큰이 최하단인 경우
//     if(idx == 0)
//     {
//         CircleCollider2D.isTrigger = false;   
//     }
//     //이 토큰이 최하단이 아닌경우
//     else
//     {
//         CircleCollider2D.isTrigger = true;
//     }
//             
//     SpriteRenderer.sortingOrder = Constants.StartTokenLayerNum + idx;
//             
//             
// }
    public bool isMouseClicked
    {
        get { return isMouseClicked;}
        set
        {
            OnMouseClicked(value,0);
        }
    }

    public int OnProductTokenOrder;
    
    public Action OnUnderMoved;
    
    public Vector3 position
    {
        get { return transform.position; }
        set
        {
            transform.position = value;
            if(OnThisToken != null) OnThisToken.position = value;
        }
    }

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

        isMouseClicked = false;

        OnProductTokenOrder = -1;
        
        return true;
    }

    public void OnMouseClicked(bool isMouseClicked ,int layer)
    {
        CircleCollider2D.isTrigger = isMouseClicked;
        
        if(isMouseClicked)  SpriteRenderer.sortingOrder = Constants.StartMouseTokenLayerNum + layer;
        else                SpriteRenderer.sortingOrder = Constants.StartTokenLayerNum + layer;
        
        
        //위토큰세팅
        if(OnThisToken) OnThisToken.OnMouseClicked(isMouseClicked, ++layer);
        
        
    }

    
    /**
     * @desc 나와 같지 않은 ObjectType이면 스킵
     */
    public void SetOnThisToken(TokenController token)
    {
        //위에 아무 토큰도 없어질 경우 위 토큰 SetUnderThisToken(null)
        if (token == null || !token.enabled)
        {
            if (OnThisToken != null && OnThisToken.enabled)
            {
                OnThisToken.SetUnderThisToken(null);
                return;
            } 
        }
        
        //나와 같지 않은 ObjectType이면 스킵
        if (ObjectType != token.ObjectType)
        {
            Debug.Log("TokenContoller.SetOnThisToken : ObjectType체크 필요!!");
            return;
        }
        
        //위에 새로운 토큰이 왔을 때
        OnThisToken = token;
        OnThisToken.SetUnderThisToken(this);
    }
    
    public void SetUnderThisToken(TokenController token)
    {
        UnderThisToken = token;
    }

    public virtual TokenController GetHighestToken()
    {
        if (OnThisToken != null && OnThisToken.enabled)
            return OnThisToken.GetHighestToken();
        //else
            return this;
    }
    
    public virtual TokenController GetLowestToken()
    {
        if (UnderThisToken != null && UnderThisToken.enabled)
            return UnderThisToken.GetLowestToken();
        //else
            return this;
    }
    
    private void OnCollisionStay2D(Collision2D collision)
    {
        position = transform.position;
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
        
    }

    // public virtual void onUsed()
    // {
    //     
    //     Util.RemoveTokenDic(this);
    //
    //     if (InBlankToken.onTokenStack != null)
    //     {
    //         InBlankToken.productToken.RemoveToken(this);
    //     }
    //     
    //     gameObject.SetActive(false);
    //         
    // }
}