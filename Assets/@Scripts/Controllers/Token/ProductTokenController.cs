using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class ProductTokenController : TokenController
{ 
    [SerializeField]
    public TokenBackgroundController TokenBackground;
    
    public override bool Init()
    {
        if (base.Init() == false) return false;

        RigidBody = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        CircleCollider2D = GetComponent<CircleCollider2D>();

        RigidBody.mass = 1000000;
        
        return true;
    }
   
    public override void MoveToTarget(Vector3 position, float time, bool snapping)
    {
        transform.position = position;
        
        TokenBackground.MoveToTarget(transform.position + new Vector3(0,0.8f,0));
        
        StartCoroutine(CoStartProduction()); // 코루틴을 시작하는 함수

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
    
    public virtual void RemoveToken(TokenController removeToken)
    {
        TokenBackground.RemoveOnToken(removeToken);
    }

    public IEnumerator CoStartProduction()
    {
        while (true)
        {
            List<TokenController> onTokenList = new List<TokenController>();
            
            foreach(BlankTokenController blankTokenController in TokenBackground.BlankTokenDic.Values)
            {
                onTokenList.Add(blankTokenController.onTokenStack.Peek());
            }

            foreach (TokenController tc in onTokenList)
            {
                tc.onUsed();
            }
            
            yield return new WaitForSeconds(3f);
            
        }
    }
    
    // private IEnumerator CoStartDotDamage()
    // {
    //     while (true)
    //     {
    //         yield return new WaitForSeconds(1f);
    //         foreach (CreatureController target in _enteredColliderList)
    //         {
    //             target.OnDamaged(_owner, _skill);
    //         }
    //     }
    // }
}
