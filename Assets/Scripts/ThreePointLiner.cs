using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreePointLiner : MonoBehaviour
{

    private LineRenderer LR;

    public Transform originTransform = null;
    public Transform targetTransform = null;
    public Vector2 midWayPoint = Vector2.zero;
    public Vector2 midWayPoint2 = Vector2.zero;

    public float LineLimit = 20f;

    // Start is called before the first frame update
    void Start()
    {
        LR = GetComponent<LineRenderer>();
        //LR.widthMultiplier = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (targetTransform != null)
        {
            midWayPoint = Vector2.Lerp(originTransform.position, targetTransform.position, 0.3f);
            midWayPoint.x += Random.Range(-0.5f,0.5f);
            midWayPoint.y += Random.Range(-0.5f,0.5f);

            midWayPoint2 = Vector2.Lerp(originTransform.position, targetTransform.position, 0.6f);
            midWayPoint2.x += Random.Range(-0.5f,0.5f);
            midWayPoint2.y += Random.Range(-0.5f,0.5f);



            LR.SetPosition(0, originTransform.position);
            LR.SetPosition(1, midWayPoint);
            LR.SetPosition(2, midWayPoint2);
            LR.SetPosition(3, targetTransform.position);


            LR.widthMultiplier = ChangeWidthBasedOnReverseLimit();
        }

        
    }

    public float ChangeWidthBasedOnReverseLimit()
    {
        var targ = GetDistanceBetweenTargets();
        var currentW = LineLimit - targ;
        if (currentW < 0) currentW = 1;
        if (currentW > LineLimit) currentW = LineLimit;
        var revRatio = currentW / LineLimit;
        revRatio *= 0.2f;

        return revRatio;
    }

    public void LoadTargetTransforms(Transform origin, Transform target)
    {
        originTransform = origin;
        targetTransform = target;
    }

    public float GetDistanceBetweenTargets()
    {
        return Vector2.Distance(originTransform.position, targetTransform.position);
    }
}
