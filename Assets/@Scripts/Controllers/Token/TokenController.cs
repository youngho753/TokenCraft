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

    protected TokenController _onThisToken;
    protected TokenController _underThisToken;

    public TokenController OnThisToken
    {
        get => _onThisToken;
        set
        {
            _onThisToken = value;
            OnMouseClicked(value);

        }
    }
    

    public bool _isMouseClicked;
    public bool isMouseClicked
    {
        get { return _isMouseClicked;}
        set
        {
            _isMouseClicked = value;
            
            if(value) OnMouseClicked(0);
            else                OnMouseUnClicked(0);

        }
    }

    public int OnProductTokenOrder;
    
    public Action OnUnderMoved;
    
    public virtual Vector3 position
    {
        get { return new Vector3(); }
        set
        {
            transform.position = value;
            if(OnThisToken.IsValid()) OnThisToken.position = value + new Vector3(0,0.2f,0);
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
    
    #region 마우스 처리로직(OnMouseClicked,OnMouseUnClicked)
    
    public void OnMouseClicked(int layer)
    {
        //첫 하나만 적용
        if(layer == 0){
            CircleCollider2D.isTrigger = false;
            SetUnderThisToken(null);
        }
        
        CircleCollider2D.isTrigger = true;
        
        SpriteRenderer.sortingOrder = Constants.StartMouseTokenLayerNum + layer;
        
        //위토큰세팅
        if(OnThisToken.IsValid()) OnThisToken.OnMouseClicked(++layer);
    }

    public void OnMouseUnClicked(int layer)
    {
        //첫 하나만 적용
        if(layer == 0){
            CircleCollider2D.isTrigger = false;
        }
        
        SpriteRenderer.sortingOrder = Constants.StartTokenLayerNum + layer;
        
        //위토큰세팅
        if(OnThisToken.IsValid()) OnThisToken.OnMouseUnClicked(++layer);
    }
    
    #endregion

    #region TokenDataSetting부분(OnThisToken,UnderThisToken)
    
    /**
     * @desc 나와 같지 않은 ObjectType이면 스킵
     */
    public void SetOnThisToken(TokenController token)
    {
        //위에 아무 토큰도 없어질 경우 위 토큰 SetUnderThisToken(null)
        if (!token.IsValid())
        {
            if (OnThisToken.IsValid())
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
        OnThisToken.CircleCollider2D.isTrigger = true;
        OnThisToken.position = transform.position + new Vector3(0,0.2f,0);
        OnThisToken.SpriteRenderer.sortingOrder = SpriteRenderer.sortingOrder + 1;
        OnThisToken.SetUnderThisToken(this);
    }
    
    public void SetUnderThisToken(TokenController token)
    {
        UnderThisToken = token;
    }
    
    #endregion

    #region DataGet(GetHighestToken,GetLowestToken)
    
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
    
    #endregion

    #region 충돌이벤트(OnCollisionStay2D)

    private void OnCollisionStay2D(Collision2D collision)
    {
        position = transform.position;
    }

    #endregion
    
    
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