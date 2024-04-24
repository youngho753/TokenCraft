using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using DG.Tweening;
using JetBrains.Annotations;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class ProductController : BaseController
{
    public SpriteRenderer SpriteRenderer;
    public Rigidbody2D RigidBody { get; set; }
    public BoxCollider2D BoxCollider2D { get; set; } 
    
    public ProductData ProductData;
    
    
    // public Dictionary<int, TokenController> ProductOnTokenDic;
    public List<BlankZoneController> BlankZoneList;
    
    private Coroutine _coProduction;

    public List<ProductData> ProductDataList;
    private List<ProductOutputRateData> ProductOutputRateTableList;

    public virtual int DataId { get; set; }
    public virtual int makeId { get; set; }
    public virtual string PrefabName { get; set; }
    public virtual string KoreanName { get; set; }
    public virtual string EnglishName { get; set; }
    public virtual int Value { get; set; }
    public virtual string Icon { get; set; }
    
    public virtual float _floatValue { get; set; }   //공중에 떠 있는 수치

    public int _state;
    public int blankZoneCnt = 1;
    
    public bool _isMouseClicked;
    
    //그림자
    // public GameObject ShadowObject;
    // public ShadowController ShadowController;
     
     public override bool Init()
     {
         if (base.Init() == false) return false;
         
         RigidBody = GetComponent<Rigidbody2D>();
         SpriteRenderer = GetComponent<SpriteRenderer>();
         BoxCollider2D = GetComponent<BoxCollider2D>();

         // ProductOnTokenDic = new Dictionary<int, TokenController>();

         RigidBody.mass = 1000000;
     
         //BlankZone 초기 생성
         // for (int i = 0; i < startBlankZoneCnt; i++)
         // {
         //     BlankZoneController bzc = Managers.Object.Spawn<BlankZoneController>(transform.position,0,"BlankZone");
         //     bzc.order = i;
         //     BlankZoneList.Add(bzc);
         //     bzc.InProductToken = this;
         // }

         _coProduction = null;

         _isMouseClicked = false;

         DropProduct();
         
         
         return true;
     }
//
     public virtual void FixedUpdate()
     {
         setProductData();
     }

     public virtual void SetInfo(int productId)
     {
         // public int DataId;
         // public int MakerId;
         // public List<int> InputList;
         // public int MakeOrder;
         // public int MakeId;
         
         
         DataId = productId;
         Dictionary<int, Data.ProductData> dict = Managers.Data.ProductDic;
         ProductData = dict[productId];
         // SpriteName = ;
         // KoreanName = ProductData.KoreanName;
         // EnglishName = ProductData.EnglishName;
         // Value = ProductData.Value;
        
         Sprite sprite = Managers.Resource.Load<Sprite>($"{ProductData.SpriteName}");
         SpriteRenderer.sprite = sprite;
         
         
         // ProductList에서 Maker가 자신인 데이터를 모두 찾기.
         // ProductDataList = Managers.Data.ProductDic.Where(w => w.Value.MakerId == ProductData.DataId).Select(s => s.Value)
         //     .ToList();
         
         // foreach(ProductData productData in Managers.Data.ProductDic.Where(w=>w.Value.MakerId == ProductData.DataId).Select(s=>s.Value))
         // {
         //     ProductDataList.Add(productData);
         // }
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

             // ShadowController._parentFloatValue = value;


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
             // this.ShadowController.SpriteRenderer.sortingOrder = value - 10;
         }
     }
     
     #endregion Deligater
     
     #region 마우스 처리로직(IsMouseClicked, IsMouseClickGroup)
    
     public virtual bool IsMouseClicked
     {
         get { return _isMouseClicked;}
         set
         {
             _isMouseClicked = value;

             //클릭할때는 UnderToken은 항상 Null
             if (value)
             {
                 FloatValue = 0.2f;
             }
             else
             {
                 FloatValue = 0f;
             }
         }
     }
     
     #endregion
     
     
     #region TokenDataSetting부분(OnThisToken,UnderThisToken,SetTokenData)
//     
//     public override TokenController OnThisToken
//     {
//         get => base.OnThisToken;
//         set
//         {
//             if (value.IsValid() && value.ObjectType == Define.ObjectType.MaterialToken)
//             {
//                 AddToken(value);
//                 return;
//             }
//
//             base.OnThisToken = value;
//         }
//     }

    protected virtual void setProductData()
    {
        //마우스 클릭중이면 
        if (IsMouseClicked)
        {
            BoxCollider2D.isTrigger = true;
            SortingOrder = Constants.StartMouseTokenLayerNum;
            return;
        }
        
        //공중에 떠있으면
        if (FloatValue > 0)
        {
            BoxCollider2D.isTrigger = true;
            SortingOrder = Constants.FloatTokenLayerNum;
            return;
        }

        BoxCollider2D.isTrigger = false;
        SortingOrder = Constants.StartTokenLayerNum;
        

    }
     
     #endregion
