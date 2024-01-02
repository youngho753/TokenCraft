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
    private Stack<TokenController> _mouseTokenStack;

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
            

            /** 2. 클릭한 토큰의 아래 토큰 Get */ 
            Stack<TokenController> underTokenStack = Util.GetUnderTokenStack(mouseDownToken); 

            
            /** 3. 클릭한 토큰의 위 토큰 Get */
            Stack<TokenController> onTokenStack = Util.GetOnTokenStack(mouseDownToken);
            
            
            /** 4. 건물데이터 제거 */
            SetMouseUpBackgound(underTokenStack);
            SetMouseUpBackgound(onTokenStack);
            
            
            /** 5. 토큰스택 tokenDic 정리 */
            Util.SettingTokenStack(underTokenStack, true);
            Util.SettingTokenStack(onTokenStack, true);
            

            /** 6. _mouseTokenStack 세팅 */
            _mouseTokenStack = onTokenStack;

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
        if (_mouseTokenStack == null ) return;
        Util.MoveTokenStack(_mouseTokenStack, _mousePosition);
    }


    void OnMouseUp()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (_mouseTokenStack == null) return;
            
            // 마우스를 놨을때 
            // 1.UI  2.적  3.건물 4.일반토큰 순으로 체크를 해서 분기를 태움.  
            
            /** 1. 마우스아래 GameObject 타입 구하기 */
            ObjectType objectType = GetMouseUpObjectType();
            
            
            /** case 3. 건물위에 놨을때 */
            /** 1. 마우스 놨을때 건물 Get */
            TokenBackgroundController mouseUpTokenBackground = GetMouseUpTokenBackground();

            // 건물클릭
            if (mouseUpTokenBackground != null)
            {
                // Util.MoveTokenStack(_mouseTokenStack, mouseUpTokenBackground.transform.position + new Vector3((1.2f * mouseUpTokenBackground.TokenStacks.Count) -0.6f,-0.3f,0f));
                /** 1. */
                
                
                // mouseUpTokenBackground.AddTokenStack(_mouseTokenStack);
                foreach (TokenController tc in _mouseTokenStack)
                {
                    // tc.TokenBackground = mouseUpTokenBackground;
                }
                
                Debug.Log("건물 클릭!!");
            }
            else
            {
                /** case 2. 토큰위에 놨을때 */
                /** 1. 마우스 놨을때 토큰 스택 Get */
                Stack<TokenController> mouseUpTokenStack = GetMouseUpTokenStack();


                /** 2. 바닥에 있는 토큰스택과 마우스로 들고 있는 토큰스택 concat */
                Stack<TokenController> concatTokenStack = Util.ConcatTokenStack(mouseUpTokenStack, _mouseTokenStack);


                /** 3. 토큰스택 tokenDic 정리 */
                Util.SettingTokenStack(null, false, _mouseTokenStack.Peek());
                Util.SettingTokenStack(concatTokenStack);
            }

            /** 4._mousetokenStack 세팅 */
            _mouseTokenStack = null;

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
            if (targets[i].transform.gameObject.GetComponent<TokenController>())
            {
                return targets[i].transform.gameObject.GetComponent<TokenController>();
            }
        }
        
        return null;
    }

    private void SetMouseUpBackgound(Stack<TokenController> tokenStack)
    {
        if (tokenStack == null) return;
        
        //가장 아래 토큰이면 건물의 토큰스택에서 빼줘야함
        TokenController token = Util.GetLowestToken(tokenStack);
        if (token.groupNum == token.pkGroupNum && token.InTokenBackground != null)
        {
            // token.TokenBackground.TokenStacks.Remove(tokenStack);
        }
        
        //토큰에서 건물을 빼기
        foreach (TokenController tc in tokenStack)
        {
            tc.InTokenBackground = null;
        }
    }

    #endregion

    #region MouseMove

    
    
    #endregion
    
    #region MouseUp

    private ObjectType GetMouseUpObjectType()
    {
        RaycastHit2D[] targets = Physics2D.CircleCastAll(_mousePosition, 0.5f, Vector2.zero, 0);

        // for (int i = 0; i < targets.Length; i++)
        // {
        //     if (targets[i].transform.gameObject.GetComponent<BlankTokenController>())
        //         return ObjectType.ProductToken;
        // }
        //
        //for (int i = 0; i < targets.Length; i++)
        // {
        //     if (targets[i].transform.gameObject.GetComponent<BlankTokenController>())
        //         return ObjectType.ProductToken;
        // }
        //
        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i].transform.gameObject.GetComponent<BlankTokenController>())
                return ObjectType.ProductToken;
        }
        
        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i].transform.gameObject.GetComponent<MaterialTokenController>())
                return ObjectType.MaterialToken;
        }

        
        return ObjectType.Null;
    }
    
    private TokenBackgroundController GetMouseUpTokenBackground()
    {
        RaycastHit2D[] targets = Physics2D.CircleCastAll(_mousePosition, 0.5f, Vector2.zero, 0);

        Stack<TokenController> getStack = null;
        Stack<TokenController> copyStack = null;

        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i].transform.gameObject.GetComponent<TokenBackgroundController>())
            {
                return targets[i].transform.gameObject.GetComponent<TokenBackgroundController>();
            }
        }

        return null;
    }

    
    private Stack<TokenController> GetMouseUpTokenStack()
    {
        RaycastHit2D[] targets = Physics2D.CircleCastAll(_mousePosition, 0.5f, Vector2.zero, 0);

        Stack<TokenController> getStack = null;
        Stack<TokenController> copyStack = null;
        
        //GroupNum이 MouseToken과 다른 것의 토큰 Get
        TokenController tc = null;
        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i].transform.gameObject.GetComponent<TokenController>().groupNum == _mouseTokenStack.Peek().groupNum) 
                continue;

            //단일 토큰을 위해 사용
            tc = targets[i].transform.gameObject.GetComponent<TokenController>();

            getStack = Managers.Game._tokenStackDic.GetValueOrDefault(
                targets[i].transform.gameObject.GetComponent<TokenController>().groupNum, null);

            copyStack = Util.DeepCopy(getStack);

            break;
        }

        // 빈 곳에 내려놨을 경우
        if (tc == null) return null;
        
        //단일 토큰위에 내려놨을 경우
        if (copyStack == null)
        {
            copyStack = new Stack<TokenController>();
            copyStack.Push(tc);
            return copyStack;
        }
        
        // 다중 토큰위에 내려놨을 경우
        return copyStack;
    }
    
   
    
    #endregion
    

   

    
}
