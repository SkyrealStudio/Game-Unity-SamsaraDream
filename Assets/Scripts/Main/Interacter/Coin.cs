using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public GameManager gm;  //[Tip][20201225]需要手动挂载...

    public GameObject coin_Prefab;
    private GameObject[] coins;
    private GameObject[] tipers;
    public Vector3[] positions;
    //设置Coin生成细节...
    //[Tip][20201229]这迟早给我塞进文件里 -- 地图编辑器
    //[Tip][20210124]再次吐槽: 目前正在设置重载的细节 -- 而这个手动设置的状态没给我少添麻烦

    private int childrenCount;
    private float[] destroyingRemainings;
    private float[] originColorAlphaValues;
    private bool[] onDestroying;
    public float scoreEach;
    public float destroyRemainTime_Set; //Seconds
    
    void Start()
    {
        childrenCount = positions.Length;

        coins = new GameObject[childrenCount];
        tipers= new GameObject[childrenCount];

        onDestroying = new bool[childrenCount];
        destroyingRemainings = new float[childrenCount];
        originColorAlphaValues = new float[childrenCount];
        
        for(int i=0;i<childrenCount;i++)
        {
            onDestroying[i] = false;
            coins[i] = Instantiate(coin_Prefab, positions[i], new Quaternion() ,gameObject.transform);
            coins[i].name = i.ToString(); //名字为碰撞作检索
            tipers[i] = coins[i].transform.GetChild(0).gameObject;
            tipers[i].SetActive(false);
        }
    }

    public bool DeleteSelf(int i)
    {
        if (onDestroying[i])
            return false;
        else
        {
            gm.AddScore(scoreEach);
            destroyingRemainings[i] = 0f;
            tipers[i].SetActive(true);
            onDestroying[i] = true;
            originColorAlphaValues[i] = tipers[i].GetComponent<SpriteRenderer>().color.a;
            coins[i].GetComponent<SpriteRenderer>().color = new Color(0f,0f,0f,0f);
            return true;
        }
    }
    
    void Update()
    {
        for (int i = 0; i < childrenCount; i++)
        {
            if (onDestroying[i])
            {
                destroyingRemainings[i] += Time.deltaTime;
                float step = (Time.deltaTime / destroyRemainTime_Set) * originColorAlphaValues[i];
                if (destroyingRemainings[i] >= destroyRemainTime_Set)
                {
                    Destroy(tipers[i]);
                    Destroy(coins[i]);
                    continue;
                }                
                tipers[i].GetComponent<SpriteRenderer>().color -= new Color(0f,0f,0f,step);//变得透明点?
                tipers[i].transform.position += new Vector3(0f, 0.01f, 0f);
            }
        }
    }

    public void Reset()
    {
        foreach(Transform transform in gameObject.GetComponentInChildren<Transform>())
        {
            Destroy(transform.gameObject);
        }
        Start();
    }
}
