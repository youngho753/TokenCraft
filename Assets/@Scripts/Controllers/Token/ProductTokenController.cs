using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class ProductTokenController : TokenController
{
    public Dictionary<int, TokenController> ProductOnTokenDic;

    public int startBlankZoneCnt = 2;
    public int inBlankZoneOrder = -1;
    
    public override bool Init()
    {
        if (base.Init() == false) return false;
        
        ProductOnTokenDic = new Dictionary<int, TokenController>();

        RigidBody.mass = 1000000;
    
        //BlankZone 초기 생성
        for (int i = 0; i < startBlankZoneCnt; i++)
        {
            BlankZoneController bzc = Managers.Object.Spawn<BlankZoneController>(transform.position + new Vector3(-0.6f + (1.2f * i), 0.2f, 0),0,"BlankZone");
            bzc.order = i;
        }
        
        
        return true;
    }

    public virtual void AddTokenStack(TokenController token)
    {
        //BlankZone에 토큰이 없을경우
        if (inBlankZoneOrder == -1) return;
        
        ProductOnTokenDic.Add(inBlankZoneOrder, token);
        
        token.OnProductTokenOrder = inBlankZoneOrder;
    }
    
    public virtual void RemoveTokenStack(int order)
    {
        ProductOnTokenDic.Remove(order);
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

  

    // public IEnumerator CoStartProduction()
    // {
    //     while (true)
    //     {
    //         List<TokenController> onTokenList = new List<TokenController>();
    //         
    //         foreach(BlankTokenController blankTokenController in TokenBackground.BlankTokenDic.Values)
    //         {
    //             onTokenList.Add(blankTokenController.onTokenStack.Peek());
    //         }
    //
    //         foreach (TokenController tc in onTokenList)
    //         {
    //             tc.onUsed();
    //         }
    //         
    //         yield return new WaitForSeconds(3f);
    //         
    //     }
    // }
    
   
}
