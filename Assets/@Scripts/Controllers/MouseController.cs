using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Numerics;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

using static Define;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;


public class MouseController : MonoBehaviour
{
    public Vector3 _mousePosition;
    private TokenController _mouseToken;

    void Start()
    {
        Cursor.visible = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_mousePosition, 0.2f);
    }

    void Update()
    {
        OnMoveMouse();
        OnMouseDown();
        OnMouseUp();
        
    }

    void FixedUpdate()
    {

    }

    void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            /** 1. 클릭한 토큰 찾기 */
            TokenController mouseDownToken = GetMouseDownToken();
            if (mouseDownToken == null) return;

            /** 2. _mouseTokenStack 세팅 */
            mouseDownToken.isMouseClicked = true;
            _mouseToken = mouseDownToken;

        }
    }

    void OnMoveMouse()
    {
        //마우스 커서 세팅
        // Cursor.visible = false;
        _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _mousePosition.z = 0;
        transform.position = _mousePosition;

        
        /** 토큰 움직임 */
        if (_mouseToken == null ) return;
        _mouseToken.position = _mousePosition;
    }


    void OnMouseUp()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (_mouseToken == null) return;
            
        //     // 마우스를 놨을때 
        //     
        //     /** 1. 마우스 놨을때 토큰 스택 Get */
        //     Stack<TokenController> mouseUpTokenStack = GetMouseUpTokenStack();
        //     
        //     
        //     /** 1. 마우스 놨을때 데이터 세팅 */
        //     SetMouseUpDataSetting(mouseUpTokenStack,_mouseTokenStack);
        //
        //
        //     /** 2. 바닥에 있는 토큰스택과 마우스로 들고 있는 토큰스택 concat */
        //     Stack<TokenController> concatTokenStack = Util.ConcatTokenStack(mouseUpTokenStack, _mouseTokenStack);
        //
        //
        //     /** 3. 토큰스택 tokenDic 정리 */
        //     Util.SettingTokenStack(null, false, _mouseTokenStack.Peek());
        //     Util.SettingTokenStack(concatTokenStack);
        // // }

            /** 4._mousetokenStack 세팅 */
            _mouseToken = null;

        }
    }

    #region MouseDown
    private TokenController GetMouseDownToken()
    {
        //해당 좌표에 있는 오브젝트 찾기
        Ray2D ray = new Ray2D(_mousePosition, Vector2.zero);

        RaycastHit2D[] targets = Physics2D.RaycastAll(_mousePosition, ray.direction, 15f);
        
        for (int i = 0; i < targets.Length; i++)
        {
            if (Util.GetTokenController(targets[i].transform.gameObject, Constants.ExceptBlankTokenContoller))
            {
                return Util.GetTokenController(targets[i].transform.gameObject, Constants.ExceptBlankTokenContoller);
            }
        }
        
        return null;
    }

  

    #endregion

    #region MouseMove

    
    
    #endregion
    
    #region MouseUp
    
   
    
    #endregion
    

   

    
}

// 1.UI  2.적  3.건물 4.일반토큰
/** case 3. 건물위에 놨을때 */
/** 1. 마우스 놨을때 건물 Get */
// TokenBackgroundController mouseUpTokenBackground = GetMouseUpTokenBackground();
//
// // 건물클릭
// if (mouseUpTokenBackground != null)
// {
//     // Util.MoveTokenStack(_mouseTokenStack, mouseUpTokenBackground.transform.position + new Vector3((1.2f * mouseUpTokenBackground.TokenStacks.Count) -0.6f,-0.3f,0f));
//     /** 1. */
//     
//     
//     // mouseUpTokenBackground.AddTokenStack(_mouseTokenStack);
//     foreach (TokenController tc in _mouseTokenStack)
//     {
//         // tc.TokenBackground = mouseUpTokenBackground;
//     }
//     
//     Debug.Log("건물 클릭!!");
// }
// else
// {
/** case 2. 토큰위에 놨을때 */