using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class ProductTokenController : TokenController
{
    public Dictionary<int, TokenController> ProductOnTokenDic;
    public List<BlankZoneController> BlankZoneList;
    
    private Coroutine _coProduction;

    
    //처음 Init할때의 BlankZone개수 엑셀에서 읽어와야함
    public int startBlankZoneCnt = 2;
    
    //현재 BlankZone개수 
    public int blankZoneCnt = 2;
    
    //BlankZone에서 충돌처리받을때 값을 세팅 
    public int inBlankZoneOrder = -1;
    
    public override bool Init()
    {
        if (base.Init() == false) return false;
        
        ProductOnTokenDic = new Dictionary<int, TokenController>();

        RigidBody.mass = 1000000;
    
        //BlankZone 초기 생성
        for (int i = 0; i < startBlankZoneCnt; i++)
        {
            BlankZoneController bzc = Managers.Object.Spawn<BlankZoneController>(transform.position,0,"BlankZone");
            bzc.order = i;
            BlankZoneList.Add(bzc);
            bzc.InProductToken = this;
        }

        _coProduction = null;
        
        
        return true;
    }

    public override void Update()
    {
        base.Update();

        if (ProductCheck())
        {
            if (_coProduction == null)
            _coProduction = StartCoroutine(CoStartProduction()); 
        }
        else
        {
            if (_coProduction != null)
            {
                StopCoroutine(_coProduction);
                _coProduction = null;
            }
            
        }
    }
    
    public override Vector3 Position
    {
        get { return transform.position; }
        set
        {
            base.Position = value;
            
            //ProductToken전용
            
            //BlankZone 이동
            for (int i = 0; i < BlankZoneList.Count; i++)
            {
                BlankZoneList[i].transform.position = transform.position + new Vector3(-0.6f + (1.2f * i), 1f, 0);
            }
        }
    }
    
    
    
    #region TokenDataSetting부분(OnThisToken,UnderThisToken,SetTokenData)
    
    public override TokenController OnThisToken
    {
        get => base.OnThisToken;
        set
        {
            if (value.IsValid() && value.ObjectType == Define.ObjectType.MaterialToken)
            {
                AddToken(value);
                return;
            }

            base.OnThisToken = value;
        }
    }
    
    #endregion

    #region 생산토큰 전용로직

    public bool ProductCheck()
    {
        bool isProduct = true;
        for (int i = 0; i < blankZoneCnt; i++)
        {
            if (ProductOnTokenDic.TryGetValue(i, out TokenController tc) == false)
            {
                isProduct = false;
                break;
            }
        }

        return isProduct;
    }
    
    public IEnumerator CoStartProduction()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);

            for (int i = 0; i < blankZoneCnt; i++)
            {
                if(ProductOnTokenDic.TryGetValue(i, out TokenController tc))
                {
                    tc.GetHighestToken().GetComponent<MaterialTokenController>().OnUsed();
                }
            }
            MaterialTokenController mtc = Managers.Object.SpawnToken<MaterialTokenController>(Position - new Vector3(0,Random.Range(1f,2f),0), 0, "MaterialToken");
            mtc.GetHighestToken().OnThisToken = Managers.Object.SpawnToken<MaterialTokenController>(
                    Position - new Vector3(0, Random.Range(1f, 2f), 0), 0, "MaterialToken");
            mtc.GetHighestToken().OnThisToken = Managers.Object.SpawnToken<MaterialTokenController>(
                Position - new Vector3(0, Random.Range(1f, 2f), 0), 0, "MaterialToken");
            //     nearToken.GetHighestToken().OnThisToken = mtc;

            // MaterialTokenController nearToken = Util.GetNearestSameMaterialToken(Position, 100);

            // if (nearToken.IsValid())
            // {
            //     MaterialTokenController mtc = Managers.Object.SpawnToken<MaterialTokenController>(Position - new Vector3(0,Random.Range(1f,2f),0), 0, "MaterialToken");
            //     nearToken.GetHighestToken().OnThisToken = mtc;
            // }
            //
            // //TODO 이 아래는 테스트용 나중에 없애야함 
            // if (nearToken.IsValid())
            // {
            //     MaterialTokenController mtc = Managers.Object.SpawnToken<MaterialTokenController>(Position - new Vector3(0, Random.Range(1f, 2f), 0), 0, "MaterialToken");
            //     nearToken.GetHighestToken().OnThisToken = mtc;
            // }
            //
            // if (nearToken.IsValid())
            // {
            //     MaterialTokenController mtc = Managers.Object.SpawnToken<MaterialTokenController>(Position - new Vector3(0,Random.Range(1f,2f),0), 0, "MaterialToken");
            //     nearToken.GetHighestToken().OnThisToken = mtc;
            // }
        }
    }

    public virtual void AddToken(TokenController token)
    {
        //BlankZone에 토큰이 없을경우
        if (inBlankZoneOrder == -1) return;

        //값이 있을경우 return
        if (ProductOnTokenDic.ContainsKey(inBlankZoneOrder))
        {
            return;
        }
        
        // 자원토큰일때만 Setting
        if (token.GetComponent<MaterialTokenController>().IsValid())
        {
            token.GetComponent<MaterialTokenController>().OnProductToken = this;
            
        }
    }
    
    public virtual void RemoveToken(int order)
    {
        ProductOnTokenDic.Remove(order);
        
        BlankZoneList[inBlankZoneOrder].onToken = false;
    }

    public virtual void OnBlankZoneEnter(int order)
    {
        //이미 토큰이 올려져있으면 데이터세팅 X
        if (ProductOnTokenDic.ContainsKey(order)) return;
        
        inBlankZoneOrder = order;
    }
    
    public virtual void OnBlankZoneExit(int order)
    {
        if (inBlankZoneOrder == order) inBlankZoneOrder = -1;
    }

    #endregion
    
    
}

  

   
    
   
