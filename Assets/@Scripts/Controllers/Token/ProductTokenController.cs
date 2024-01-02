using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ProductTokenController : TokenController
{ 
    public TokenBackgroundController TokenBackground;
   
    public override void MoveToTarget(Vector3 position, float time, bool snapping)
    {
        transform.position = position;
        
        TokenBackground.MoveToTarget(transform.position + new Vector3(0,0.8f,0));
        
    }
}
