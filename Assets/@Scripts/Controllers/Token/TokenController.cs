using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using DG.Tweening;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class TokenController : BaseController
{
    public int tokenOrder;
    
    public Define.TokenType TokenType { get; protected set; }
    
    [SerializeField]
    public SpriteRenderer SpriteRenderer;
    public Rigidbody2D RigidBody { get; set; }
    public CircleCollider2D CircleCollider2D { get; set; }
    protected Animator Anim;
    public TokenData TokenData;

    private TokenController _onThisToken;
    private TokenController _underThisToken;

    public virtual int DataId { get; set; }
    public virtual string PrefabName { get; set; }
    public virtual string KoreanName { get; set; }
    public virtual string EnglishName { get; set; }
    public virtual int Value { get; set; }
    public virtual string Icon { get; set; }

    public bool _isMouseClicked;
    public bool _isMouseClickGroup;
    
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
        IsMouseClickGroup = false;

        return true;
    }
    
    public virtual void Update()
    {
        SetTokenData();
        
        //Debug용
        PrintDebug();
    }

    public virtual void SetInfo(int tokenId)
    {
        DataId = tokenId;
        Dictionary<int, Data.TokenData> dict = Managers.Data.TokenDic;
        TokenData = dict[tokenId];
        KoreanName = TokenData.KoreanName;
        EnglishName = TokenData.EnglishName;
        Value = TokenData.Value;
    }
    
    

    public virtual Vector3 Position
    {
        get { return transform.position; }
        set
        {
            transform.position = value;
        }
    }

    #region 마우스 처리로직(IsMouseClicked, IsMouseClickGroup)
    
    public virtual bool IsMouseClicked
    {
        get { return _isMouseClicked;}
        set
        {
            _isMouseClicked = value;
            IsMouseClickGroup = value;

            //클릭할때는 UnderToken은 항상 Null
            if (value) UnderThisToken = null;
        }
    }
    
    public virtual bool IsMouseClickGroup
    {
        get { return _isMouseClickGroup;}
        set
        {
            _isMouseClickGroup = value;
            if(OnThisToken.IsValid()) OnThisToken.IsMouseClickGroup = value;
        }
    }
    
    #endregion

    #region TokenDataSetting부분(OnThisToken,UnderThisToken,SetTokenData)
    
    public virtual TokenController OnThisToken
    {
        get => _onThisToken;
        set
        {
            if (_onThisToken == value) return;
            
            //빈 토큰
            if (!value.IsValid())
            {
                if(OnThisToken.IsValid()) OnThisToken._underThisToken = null;
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
    
    public virtual TokenController UnderThisToken
    {
        get => _underThisToken;
        set
        {
            if (_underThisToken == value) return;
            
            //빈 토큰
            if (!value.IsValid())
            {
                if (UnderThisToken.IsValid()) UnderThisToken._onThisToken = null;
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

 

    protected virtual void SetTokenData()
    {
        //마우스 클릭중이면 
        if (IsMouseClicked)
        {
            CircleCollider2D.isTrigger = true;
            SpriteRenderer.sortingOrder = Constants.StartMouseTokenLayerNum;
            return;
        }
        
        //아래 토큰이 있으면 
        if (UnderThisToken.IsValid()){
            CircleCollider2D.isTrigger = true;
            SpriteRenderer.sortingOrder = UnderThisToken.SpriteRenderer.sortingOrder + 1;
            Position = UnderThisToken.Position + new Vector3(0,0.2f,0);
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
        if (OnThisToken.IsValid())
            return OnThisToken.GetHighestToken();
        //else
            return this;
    }
    
    public virtual TokenController GetLowestToken()
    {
        if (UnderThisToken.IsValid())
            return UnderThisToken.GetLowestToken();
        //else
            return this;
    }
    
    #endregion

    #region 충돌이벤트(OnCollisionStay2D)

    public virtual void OnCollisionStay2D(Collision2D collision)
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

    public virtual void PrintDebug()
    {
        /** 위 토큰의 아래토큰과 현재토큰이 다를때 */
        if (OnThisToken.IsValid() && OnThisToken.UnderThisToken.IsValid() && OnThisToken.UnderThisToken.tokenOrder != this.tokenOrder)
        {
            Debug.LogError("##### 위 토큰의 아래토큰("+OnThisToken.UnderThisToken.tokenOrder+")과 현재토큰("+tokenOrder+")이 다릅니다.");
        }
        
        /** 아래 토큰의 위토큰과 현재토큰이 다를때 */
        if (UnderThisToken.IsValid() && UnderThisToken.OnThisToken.tokenOrder != this.tokenOrder)
        {
            Debug.LogError("##### 아래 토큰의 위 토큰("+UnderThisToken.OnThisToken.tokenOrder+")과 현재("+tokenOrder+")이 다릅니다.");
        }
        
        
    }
}