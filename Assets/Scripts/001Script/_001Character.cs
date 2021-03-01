using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

public class _001Character : MonoBehaviour
{
    public UnityEvent<Collider2D> DoorEventIn_OnWall = new UnityEvent<Collider2D>();
    public UnityEvent<Collider2D> DoorEventExit_OnWall = new UnityEvent<Collider2D>();


    public UnityEvent GenerateGroundEvent = new UnityEvent();

    public UnityEvent<Collider2D,GameObject> DoorEventOpenerIn = new UnityEvent<Collider2D, GameObject>();
    public UnityEvent<Collider2D,GameObject> DoorEventOpenerExit = new UnityEvent<Collider2D, GameObject>();
    public UnityEvent<Collider2D, GameObject> DoorEventLockerIn = new UnityEvent<Collider2D, GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.tag == "001GenerateKey")
        {
            GenerateGroundEvent.Invoke();
            collider2D.gameObject.SetActive(false);
        }

        if (collider2D.gameObject.tag == "001door_OnWall" || collider2D.gameObject.tag == "001door_OnWall_touched")
        {
            DoorEventIn_OnWall.Invoke(collider2D);
        }

        if (collider2D.gameObject.tag == "001doorOpener")
        {
            DoorEventOpenerIn.Invoke(collider2D, collider2D.transform.parent.Find("door").gameObject);
        }

        if(collider2D.gameObject.tag == "001doorLocker")
        {
            DoorEventLockerIn.Invoke(collider2D, collider2D.transform.parent.Find("door").gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.tag == "001door_OnWall" || collider2D.gameObject.tag == "001door_OnWall_touched")
        {
            DoorEventExit_OnWall.Invoke(collider2D);
        }
        if (collider2D.gameObject.tag == "001doorOpener")
        {
            DoorEventOpenerExit.Invoke(collider2D, collider2D.transform.parent.Find("door").gameObject);
        }
    }
}
