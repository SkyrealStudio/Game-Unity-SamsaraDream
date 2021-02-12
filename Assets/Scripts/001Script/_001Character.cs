using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class _001Character : MonoBehaviour
{
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
            //EditorApplication.isPaused = true;
            GameObject t = Instantiate(gm.groundGroup, (gm.V3_groundGroupPre += new Vector3(gm.groundFloatAdd, 0f, 0f)), gm.groundGroup.transform.rotation);
            //collision.gameObject.SetActive(false);
        }
    }

}
