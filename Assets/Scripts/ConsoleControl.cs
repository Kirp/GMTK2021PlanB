using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleControl : MonoBehaviour
{

    public Sprite ConsoleOn;
    public Sprite ConsoleOff;

    public GameObject Arrow;

    private float moveDirection;

    private GameObject linkedUp;
    private WalkerBase linkedUpScript;
    private float grabRange = 2f;

    private SpriteRenderer sr;

    private GameObject MyLine;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void ArrowRotator(float direction)
    {
        var rotator = new Vector3(0f,0f,0f);
        if(direction > 0)
        {
            rotator.z = -90f;
            Arrow.SetActive(true);

        }
        else if(direction < 0)
        {
            rotator.z = 90f;
            Arrow.SetActive(true);
        }
        else
        {
            Arrow.SetActive(false);
        }

        Arrow.transform.rotation = Quaternion.Euler(rotator.x, rotator.y, rotator.z);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(transform.position, transform.position + (Vector3.right*(grabRange)), Color.red);
        moveDirection = Input.GetAxisRaw("Horizontal");

        ArrowRotator(moveDirection);



        if (linkedUp != null)
        {
            linkedUpScript.ControllerOrders(moveDirection);
        }


    }

    private void FixedUpdate()
    {
        
        if (Input.GetKey(KeyCode.Z))
        {
            Debug.Log("Z!");

            if (linkedUp == null)
            {
                //Physics2D.autoSyncTransforms = true;
                
                Collider2D victim = Physics2D.OverlapCircle(transform.position, grabRange, (1 << 3));
                if (victim != null)
                {
                    Debug.Log("victim not null");
                    //Debug.Log(victims.Length);
                    Debug.Log(victim.gameObject.tag);
                    linkedUp = victim.gameObject;

                    //now lets possess him
                    linkedUpScript = linkedUp.GetComponent<WalkerBase>();
                    linkedUpScript.GetControlled();
                    linkedUpScript.BaseAction();

                    sr.sprite = ConsoleOn;

                    GameObject.Find("LineManager").GetComponent<LineFactory>().GetLine(transform, linkedUp.transform);
                }
                //Physics2D.autoSyncTransforms = false;

                
                
                

            }







        }
    }
}
