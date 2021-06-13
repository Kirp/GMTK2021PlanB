using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WalkerBase : MonoBehaviour
{

    private Rigidbody2D rb;
    protected float WalkSpeed = 5f;
    private float currentDirection = 1;

    [SerializeField]
    private bool hasFreeWill = true;
    protected float sightRayLength = 0.5f;
    protected float nextBlockRayLengthMultiplier = 4f;
    //private float controllerMoveDirection;
    protected float grabRange = 5f;
    

    private bool isOnGround = false;
    protected float JumpForce = 15f;

    private SpriteRenderer sr;
    
    protected float groundCheckRayLength = 1.15f;

    
    protected GameObject lineToVictim = null;

    [SerializeField]
    private float distanceToVictim = 0;

    protected float DistanceLimit = 20f;

    protected bool PossessedButAsleep = false;
    
    // Start is called before the first frame update
    public virtual void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    public virtual void GetControlled()
    {
        hasFreeWill = false;
        rb.velocity = Vector2.zero;

    }

    public virtual bool HasLineToVictim()
    {
        if(lineToVictim = null)
        {
            return false;
        }

        return true;
    }

    public virtual void RenewControl()
    {
        Destroy(lineToVictim);
        hasFreeWill = false;
        rb.velocity = Vector2.zero;
        AreWeColoredDead(false);
        SetColliderActivation(true);
        rb.isKinematic = false;
    }

    public virtual void ReleaseControl()
    {
        Debug.Log("Freedom!");
        hasFreeWill = true;
        //Destroy(lineToVictim);
        SetColliderActivation(true);
        rb.isKinematic = false;
        currentDirection = -1;
    }

    public virtual void AreWeColoredDead(bool flag)
    {
        Color grab = sr.color;
        grab.a = flag==true?0.5f:1f;
        sr.color = grab;

        PossessedButAsleep = flag;
    }

    public virtual bool ControlledButAsleep()
    {
        return PossessedButAsleep;
    }

    

    public virtual void ControllerOrders(float moveDirection)
    {
        /*
        rb.velocity = new Vector2(WalkSpeed * moveDirection, rb.velocity.y);
        if (moveDirection > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        if (moveDirection < 0)
        { 
            transform.localScale = new Vector3(-1, 1, 1);
        }
        */
        currentDirection = moveDirection;
    }

    public abstract void BaseAction();

    public abstract void SetColliderActivation(bool setter);

    public virtual GameObject PossessAuraEmit()
    {
        if (!isOnGround) return null;
        rb.isKinematic = true;
        SetColliderActivation(false);
        Collider2D victim = Physics2D.OverlapCircle(transform.position, grabRange, (1 << 3));
        GameObject.Find("AuraController").GetComponent<AuraController>().CallAuraBeParent(gameObject, grabRange);
        if (victim != null)
        {
            if (victim.gameObject.GetComponent<WalkerBase>().ControlledButAsleep() == true) return null;
            //Debug.Log("victim not null");
            Debug.Log("Get -> "+victim.gameObject.name);

            lineToVictim = GameObject.Find("LineManager").GetComponent<LineFactory>().GetLine(transform, victim.gameObject.transform);
            currentDirection = 0;
            AreWeColoredDead(true);
            rb.velocity = Vector2.zero;
            SetColliderActivation(true);
            return victim.gameObject;

        }
        SetColliderActivation(true);
        rb.isKinematic = false;

        return null;
    }

    public virtual void Jump()
    {
        if (isOnGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpForce);
        }
    }
    

    public virtual void SelfMovement()
    {
        if (!isOnGround && hasFreeWill) return;
        rb.velocity = new Vector2(WalkSpeed * currentDirection, rb.velocity.y);
    }

    

    private void Update()
    {
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
        if (hasFreeWill)
        {


            RaycastHit2D hit = Physics2D.Raycast(transform.position, shootVector, sightRayLength, (1 << 6));
            if (hit.collider != null)
            {
                currentDirection *= -1;
            }


            RaycastHit2D groundHit = Physics2D.Raycast(transform.position, (Vector2.down + shootVector), (sightRayLength * nextBlockRayLengthMultiplier), (1 << 6));
            if (groundHit.collider == null)
            {
                currentDirection *= -1;
                Debug.Log("no more next tile!");
            }
            else Debug.Log(groundHit.collider.gameObject.tag);
            
        }

        Debug.DrawRay(transform.position, Vector2.down * groundCheckRayLength, Color.red);
        RaycastHit2D downGround = Physics2D.Raycast(transform.position, Vector2.down, groundCheckRayLength, (1 << 6));
        if (downGround.collider == null)
        {
            isOnGround = false;
        }
        else isOnGround = true;

        if (lineToVictim != null)
        {
            distanceToVictim = lineToVictim.GetComponent<ThreePointLiner>().GetDistanceBetweenTargets();
            //Debug.Log("Distance to victim: " + distanceToVictim);
            if (distanceToVictim > DistanceLimit)
            {
                GameObject.Find("PlayerConsole").GetComponent<ConsoleControl>().ReleaseLatestVictim();
            }
        }
    }

    private void FixedUpdate()
    {
        SelfMovement();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasFreeWill && collision.gameObject.tag == "goal")
        {
            GameObject.Find("GameStateManager").GetComponent<GameStateManager>().GameWin();
            rb.velocity = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(hasFreeWill && collision.gameObject.layer == 3)
        {
            Debug.Log("Collided with walker");
            currentDirection *= -1;
        }
    }

}
