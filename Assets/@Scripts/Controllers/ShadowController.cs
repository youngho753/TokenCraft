using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using DG.Tweening;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class ShadowController : BaseController
{
    public SpriteRenderer SpriteRenderer;
    public TokenController parentTokenController;
    
    public float _parentFloatValue;
    
    public override bool Init()
    {
        if (base.Init() == false) return false;

        SpriteRenderer = GetComponent<SpriteRenderer>();

        _parentFloatValue = 0;

        return true;
    }

    public void SetInfo(SpriteRenderer spriteRenderer)
    {
        this.SpriteRenderer.sprite = spriteRenderer.sprite;
        this.SpriteRenderer.color = new Color(0, 0, 0, 0.66f);
        this.SpriteRenderer.sortingOrder = 0;
    }
    
    
    public virtual void Update()
    {
        transform.position = transform.parent.transform.position
                             + new Vector3(0,
                                 -0.1f - _parentFloatValue,
                                 0);

        if (_parentFloatValue == 0f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(
                1f - _parentFloatValue / 20f,
                1f - _parentFloatValue / 20f,
                0);    
        }
    }

    
}