using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class MaterialTokenController : TokenController
{
    public ProductTokenController _onProductToken;
    public int _onProductOrder;

    
    void Awake()
    {
        Init();
    }
    public override bool Init()
    {
        base.Init();

        ObjectType = Define.ObjectType.MaterialToken;
        TokenType = Define.TokenType.MaterialToken;
        
        _onProductOrder = -1;
        
        return true;
    }
    
    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }
    
    
    #region 마우스 처리로직(IsMouseClicked, IsMouseClickGroup)
    
    public override bool IsMouseClicked
    {
        get { return base.IsMouseClicked;}
        set
        {
            //클릭할때는 생산토큰은 항상 Null
            if(value && OnProductToken.IsValid()){
                OnProductToken = null;
            }
            
            base.IsMouseClicked = value;
            
            
        }
    }
    
    #endregion
    
    #region TokenDataSetting부분(OnThisToken,UnderThisToken,OnProductToken,SetTokenData)

    public virtual ProductTokenController OnProductToken
    {
        get => _onProductToken;
        set
        {
            if (_onProductToken == value) return;
            
            //빈 토큰
            if (!value.IsValid())
            {
                if (OnProductToken.IsValid())
                {
                    Debug.Log("ProductToken수" + OnProductToken.ProductOnTokenDic.Count);
                    OnProductToken.BlankZoneList[_onProductOrder].onToken = false;
                    OnProductToken.ProductOnTokenDic.Remove(_onProductOrder);
                    Debug.Log("ProductToken수" + OnProductToken.ProductOnTokenDic.Count);
                }
                _onProductToken = null;
                return;
            } 
            
            _onProductToken = value;
            _onProductOrder = _onProductToken.inBlankZoneOrder;
            
            OnProductToken.ProductOnTokenDic[_onProductOrder] = this;
            OnProductToken.BlankZoneList[_onProductOrder].onToken = true;
            return;

        }
    }
    
    protected override void SetTokenData()
    {
        //마우스 클릭중이면 
        if (IsMouseClicked)
        {
            CircleCollider2D.isTrigger = true;
            SpriteRenderer.sortingOrder = Constants.StartMouseTokenLayerNum;
            return;
        }

        
        //생산토큰 위면
        if (OnProductToken.IsValid())
        {
            CircleCollider2D.isTrigger = true;
            SpriteRenderer.sortingOrder = OnProductToken.SpriteRenderer.sortingOrder + 1;
            Position = OnProductToken.BlankZoneList[_onProductOrder].transform.position;
            return;
        }
        
        base.SetTokenData();
        
    }
    
    #endregion

 

 
}