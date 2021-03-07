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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        doorStatus = doorNormal.GetComponent<BoxCollider2D>().isTrigger;
    }
}
