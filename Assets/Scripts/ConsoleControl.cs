using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ConsoleControl : MonoBehaviour
{

    public Sprite ConsoleOn;
    public Sprite ConsoleOff;

    public GameObject Arrow;
    public GameObject Arrow2;
    public GameObject Arrow3;
    public GameObject Arrow4;

    public SpriteRenderer rArrow;
    public SpriteRenderer rArrow2;
    public SpriteRenderer rArrow3;
    public SpriteRenderer rArrow4;

    public Sprite cArrow;
    public Sprite cJump;
    public Sprite cPoss;
    public Sprite cNeutral;



    private float moveDirection;

    private GameObject linkedUp;
    private GameObject firstVictim;
    private WalkerBase linkedUpScript;
    private float grabRange = 5f;

    private SpriteRenderer sr;

    private GameObject MyLine = null;

    public CinemachineVirtualCamera myCam;

    private List<GameObject> PossessedList = new List<GameObject>();

    [SerializeField]
    private float distanceToFirstVictim = 0f;

    public bool canPlay = true;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        rArrow = Arrow.GetComponent<SpriteRenderer>();
        rArrow2 = Arrow2.GetComponent<SpriteRenderer>();
        rArrow3 = Arrow3.GetComponent<SpriteRenderer>();
        rArrow4 = Arrow4.GetComponent<SpriteRenderer>();
    }

    private void changeArrowFace(string phase)
    {
        Arrow.transform.rotation = Quaternion.Euler(Vector3.zero);
        Arrow2.transform.rotation = Quaternion.Euler(Vector3.zero);
        Arrow3.transform.rotation = Quaternion.Euler(Vector3.zero);
        Arrow4.transform.rotation = Quaternion.Euler(Vector3.zero);

        switch (phase)
        {
            case "arrow":
                rArrow.sprite = cArrow;
                rArrow2.sprite = cArrow;
                rArrow3.sprite = cArrow;
                rArrow4.sprite = cArrow;
                break;
            case "jump":
                rArrow.sprite = cJump;
                rArrow2.sprite = cJump;
                rArrow3.sprite = cJump;
                rArrow4.sprite = cJump;
                break;
            case "neutral":
                rArrow.sprite = cNeutral;
                rArrow2.sprite = cNeutral;
                rArrow3.sprite = cNeutral;
                rArrow4.sprite = cNeutral;
                break;
            case "possess":
                rArrow.sprite = cPoss;
                rArrow2.sprite = cPoss;
                rArrow3.sprite = cPoss;
                rArrow4.sprite = cPoss;
                break;
        }
    }

    private void ArrowRotator(float direction)
    {
        var rotator = new Vector3(0f,0f,0f);
        changeArrowFace("arrow");
        if(direction > 0)
        {
            rotator.z = -90f;
            

        }
        else if(direction < 0)
        {
            rotator.z = 90f;
            
        }
        else
        {
            rotator.z = 0f;
            changeArrowFace("neutral");
        }

        Arrow.transform.rotation = Quaternion.Euler(rotator.x, rotator.y, rotator.z);
        Arrow2.transform.rotation = Quaternion.Euler(rotator.x, rotator.y, rotator.z);
        Arrow3.transform.rotation = Quaternion.Euler(rotator.x, rotator.y, rotator.z);
        Arrow4.transform.rotation = Quaternion.Euler(rotator.x, rotator.y, rotator.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (!canPlay) return;
        //Debug.DrawLine(transform.position, transform.position + (Vector3.right*(grabRange)), Color.red);
        
        moveDirection = Input.GetAxisRaw("Horizontal");

        if(moveDirection==1||moveDirection==-1) ArrowRotator(moveDirection);



        if (linkedUp != null)
        {
            linkedUpScript.ControllerOrders(moveDirection);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("Possess Aura go!");
            changeArrowFace("possess");
            if (linkedUp == null)
            {
                //Physics2D.autoSyncTransforms = true;

                Collider2D victim = Physics2D.OverlapCircle(transform.position, grabRange, (1 << 3));
                //GameObject.Find("AuraController").GetComponent<AuraController>().CallAura(transform.position, grabRange);
                GameObject.Find("AuraController").GetComponent<AuraController>().CallAuraBeParent(gameObject, grabRange);
                if (victim != null)
                {
                    //Debug.Log("victim not null");
                    //Debug.Log(victims.Length);
                    //Debug.Log(victim.gameObject.tag);
                    PossessedList.Add(victim.gameObject);
                    Debug.Log("Possesed list count: "+PossessedList.Count);
                    linkedUp = victim.gameObject;


                    //now lets possess him
                    linkedUpScript = linkedUp.GetComponent<WalkerBase>();
                    linkedUpScript.GetControlled();
                    linkedUpScript.BaseAction();

                    //sr.sprite = ConsoleOn;

                    MyLine = GameObject.Find("LineManager").GetComponent<LineFactory>().GetLine(transform, linkedUp.transform);
                    myCam.Follow = linkedUp.transform;
                }
                //Physics2D.autoSyncTransforms = false;
            }
            else
            {
                //we use the linked up aura power
                var newVictim = linkedUpScript.PossessAuraEmit();
                if(newVictim!=null)
                {
                    //new possess
                    
                    linkedUp = newVictim;
                    PossessedList.Add(newVictim);
                    linkedUpScript = linkedUp.GetComponent<WalkerBase>();
                    linkedUpScript.GetControlled();
                    myCam.Follow = linkedUp.transform;
                }
            }

        }

        if (Input.GetButtonDown("Jump"))
        {
            if (linkedUp != null)
            {
                changeArrowFace("jump");
                linkedUpScript.Jump();
            }
        }

        if (Input.GetButtonDown("Fire2"))
        {
            Debug.Log("take it back");
            if (MyLine == null) return; //this checks if we have at last got one

            ReleaseLatestVictim();

            


        }

        
        if (MyLine != null)
        {
            distanceToFirstVictim = MyLine.GetComponent<ThreePointLiner>().GetDistanceBetweenTargets();
            //Debug.Log("Distance to victim: " + distanceToVictim);
            if (distanceToFirstVictim > 20)
            {
                ReleaseLatestVictim();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            GameObject.Find("GameStateManager").GetComponent<GameStateManager>().ResetStage();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            GameObject.Find("GameStateManager").GetComponent<GameStateManager>().ResetToTitle();
        }



    }

    public void ReleaseLatestVictim()
    {
        GameObject oldVictim;
        linkedUpScript.ReleaseControl();
        PossessedList.Remove(linkedUp);
        Debug.Log("Possesed list count: " + PossessedList.Count);

        if (PossessedList.Count > 0)
        {
            oldVictim = PossessedList[PossessedList.Count - 1];
            linkedUp = oldVictim;
            linkedUpScript = linkedUp.GetComponent<WalkerBase>();
            linkedUpScript.RenewControl();
            myCam.Follow = linkedUp.transform;
        }
        else
        {
            linkedUp = null;
            //theres nothing left but the console
            Destroy(MyLine);
            myCam.Follow = transform;
        }
    }

    private void FixedUpdate()
    {
        
        
    }
}
