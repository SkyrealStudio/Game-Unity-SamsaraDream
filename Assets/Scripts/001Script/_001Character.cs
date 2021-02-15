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
        if(collider2D.gameObject.tag == "001doorOpener")
        {
            DoorEventOpenerIn.Invoke(collider2D, collider2D.gameObject.transform.GetChild(0).gameObject);
        }
        if (collider2D.gameObject.tag == "001door_OnWall")
        {
            DoorEventIn_OnWall.Invoke(collider2D);
        }
    }

    private void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.tag == "001door_OnWall")
        {
            DoorEventExit_OnWall.Invoke(collider2D);
        }
    }
}
