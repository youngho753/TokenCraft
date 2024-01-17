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
    public Define.TokenType TokenType { get; protected set; }
    
    [SerializeField]
    public SpriteRenderer SpriteRenderer;
    public Rigidbody2D RigidBody { get; set; }
    public CircleCollider2D CircleCollider2D { get; set; }
    protected Animator Anim;

    private TokenController _onThisToken;
    private TokenController _underThisToken;


    public bool _isMouseClicked;
    

    public virtual Vector3 Position
    {
        get { return transform.position; }
        set
        {
            transform.position = value;
        }
    }

    public virtual void Update()
    {
        SetTokenData();
        
        //Debug용
        Debug.Log(Managers.Object.Tokens.Count);
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

        IsMouseClicked = false;

        return true;
    }

    #region 마우스 처리로직(IsMouseClicked)
    
    public bool IsMouseClicked
    {
        get { return _isMouseClicked;}
        set
        {
            _isMouseClicked = value;
            UnderThisToken = null;
            //TODO 생산토큰 없애줘야함
        }
    }
    
    #endregion

    #region TokenDataSetting부분(OnThisToken,UnderThisToken)
    
    public TokenController OnThisToken
    {
        get => _onThisToken;
        set
        {
            if (_onThisToken == value) return;
            
            //빈 토큰
            if (!value.IsValid())
            {
                if(OnThisToken.IsValid()) OnThisToken.UnderThisToken = null;
                _onThisToken = null;
                return;
            } 
            
            if (TokenType == value.TokenType)
            {
                _onThisToken = value;
                OnThisToken.UnderThisToken = this;
                return;
            }
            
        }
    }
    
    public TokenController UnderThisToken
    {
        get => _underThisToken;
        set
        {
            if (_underThisToken == value) return;
            
            //빈 토큰
            if (!value.IsValid())
            {
                if (UnderThisToken.IsValid()) UnderThisToken.OnThisToken = null;
                _underThisToken = null;
                return;
            } 
            
            if (TokenType == value.TokenType)
            {
                _underThisToken = value;
                UnderThisToken.OnThisToken = this;
                return;
            }
        }
    }

    public virtual void SetTokenData()
    {
        //아래 토큰이 있으면 
        if (UnderThisToken.IsValid()){
            CircleCollider2D.isTrigger = true;
            SpriteRenderer.sortingOrder = UnderThisToken.SpriteRenderer.sortingOrder + 1;
            Position = UnderThisToken.Position + new Vector3(0,0.2f,0);
            return;
        }
        
        //아래 토큰이 없고 마우스 클릭중이면 
        if (IsMouseClicked)
        {
            CircleCollider2D.isTrigger = true;
            SpriteRenderer.sortingOrder = Constants.StartMouseTokenLayerNum;
            return;
        }
        
        //TODO 아래 토큰이 없고 생산토큰 위면
        if (IsMouseClicked)
        {
            CircleCollider2D.isTrigger = true;
            SpriteRenderer.sortingOrder = Constants.StartTokenLayerNum;
            return;
        }
        
        //아래 토큰이 없고 빈공간에 있으면
        CircleCollider2D.isTrigger = false;
        SpriteRenderer.sortingOrder = Constants.StartTokenLayerNum;

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
        Position = transform.position;
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