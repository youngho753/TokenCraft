using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;
using UnityEngine;

public class TokenDropEffect : MonoBehaviour 
{
    protected Animator Anim;

    public void Start()    
    {
        Anim = GetComponent<Animator>();
        Anim.Play("TokenDropEffectAnim");
    }
    
    public void OnEffectEnd()
    {
        Managers.Resource.Destroy(gameObject);
    }

    
}