using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BlankZoneController : BaseController
{
    public ProductTokenController InProductToken;
    
    public bool onToken;
    
    public int order;
    public int _spriteType;
    
    public override bool Init()
    {
        base.Init();

        onToken = false;
        
        return true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (onToken) return;
        
        MaterialTokenController materialToken = collision.GetComponent<MaterialTokenController>();

        if (materialToken.IsValid() == false)
            return;

        InProductToken.OnBlankZoneEnter(order);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (onToken) return;
        
        MaterialTokenController materialToken = collision.GetComponent<MaterialTokenController>();

        if (materialToken.IsValid() == false)
            return;

        InProductToken.OnBlankZoneExit(order);
    }
}
