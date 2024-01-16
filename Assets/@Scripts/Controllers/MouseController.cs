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

    void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            /** 1. 클릭한 토큰 찾기 */
            TokenController mouseDownToken = GetMouseDownToken();
            if (!mouseDownToken.IsValid()) return;

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
        if (_mouseToken is null ) return;
        _mouseToken.position = _mousePosition;
    }


    void OnMouseUp()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (!_mouseToken.IsValid()) return;

            TokenController mouseDownToken = GetMouseUpToken();
            
            /** mouseClickToken에 MouseUp 주기 */
            _mouseToken.isMouseClicked = false;
            
            /** 아래토큰에 클릭토큰주기 */
            if(mouseDownToken.IsValid()) mouseDownToken.SetOnThisToken(_mouseToken);

            /** ._mousetokenStack 세팅 */
            _mouseToken = null;

        }
    }

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
    
    private TokenController GetMouseUpToken()
    {
        RaycastHit2D[] targets = Physics2D.CircleCastAll(_mousePosition, 0.5f, Vector2.zero, 0);

        TokenController tc = null;
        for (int i = 0; i < targets.Length; i++)
        {
            if (Util.GetTokenController(targets[i].transform.gameObject).IsValid() && Util.GetTokenController(targets[i].transform.gameObject).isMouseClicked) 
                continue;

            return Util.GetTokenController(targets[i].transform.gameObject);
        }
        
        return null;
    }
}
    