using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using shaderStorage;

public class Bed_1 : MonoBehaviour
{
    public GameManager gm;
    public ShaderStorage shstor;
    public GameObject bed1_Prefab;
    private GameObject[] bed_1s; // child "bed_1"Array;
    private GameObject[] tipers;
    public Vector3[] positions;

    private int childrenCount;
    public float destroyRemainTime_Set = 0.5f;
    
    private float[] destroyingRemainings;
    private float[] originColorAlphaValues;

    private bool[] status = new bool[2] { false, false };
    private int[] interaction = new int[2]; // [0] = pre [1] = now
                                         //[Tip][20210124]只需要2个...这样设置主要...是我不相信玩家可以从一个存档点到另外一个存档点会短于几秒钟的时间
    
    
    void Start()
    {
        childrenCount = positions.Length;
        
        bed_1s = new GameObject[childrenCount];
        tipers = new GameObject[childrenCount];

        interaction[0] = interaction[1] = -1;
        
        destroyingRemainings = new float[childrenCount];
        originColorAlphaValues = new float[childrenCount];

        for (int i = 0; i < childrenCount; i++)
        {
            status[i] = false;
            bed_1s[i] = Instantiate(bed1_Prefab, positions[i], new Quaternion(), gameObject.transform);
            bed_1s[i].name = i.ToString(); //名字为碰撞作检索
            bed_1s[i].transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            tipers[i] = bed_1s[i].transform.GetChild(0).gameObject;
        }
        Interact(0);//设置初始重生点
        bed_1s[0].transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 1);
    }

    public bool Interact(int i)
    {
        if (i == interaction[1])
            return false; //已登记
        else
        {
            interaction[0] = interaction[1];
            interaction[1] = i;

            destroyingRemainings[interaction[1]] = 0f;
            if(interaction[0] != -1)
                destroyingRemainings[interaction[0]] = 0f;
            
            if(interaction[0]!=-1)
                originColorAlphaValues[interaction[0]] = tipers[interaction[0]].GetComponent<SpriteRenderer>().color.a;

            
            gm.RespawnPosition = bed_1s[i].transform.position + new Vector3(0f,0.5f,0f);
            return true;
        }
    }

    public void SetHighlight(int n,bool mode)
    {
        bed_1s[n].GetComponent<SpriteRenderer>().material = mode ? new Material(Shader.Find("Custom/shader001")) : new Material(Shader.Find("Universal Render Pipeline/2D/Sprite-Lit-Default"));
        if(mode)
        {
            bed_1s[n].GetComponent<SpriteRenderer>().material.SetVector("_lineColor", new Vector4(1, 1, 0, 1));//yellow
            bed_1s[n].GetComponent<SpriteRenderer>().material.SetFloat("_lineWidth", 1);//yellow
            bed_1s[n].GetComponent<SpriteRenderer>().material.SetFloat("_AspectRatio", 0.4545f);//[Tip][20210205]这里目前是手动的
            bed_1s[n].GetComponent<SpriteRenderer>().material.SetFloat("_PixelPreUnit", 20);//[Tip][20210205]这里目前是手动的
        }
    }
    
    void Update()
    {
        for (int i = 0; i < childrenCount; i++)
        {
            if (i == interaction[1])
            {
                if (destroyingRemainings[i] > destroyRemainTime_Set)
                {
                    continue;
                }
                destroyingRemainings[i] += Time.deltaTime;
                float step = (Time.deltaTime / destroyRemainTime_Set) * 1f;
                tipers[i].GetComponent<SpriteRenderer>().color += new Color(0f, 0f, 0f, step);//变得不透明
            }
            else if (i == interaction[0])
            {
                if (destroyingRemainings[i] > destroyRemainTime_Set)
                {
                    continue;
                }
                destroyingRemainings[i] += Time.deltaTime;
                float step = (Time.deltaTime / destroyRemainTime_Set) * originColorAlphaValues[i];
                tipers[i].GetComponent<SpriteRenderer>().color -= new Color(0f, 0f, 0f, step);//变得透明
            }
            else
            {
                //continue;
            }
        }
    }

    public void Reset()
    {
        foreach (Transform transform in gameObject.GetComponentInChildren<Transform>())
        {
            Destroy(transform.gameObject);
        }
        Start();
    }
}
