using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureController : BaseController
{
    [SerializeField]
    protected SpriteRenderer SpriteRenderer;
    public Rigidbody2D RigidBody { get; set; }
    protected Animator Anim;
    
    Define.CreatureState _creatureState = Define.CreatureState.Idle;
    public virtual Define.CreatureState CreatureState
    {
        get { return _creatureState; }
        set
        {
            _creatureState = value;
            UpdateAnimation();
        }
    }
    
    void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        base.Init();

        RigidBody = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        return true;
    }

    public virtual void UpdateAnimation()
    {
    }
}
