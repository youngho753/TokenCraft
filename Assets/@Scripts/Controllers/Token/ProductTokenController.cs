using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ProductTokenController : TokenController
{ 
    [SerializeField]
    public TokenBackgroundController TokenBackground;
   
    public override void MoveToTarget(Vector3 position, float time, bool snapping)
    {
        transform.position = position;
        
        TokenBackground.MoveToTarget(transform.position + new Vector3(0,0.8f,0));
        
    }

    public virtual void SetTokenStack(int key, Stack<TokenController> tokenStack)
    {
        
        //제거하는경우
        if (tokenStack == null || tokenStack.Count == 0)
        {
            RemoveTokenStack(key);
            return;
        }
        
        //추가하는 경우
        AddTokenStack(key, tokenStack);
    }
    public virtual void AddTokenStack(int key, Stack<TokenController> tokenStack)
    {
        TokenBackground.AddTokenStack(key, tokenStack);
    }
    
    public virtual void RemoveTokenStack(int key)
    {
        TokenBackground.RemoveTokenStack(key);
    }
}
