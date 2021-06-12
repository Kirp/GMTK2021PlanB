using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WalkerBase : MonoBehaviour
{

    private Rigidbody2D rb;
    private float WalkSpeed = 5f;
    private float currentDirection = 1;
    private bool hasFreeWill = true;
    private float sightRayLength = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public virtual void GetControlled()
    {
        hasFreeWill = false;
        rb.velocity = Vector2.zero;
    }

    public virtual void ControllerOrders(float moveDirection)
    {
        rb.velocity = new Vector2(WalkSpeed * moveDirection, rb.velocity.y);
        if (moveDirection > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        if (moveDirection < 0)
        { 
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public abstract void BaseAction();
    

    public virtual void SelfMovement()
    {
        if (hasFreeWill)
        {
            rb.velocity = new Vector2(WalkSpeed * currentDirection, rb.velocity.y);

            var shootVector = Vector2.right;
            if (currentDirection > 0)
            {
                shootVector = Vector2.right;
                transform.localScale = new Vector3(1, 1, 1);


            }
            if (currentDirection < 0)
            {
                shootVector = Vector2.left;
                transform.localScale = new Vector3(-1, 1, 1);

            }

            RaycastHit2D hit = Physics2D.Raycast(transform.position, shootVector, sightRayLength, (1<<6));
            if(hit.collider != null)
            {
                currentDirection *= -1;
            }

            Debug.DrawRay(transform.position, shootVector * sightRayLength, Color.red);


        }
    }

    

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        SelfMovement();
    }



}
