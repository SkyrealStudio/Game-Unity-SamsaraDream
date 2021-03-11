using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _001NormalDoorManager : MonoBehaviour
{
    // closed -> false
    public bool doorStatus = false;
    public GameObject doorNormal;
    public GameObject opener1;
    public GameObject opener2;

    void Start()
    {
        
    }

    void Update()
    {
        //doorStatus = doorNormal.GetComponent<BoxCollider2D>().isTrigger;
    }
}
