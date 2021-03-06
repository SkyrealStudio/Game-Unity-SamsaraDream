﻿//MainCharacter.cs
//

using UnityEngine;
using System.Collections.Generic;

public class MainCharacter : MonoBehaviour
{
    /// <summary>
    /// Attached game manager.
    /// </summary>
    public GameManager gm;

    private Stack<Collider2D> queLadderCollider2D = new Stack<Collider2D>();

    private void Start()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if ( collision.transform.gameObject.tag == "Ground" )
        {
            gm.g_grounded = true;
            if (gm.AddJump() == true)
                gm.AddJump();
            gm.nowGrounding = collision.gameObject.GetComponent<Collider2D>();
            //gm.TouchGroundLittleJump();
            // two times if touched ground
            // and a bit fixing
        }

        if (collision.transform.gameObject.tag == "Cliff_Side_Left")
        {
            gm.g_movementJurisdiction[1] = false;// right denied
        }

        if (collision.transform.gameObject.tag == "Cliff_Side_Right")
        {
            gm.g_movementJurisdiction[0] = false;// left denied
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.transform.gameObject.tag == "Interactive_Coin")
        {
            collision.gameObject.transform.GetComponentInParent<Coin>().DeleteSelf(int.Parse(collision.gameObject.name));
        }

        if (collision.transform.gameObject.tag == "Interactive_Bed")
        {
            gm.nowInteract_possibly = collision;
            //仅触发高亮, 取消高亮在gm.De_nowInteract_possibly()方法中
            collision.transform.parent.gameObject.GetComponent<Bed_1>().SetHighlight(int.Parse(collision.gameObject.name), true);
        }

        if (collision.transform.gameObject.tag == "Ladder")
        {
            queLadderCollider2D.Push(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.gameObject.tag == "Interactive_Bed")
        {
            gm.De_nowInteract_possibly();//[Tip][20210205]注意! 按照这里的整套逻辑, 只能同时Interact一个可InterAct的对象
            
            //gm.nowInteract_possibly = null; //[Tip][20201225]这里逻辑可能有问题
            //gm.SwitchKeyMode(true);
        }

        if (collision.transform.gameObject.tag == "Ladder")
        {
            if(queLadderCollider2D.Count!=0)//
                queLadderCollider2D.Pop();//[Tip][20200125]只需知晓数量, 因此随意弹出
        }

        if (collision.transform.gameObject.tag == "Zone_GameZone")
        {
            gm.CharacterDie();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.gameObject.tag == "Ground")
        {
            gm.g_grounded = false;
        }

        if (collision.transform.gameObject.tag == "Cliff_Side_Left")
        {
            gm.g_movementJurisdiction[1] = true; //right allowed
        }

        if (collision.transform.gameObject.tag == "Cliff_Side_Right")
        {
            gm.g_movementJurisdiction[0] = true; //left allowed
        }
    }

    private void Update()
    {

    }
}