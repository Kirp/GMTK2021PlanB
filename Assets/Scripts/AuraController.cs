using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraController : MonoBehaviour
{
    public GameObject singleAura;
    private Animator animator;

    private void Start()
    {
        animator = singleAura.GetComponent<Animator>();
    }

    public void CallAura(Vector3 position, float size)
    {
        singleAura.transform.position = position;
        singleAura.transform.localScale = new Vector3(size, size, size);
        animator.Play("Base Layer.appear", 0);
    }

    public void CallAuraBeParent(GameObject adopter, float size)
    {
        singleAura.transform.SetParent(adopter.transform, true);
        singleAura.transform.position = adopter.transform.position;
        singleAura.transform.localScale = new Vector3(size, size, size);
        animator.Play("Base Layer.appear", 0);
    }
}
