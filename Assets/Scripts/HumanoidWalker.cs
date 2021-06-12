using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidWalker : WalkerBase
{

    public override void BaseAction()
    {
        Debug.Log("Im supposed to use a console!");
    }

    public override void GetControlled()
    {
        base.GetControlled();
    }
}
