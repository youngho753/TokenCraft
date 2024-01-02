using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NatureTokenController : ProductTokenController
{
   public bool isMouseHolding = false;


   

   public void SetBackgroundSprite()
   {
   }
   public override void SettingToken(int groupNum, int idx, bool isMoveTokenStack = false)
   {
      //마우스로 움직이고 있는 토큰일 경우
      if (isMoveTokenStack)
      {
         CircleCollider2D.isTrigger = true;
         SpriteRenderer.sortingOrder = Constants.StartMouseTokenLayerNum + idx;
         
         // _background.SetActive(false);
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
         // _background.SetActive(true);
            
            
      }
        
      //모든 공통
      this.groupNum = groupNum;
   }
   

}