using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidWalker : WalkerBase
{
    private CircleCollider2D cc2d;
    private BoxCollider2D bc2d;

    public override void Start()
    {
        base.Start();
        cc2d = GetComponent<CircleCollider2D>();
        bc2d = GetComponent<BoxCollider2D>();
        JumpForce = 14f;
        WalkSpeed = 4f;
    }

    public override void SetColliderActivation(bool setter)
    {
        cc2d.enabled = setter;
        bc2d.enabled = setter;

    }

    public override void BaseAction()
    {
        Debug.Log("Im supposed to use a console!");
    }

    
}
