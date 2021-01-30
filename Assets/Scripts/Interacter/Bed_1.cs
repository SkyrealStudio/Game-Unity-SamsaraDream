﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed_1 : MonoBehaviour
{
    public GameManager gm;
    
    public GameObject bed1_Prefab;
    private GameObject[] bed_1s;
    private GameObject[] tipers;
    public Vector3[] positions;

    private int childrenCount;
    public float destroyRemainTime_Set = 0.5f;
    
    private float[] destroyingRemainings;
    private float[] originColorAlphaValues;

    private bool[] status = new bool[2] { false, false };
    private int[] interact = new int[2]; // [0] = pre [1] = now
                                         //[Tip][20210124]只需要2个...这样设置主要...是我不相信玩家可以从一个存档点到另外一个存档点会短于几秒钟的时间
    void Start()
    {
        childrenCount = positions.Length;
        
        bed_1s = new GameObject[childrenCount];
        tipers = new GameObject[childrenCount];

        interact[0] = interact[1] = -1;
        
        destroyingRemainings = new float[childrenCount];
        originColorAlphaValues = new float[childrenCount];

        for (int i = 0; i < childrenCount; i++)
        {
            status[i] = false;
            bed_1s[i] = Instantiate(bed1_Prefab, positions[i], new Quaternion(), gameObject.transform);
            bed_1s[i].name = i.ToString(); //名字为碰撞作检索
            tipers[i] = bed_1s[i].transform.GetChild(0).gameObject;
        }
        Interact(0);//设置初始重生点
    }

    public bool Interact(int i)
    {
        if (i == interact[1])
            return false; //已登记
        else
        {
            interact[0] = interact[1];
            interact[1] = i;

            destroyingRemainings[interact[1]] = 0f;
            if(interact[0] != -1)
                destroyingRemainings[interact[0]] = 0f;
            
            if(interact[0]!=-1)
                originColorAlphaValues[interact[0]] = tipers[interact[0]].GetComponent<SpriteRenderer>().color.a;

            
            gm.RespawnPosition = bed_1s[i].transform.position + new Vector3(0f,0.5f,0f);
            return true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for (int i = 0; i < childrenCount; i++)
        {
            if (i == interact[1])
            {
                destroyingRemainings[i] += Time.deltaTime;
                float step = (Time.deltaTime / destroyRemainTime_Set) * 1f;
                if (destroyingRemainings[i] >= destroyRemainTime_Set)
                {
                    continue;
                }
                tipers[i].GetComponent<SpriteRenderer>().color += new Color(0f, 0f, 0f, step);//变得不透明
            }
            else if (i == interact[0])
            {
                destroyingRemainings[i] += Time.deltaTime;
                float step = (Time.deltaTime / destroyRemainTime_Set) * originColorAlphaValues[i];
                if (destroyingRemainings[i] >= destroyRemainTime_Set)
                {
                    continue;
                }
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
