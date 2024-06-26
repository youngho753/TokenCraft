using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


public class MaterialTokenController : TokenController
{
    public ProductController _onProductToken;
    public int _onProductOrder;
    public Text textObject;
    public AnimationCurve curve;
    public float curveHeight = 1f;
    
    public override bool Init()
    {
        base.Init();

        // this.DoFlip();

        ObjectType = Define.ObjectType.MaterialToken;
        TokenType = Define.TokenType.MaterialToken;
        
        _onProductOrder = -1;
        
        return true;
    }
    
    // Update is called once per frame
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    
    
    #region 마우스 처리로직(IsMouseClicked, IsMouseClickGroup)
    
    public override bool IsMouseClicked
    {
        get { return base.IsMouseClicked;}
        set
        {
            // //클릭할때는 생산토큰은 항상 Null
            // if(value && OnProductToken.IsValid()){
            //     OnProductToken = null;
            // }
            
            base.IsMouseClicked = value;
            
            
        }
    }
    
    #endregion
    
    #region TokenDataSetting부분(OnThisToken,UnderThisToken,OnProductToken,SetTokenData)

    // public virtual ProductController OnProductToken
    // {
    //     get => _onProductToken;
    //     set
    //     {
    //         if (_onProductToken == value) return;
    //         
    //         //빈 토큰
    //         if (!value.IsValid())
    //         {
    //             if (OnProductToken.IsValid())
    //             {
    //                 OnProductToken.BlankZoneList[_onProductOrder].onToken = false;
    //                 OnProductToken.ProductOnTokenDic.Remove(_onProductOrder);
    //             }
    //             _onProductToken = null;
    //             return;
    //         } 
    //         
    //         _onProductToken = value;
    //
    //         OnProductToken.ProductOnTokenDic[_onProductOrder] = this;
    //         OnProductToken.BlankZoneList[_onProductOrder].onToken = true;
    //         return;
    //
    //     }
    // }
    
    protected override void SetTokenData()
    {
        // //마우스 클릭중이면 
        // if (IsMouseClicked)
        // {
        //     CircleCollider2D.isTrigger = true;
        //     SpriteRenderer.sortingOrder = Constants.StartMouseTokenLayerNum;
        //     return;
        // }

        
        // //생산토큰 위면
        // if (OnProductToken.IsValid())
        // {
        //     CircleCollider2D.isTrigger = true;
        //     SpriteRenderer.sortingOrder = OnProductToken.SpriteRenderer.sortingOrder + 1;
        //     Position = OnProductToken.BlankZoneList[_onProductOrder].transform.position;
        //     return;
        // }
        
        base.SetTokenData();
        
    }

    public void OnUsed()
    {
        UnderThisToken = null;
        OnThisToken = null;
        // OnProductToken = null;
        
        Managers.Object.Despawn(this);
    }
    
    #endregion

 

 
}