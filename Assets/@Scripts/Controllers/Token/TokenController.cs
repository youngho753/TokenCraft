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
    
    public SpriteRenderer SpriteRenderer;
    public Rigidbody2D RigidBody { get; set; }
    public CircleCollider2D CircleCollider2D { get; set; }
    public TokenData TokenData;

    private TokenController _onThisToken;
    private TokenController _underThisToken;

    public virtual int DataId { get; set; }
    public virtual string PrefabName { get; set; }
    public virtual string KoreanName { get; set; }
    public virtual string EnglishName { get; set; }
    public virtual int Value { get; set; }
    public virtual string Icon { get; set; }
    
    public virtual float _floatValue { get; set; }   //공중에 떠 있는 수치

    public int _state;
    
    public bool _isMouseClicked;
    public bool _isMouseClickGroup;
    
    //그림자
    public GameObject ShadowObject;
    public ShadowController ShadowController;

    void OnEnable()
    {
        Init();
    }
    
    public override bool Init()
    {
        if (base.Init() == false) return false;

        RigidBody = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        CircleCollider2D = GetComponent<CircleCollider2D>();
        
        CircleCollider2D.enabled = true;
        
        //Shadow 자식 생성
        if(ShadowObject == null) ShadowObject = Managers.Resource.Instantiate("TokenShadow", pooling: false);
        ShadowController = ShadowObject.GetOrAddComponent<ShadowController>(); 
        ShadowController.SetInfo(this.SpriteRenderer);
        ShadowObject.transform.SetParent(transform);
        ShadowObject.SetActive(true);
        
        
        IsMouseClicked = false;
        IsMouseClickGroup = false;
        
        _state = Constants.TOKEN_IDLE;

        DropToken();

        return true;
    }
    
    public virtual void FixedUpdate()
    {
        SetTokenData();
    }

    public virtual void clear()
    {
        base._init = false;
        FloatValue = 0;
        _state = Constants.TOKEN_IDLE;
        IsMouseClicked = false;
        IsMouseClickGroup = false;
        OnThisToken = null;
        UnderThisToken = null;
        
        CircleCollider2D.enabled = false;
        
        SpriteRenderer.maskInteraction = SpriteMaskInteraction.None;
        
        Managers.Resource.Destroy(gameObject);
    }

    public virtual void SetInfo(int tokenId)
    {
        DataId = tokenId;
        Dictionary<int, Data.TokenData> dict = Managers.Data.TokenDic;
        TokenData = dict[tokenId];
        // SpriteName = ;
        KoreanName = TokenData.KoreanName;
        EnglishName = TokenData.EnglishName;
        Value = TokenData.Value;
        
        Sprite sprite = Managers.Resource.Load<Sprite>($"{TokenData.SpriteName}");
        SpriteRenderer.sprite = sprite;
    }

    #region Deligater
    
    public virtual int State
    {
        get { return _state; }
        set
        {
            _state = value;
        }
    }
    
    public virtual float FloatValue
    {
        get { return _floatValue; }
        set
        {
            float beForeFloatValue = _floatValue; 
            _floatValue = value;
            Position = Position + new Vector3(0,value - beForeFloatValue,0);

            ShadowController._parentFloatValue = value;


        }
    }

    public virtual Vector3 Position
    {
        get { return transform.position; }
        set
        {
            transform.position = value;
        }
    }
    
    public virtual int SortingOrder
    {
        get { return this.SpriteRenderer.sortingOrder; }
        set
        {
            this.SpriteRenderer.sortingOrder = value;
            this.ShadowController.SpriteRenderer.sortingOrder = value - 10;
        }
    }
    
    #endregion


    #region 마우스 처리로직(IsMouseClicked, IsMouseClickGroup)
    
    public virtual bool IsMouseClicked
    {
        get { return _isMouseClicked;}
        set
        {
            _isMouseClicked = value;
            IsMouseClickGroup = value;

            //클릭할때는 UnderToken은 항상 Null
            if (value)
            {
                UnderThisToken = null;
                FloatValue = 0.2f;
            }
            else
            {
                FloatValue = 0f;
            }
        }
    }
    
    public virtual bool IsMouseClickGroup
    {
        get { return _isMouseClickGroup;}
        set
        {
            // DOTween.Kill("DropCoin"); //떨어지는 이펙트 안나오게
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
            SortingOrder = Constants.StartMouseTokenLayerNum;
            return;
        }
        
        //아래 토큰이 있으면 
        if (UnderThisToken.IsValid()){
            CircleCollider2D.isTrigger = true;
            SortingOrder = UnderThisToken.SpriteRenderer.sortingOrder + 1;
            // Position = UnderThisToken.Position + new Vector3(0,0.2f,0);

            transform.DOMove(UnderThisToken.Position + new Vector3(0, 0.2f, 0), 0.01f);

            Position = Position;
            
            return;
        }

        //공중에 떠있으면
        if (FloatValue > 0)
        {
            CircleCollider2D.isTrigger = true;
            SortingOrder = Constants.FloatTokenLayerNum;
            return;
        }

        if (State == Constants.TOKEN_COIN_INPUT)
        {
            CircleCollider2D.isTrigger = true;
            SortingOrder = Constants.FloatTokenLayerNum;
            return;
        }
        
        
        //아래 토큰이 없고 빈공간에 있으면
        CircleCollider2D.isTrigger = false;
        SortingOrder = Constants.StartTokenLayerNum;
        

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

    #region 애니메이션 부분

    public virtual void IntoTokenInput()
    {
        Position = new Vector3(4.75f, -3.9f, 0);
        FloatValue = 0;
        SpriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        CircleCollider2D.enabled = false;
        ShadowObject.SetActive(false);

        transform.DOMove(new Vector3(0f, 1f, 0), 0.5f)
            .SetRelative(true)
            .SetEase(Ease.InExpo)
            .OnComplete(() => clear());
    }

    public virtual void DropToken()
    {
        DOTween.To(() => FloatValue, x => FloatValue = x, 0, 0.5f)
            .SetEase(Ease.InExpo)
            .SetId("DropCoin")
            .OnComplete(() => DropAnimation());
    }

    public virtual void DropAnimation()
    {
        GameObject go = Managers.Resource.Instantiate("TokenDropEffect", pooling: true);
        go.transform.position = transform.position;
        
        transform.DOShakePosition(0.3f ,new Vector3(0.15f,0.15f,0),20);
        transform.DOShakeRotation(0.3f,new Vector3(30f,30f,0),10);
    }

    #endregion
}