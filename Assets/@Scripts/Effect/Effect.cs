using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;
using UnityEngine;

public class Effect : MonoBehaviour 
{
    protected Animator Anim;

    public void Start()    
    {
        Anim = GetComponent<Animator>();
        Anim.Play("TokenDropEffectAnim");
    }
    
    public void OnDropEffectEnd()
    {
        Managers.Resource.Destroy(gameObject);
    }

    
}