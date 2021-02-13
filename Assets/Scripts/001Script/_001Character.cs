using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class _001Character : MonoBehaviour
{
    public UnityEvent doorEvent;

    public _001Manager gm;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "TempTag")
        {
            Instantiate(gm.groundGroup, (gm.V3_groundGroupPre += new Vector3(gm.groundFloatAdd, 0f, 0f)), gm.groundGroup.transform.rotation);
        }
        if (collision.gameObject.tag == "001door")
        {
            doorEvent.Invoke();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "001door")
        {
            doorEvent.Invoke();
        }
    }
}
