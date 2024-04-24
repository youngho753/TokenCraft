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
    public Vector3 _productPosition;
    public Vector3 _clickPosition;
    private TokenController _mouseToken;
    private ProductController _mouseProduct;

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
        OnMouseDown();
        OnMouseUp();
        OnMoveMouse();
    }

    void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //IMPORTANT CASE 1. 코인 
            TokenController mouseDownToken = GetMouseDownToken();
            
            if (mouseDownToken.IsValid()) {

                /** 2. 클릭한토큰 데이터 세팅 */
                _mouseToken = mouseDownToken;
                _mouseToken.IsMouseClicked = true;
                
                /** 3. 클릭한 포지션 세팅 */
                _clickPosition = _mouseToken.Position - _mousePosition;
                
            }
            
            //IMPORTANT CASE 2. 건물
            else{
            ProductController mouseDownProduct = GetMousedownProducct();
            
            if (mouseDownProduct.IsValid()) {

                /** 2. 클릭한토큰 데이터 세팅 */
                _mouseProduct = mouseDownProduct;
                _mouseProduct.IsMouseClicked = true;
                
                /** 3. 클릭한 포지션 세팅 */
                _clickPosition = _mouseProduct.Position - _mousePosition;
                
            }
            }

        }
    }

    void OnMoveMouse()
    {
        //IMPORTANT CASE 0. 공통
        //마우스 커서 세팅
        // Cursor.visible = false;
        _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _mousePosition.z = 0;
        transform.position = _mousePosition;
        
        //IMPORTANT CASE 1. 코인 
        if (_mouseToken.IsValid()){
            _mouseToken.Position = _mousePosition + _clickPosition;
        }
            
        //IMPORTANT CASE 2. 건물
        if (_mouseProduct.IsValid()){
            _mouseProduct.Position = _mousePosition + _clickPosition;
        }
    }


    void OnMouseUp()
    {
        if (Input.GetMouseButtonUp(0))
        {
            //IMPORTANT CASE 0. 공통
            if (!_mouseToken.IsValid() && !_mouseProduct.IsValid()) return;
            
            // 클릭포지션 초기화 
            _clickPosition = new Vector3(0,0,0);
            
            
            //IMPORTANT CASE 1. 코인인풋 
            CoinInputController coinInput = GetCoinInput();
            if (coinInput.IsValid() && _mouseToken.IsValid())
            {
                _mouseToken.IntoTokenInput(); 
                coinInput.InCoinInput();
            }

            //IMPORTANT CASE 2.토큰
            else if(_mouseToken.IsValid())
            {
                TokenController mouseDownToken = GetMouseUpToken();

                /** 아래토큰에 클릭토큰주기 */
                if (mouseDownToken.IsValid()) mouseDownToken.OnThisToken = _mouseToken;
                
                //마우스 플립기능 토큰 클릭후 마우스를 움직이지 않고 바로 놨을때 *
                if (_mouseToken.Position == _mousePosition + _clickPosition)
                {
                
                }       
            }

            //IMPORTANT CASE 3.건물(의 재료칸)
            else
            {
                
            }


            //IMPORTANT CASE 99.DEFAULT 클릭한토큰 데이터세팅 
            if (_mouseToken.IsValid())
            {
                _mouseToken.IsMouseClicked = false;
                _mouseToken = null;    
            }
            if (_mouseProduct.IsValid())
            {
                _mouseProduct.IsMouseClicked = false;
                _mouseProduct = null;    
            }
        }
    }

    private TokenController GetMouseDownToken()
    {
        //해당 좌표에 있는 오브젝트 찾기
        Ray2D ray = new Ray2D(_mousePosition, Vector2.zero);

        RaycastHit2D[] targets = Physics2D.RaycastAll(_mousePosition, ray.direction, 15f);
        
        for (int i = 0; i < targets.Length; i++)
        {
            if (Util.GetTokenController(targets[i].transform.gameObject))
            {
                return Util.GetTokenController(targets[i].transform.gameObject);
            }
        }
        
        return null;
    }
    
    private TokenController GetMouseUpToken()
    {
        RaycastHit2D[] targets = Physics2D.CircleCastAll(_mousePosition, 0.5f, Vector2.zero, 0);

        for (int i = 0; i < targets.Length; i++)
        {
            //BlankZone이면 ProductToken으로 return
            // if(targets[i].transform.gameObject.GetComponent<BlankZoneController>().IsValid())
            //     return targets[i].transform.gameObject.GetComponent<BlankZoneController>().InProductToken;
        
            //Token이 아니거나 클릭중인 토큰이면 continue
            if (!Util.GetTokenController(targets[i].transform.gameObject).IsValid() ||
                Util.GetTokenController(targets[i].transform.gameObject).IsMouseClickGroup)
                continue;
            
            //클릭한 토큰의 가장 높은 토큰으로 return
            return Util.GetTokenController(targets[i].transform.gameObject).GetHighestToken();
        }
        
        return null;
    }

    private CoinInputController GetCoinInput()
    {
        RaycastHit2D[] targets = Physics2D.CircleCastAll(_mousePosition, 0.5f, Vector2.zero, 0);

        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i].transform.gameObject.GetComponent<CoinInputController>())
                return targets[i].transform.gameObject.GetComponent<CoinInputController>();
        }

        return null;
    }
    
    private ProductController GetMousedownProducct()
    {
        RaycastHit2D[] targets = Physics2D.CircleCastAll(_mousePosition, 0.5f, Vector2.zero, 0);

        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i].transform.gameObject.GetComponent<ProductController>())
                return targets[i].transform.gameObject.GetComponent<ProductController>();
        }

        return null;
    }
}
    