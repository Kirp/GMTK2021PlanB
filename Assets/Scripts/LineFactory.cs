using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineFactory : MonoBehaviour
{
    public GameObject LinePrefab;

    public GameObject GetLine(Transform origin, Transform target)  
    {
        GameObject lineInstance;
        lineInstance = Instantiate(LinePrefab);
        lineInstance.GetComponent<ThreePointLiner>().LoadTargetTransforms(origin, target);
        return lineInstance;
    }
}