//
//     #region 생산토큰 전용로직
//
//     public bool ProductCheck()
//     {
//         bool isProduct = false;
//
//         //올려져 있는 토큰Id List화
//         List<int> onTokenDatas = new List<int>();
//         
//         foreach (int key in ProductOnTokenDic.Keys)
//         {
//             onTokenDatas.Add(ProductOnTokenDic.GetValueOrDefault(key,null).TokenData.DataId);
//         }
//
//         //만들 수 있는 makeId중 가장 순서가 높은 makeID 추출
//         //InputList가 없는 데이터는 0, 0보다 크면 추출한것
//         makeId = ProductDataList.Where(w => Util.isEqual(w.InputList, onTokenDatas)).OrderByDescending(o => o.MakeOrder).First().MakeId;
//
//         if (makeId <= 0) return isProduct;
//     
//         ProductOutputRateTableList = Managers.Data.ProductOutputTableDic.Where(w => w.Key.Equals(makeId))
//             .Select(s => s.Value.ProductOutputRateTable).First();
//             
//         if (makeId > 0) isProduct = true;
//
//         return isProduct;
//     }
//     
//     public IEnumerator CoStartProduction()
//     {
//         while (true)
//         {
//             yield return new WaitForSeconds(3f);
//
//             List<int> willProductOutputs = new List<int>();
//             int baseOutput = 0;
//             for (int i = 0; i < ProductOutputRateTableList.Count; i++)
//             {
//                 
//                 float random = UnityEngine.Random.value;
//                 
//                 // baseItem Setting
//                 if (ProductOutputRateTableList[i].IsBaseItem) baseOutput = i; 
//                 
//                 //확률성공
//                 if (ProductOutputRateTableList[i].Rate < random)
//                 {
//                     willProductOutputs.Add(ProductOutputRateTableList[i].Output);
//                 }
//             }
//
//             // 모두실패 -> BaseItem 생성
//             if (willProductOutputs.Count == 0)
//             {
//                 willProductOutputs.Add(ProductOutputRateTableList[baseOutput].Output);                      
//             }
//             
//             //생성
//             foreach (int i in willProductOutputs)
//             {
//                 Managers.Object.SpawnToken<MaterialTokenController>(Managers.Game.Mouse._mousePosition, i, "MaterialToken");
//                 Managers.Object.Despawn(ProductOnTokenDic[ProductOnTokenDic.Count-1]);
//             }
//         }
//     }
//     
//     public static int GetRandomToken(int makeId)
//     {
//         
//         
//         // {
//         //     
//         // }
//         // float randomValue = UnityEngine.Random.value;
//         //
//         
//         
//         
//         
//
//         // return EquipmentGrade.Common;
//         return 0;
//     }
//
//     public virtual void AddToken(TokenController token)
//     {
//         //BlankZone에 토큰이 없을경우
//         if (inBlankZoneOrder == -1) return;
//
//         //값이 있을경우 return
//         if (ProductOnTokenDic.ContainsKey(inBlankZoneOrder))
//         {
//             return;
//         }
//         
//         // 자원토큰일때만 Setting
//         if (token.GetComponent<MaterialTokenController>().IsValid())
//         {
//             token.GetComponent<MaterialTokenController>().OnProductToken = this;
//             
//         }
//     }
//     
//     public virtual void RemoveToken(int order)
//     {
//         ProductOnTokenDic.Remove(order);
//         
//         BlankZoneList[inBlankZoneOrder].onToken = false;
//     }
//
//     public virtual void OnBlankZoneEnter(int order)
//     {
//         //이미 토큰이 올려져있으면 데이터세팅 X
//         if (ProductOnTokenDic.ContainsKey(order)) return;
//         
//         inBlankZoneOrder = order;
//     }
//     
//     public virtual void OnBlankZoneExit(int order)
//     {
//         if (inBlankZoneOrder == order) inBlankZoneOrder = -1;
//     }
//
//     #endregion
//     
//     
    #region 애니메이션 부분

    public virtual void DropProduct()
    {
        DOTween.To(() => FloatValue, x => FloatValue = x, 0, 0.25f)
            .SetEase(Ease.InExpo)
            .SetId("DropProduct")
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
