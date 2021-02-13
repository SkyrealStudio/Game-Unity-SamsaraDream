using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using enumNameSpace;
using inputManager;

public class GameManager : MonoBehaviour
{
    public InputManager imputManager;
    public CameraManager cameraManager;
    //Interact modules Begins
    public Bed_1 bed_1_InteractionManager;
    public Coin coin_InteractionManager;
    //Interact modules End
    public MsgBoxManager msgboxManager;

    private Vector3 respawnPosition;
    
    public Vector3 RespawnPosition { set => respawnPosition = value; }

    public Canvas g_canvas;

    public bool g_allowUI = true;
    public GameObject UIContainer;
    
    public Buttoner[] buttoners;
    public GameObject mainCharacter; 
    private Rigidbody2D mainCharacter_Rigidbody2D;
    public Collider2D nowGrounding;

    public float score = 0;
    public Text textline;

    public Collider2D nowInteract_possibly;
    public bool isLaddering;
    private bool laddermode = false;
    public int g_jumpEnabledCount = 2;
    public int g_jumpEnabledLimit = 2;

    //public float g_touchGroundsjumpForce;
    //public float tGjF_init;

    public float g_accelerationX_origin = 1.5f;
    
    //private float tGjF_count; //TouchGroundLittleJump_Count
    
    public float g_VelocityJump = 8f;
    //private bool m_jumpBumper = true;

    public float g_restrictVelocityXAbs;
    public float g_restrictVelocityYAbs;

    public bool[] g_movementJurisdiction = new bool[4] { true, true, true, true};
    /*0=left , 1=right , 2=up , 3=down
     *true ==  
     */
    public bool g_grounded = false;
    
    public float drag_set = 0.7f;
    private bool addDragflag = false;//false == addable ; true == added
    public float drag_decline_set = 1.6f; //second
    private  float drag_timeCounter = 0f;
    
    public float ladderSpeed = 3f;

    //lock == true ; unlock == false

    private bool _pauseFlag = false;

    void Start()
    {
        coin_InteractionManager.gameObject.SetActive(true);
        mainCharacter_Rigidbody2D = mainCharacter.GetComponent<Rigidbody2D>();
        UIContainer.SetActive(g_allowUI);
        GameStart();
    }

    public void GamePause() //co-op with MsgBoxManager
    {
        if(!_pauseFlag)
        {
            _pauseFlag = true;
            //code
            print("Pause!");
        }
        else
        {
            //do nothing
        }
    }

    public void GameResume() //co-op with MsgBoxManager
    {
        if (_pauseFlag)
        {
            _pauseFlag = false;
            //code
            print("Resume!");
        }
        else
        {
            //do nothing
        }
    }

    public void GameOver()
    {
        print("gameOver \\(0ω0)/");
        mainCharacter.SetActive(false);
        score = 0;
        coin_InteractionManager.Reset();
        bed_1_InteractionManager.Reset();
        mainCharacter_Rigidbody2D.velocity = new Vector2(0f, 0f);
        mainCharacter.transform.position = respawnPosition; // ???
        mainCharacter.SetActive(true);
    }

    public void CharacterDie()
    {
        mainCharacter_Rigidbody2D.velocity = new Vector2(0f, 0f);
        mainCharacter.transform.position = respawnPosition;
    }

    public void GameStart()
    {
        print("gameStart _(:3 )∠_");
        mainCharacter.gameObject.transform.position = new Vector2(0, 0);
        score = 0f;
        g_jumpEnabledCount = g_jumpEnabledLimit;
        mainCharacter.SetActive(true);
        cameraManager.StartFollowing(mainCharacter);
        cameraManager.StartFollowing();
    }

    public bool AddJump()
    {
        if ( g_jumpEnabledCount < g_jumpEnabledLimit)
        {
            g_jumpEnabledCount++;
            return true;
        }
        return false;
    }

    public float AddScore(float n)
    {
        score += n;
        return score;
    }

    private void Print_Text(string tstr)
    {
        textline.text = tstr;
    }

    public void MainCharacterJump()
    {
        if (g_jumpEnabledCount > 0)
        {
            mainCharacter_Rigidbody2D.velocity = new Vector2(mainCharacter_Rigidbody2D.velocity.x, g_VelocityJump);
            g_jumpEnabledCount--;
        }
    }

    public void De_nowInteract_possibly()
    {
        nowInteract_possibly.transform.parent.gameObject.GetComponent<Bed_1>().SetHighlight(int.Parse(nowInteract_possibly.gameObject.name), false);
        nowInteract_possibly = null;
    }

    private void Update()
    {
        //----PlayGround----
        if(Input.GetKeyDown(KeyCode.Insert))
        {
            GameOver();
        }
        
        //----Text----
        if (score>=8)
        {
            Print_Text("Score:" + (int)score + "You Win!!");
        }
        else
        {
            Print_Text("Score:" + (int)score);
        }
        
        //----movement----
        if (imputManager.status[(int)EnumStatus.Left] && (g_grounded || g_movementJurisdiction[0]) == true)
        {
            laddermode = false;
            addDragflag = false;
            if ((mainCharacter_Rigidbody2D.velocity + new Vector2(-g_accelerationX_origin, 0)).x <= -g_restrictVelocityXAbs)
                mainCharacter_Rigidbody2D.velocity += new Vector2(-g_restrictVelocityXAbs - mainCharacter_Rigidbody2D.velocity.x, 0);
            else
                mainCharacter_Rigidbody2D.velocity += new Vector2(-g_accelerationX_origin, 0);
        }
        else if(imputManager.status[(int)EnumStatus.Right] && (g_grounded || g_movementJurisdiction[1]) == true)
        {
            laddermode = false;
            addDragflag = false;
            if ((mainCharacter_Rigidbody2D.velocity + new Vector2(g_accelerationX_origin, 0)).x >= g_restrictVelocityXAbs)
                mainCharacter_Rigidbody2D.velocity += new Vector2(g_restrictVelocityXAbs - mainCharacter_Rigidbody2D.velocity.x, 0);
            else
                mainCharacter_Rigidbody2D.velocity += new Vector2(g_accelerationX_origin, 0);
        }
        else
        {
            addDragflag = true;
        }

        if (imputManager.status[(int)EnumStatus.Jump])
        {
            MainCharacterJump();
            imputManager.status[(int)EnumStatus.Jump] = false;
        }

        if (isLaddering == true) //Laddering_Collided
        {
            mainCharacter_Rigidbody2D.gravityScale = 0f;
            //[Tip][20210125]应接上动画切换...
            if (imputManager.status[(int)EnumStatus.Up])
            {
                laddermode = true;
                mainCharacter_Rigidbody2D.velocity = new Vector2(0f, +ladderSpeed);
            }
            else if (imputManager.status[(int)EnumStatus.Down])
            {
                laddermode = true;
                mainCharacter_Rigidbody2D.velocity = new Vector2(0f, -ladderSpeed);
            }

            else
            {
                if(laddermode)
                {
                    if (imputManager.status[(int)EnumStatus.Left])
                    {
                        laddermode = true;
                        mainCharacter_Rigidbody2D.velocity = new Vector2(-ladderSpeed, mainCharacter_Rigidbody2D.velocity.y);
                    }
                    else if (imputManager.status[(int)EnumStatus.Right])
                    {
                        laddermode = true;
                        mainCharacter_Rigidbody2D.velocity = new Vector2(ladderSpeed, mainCharacter_Rigidbody2D.velocity.y);
                    }

                    mainCharacter_Rigidbody2D.velocity = new Vector2(0f, 0f);
                }
                else
                {
                    mainCharacter_Rigidbody2D.gravityScale = 1f;
                }
            }
        }
        else
        {
            mainCharacter_Rigidbody2D.gravityScale = 1f;
        }
        //----endof movement----
        
        //----interact----
        if(nowInteract_possibly == true)
        {
            if (imputManager.status[(int)EnumStatus.Interact] )
            {
                nowInteract_possibly.transform.parent.gameObject.GetComponent<Bed_1>().Interact(int.Parse(nowInteract_possibly.gameObject.name));
            }
        }
        else
        {

        }
        
        drag_timeCounter = (addDragflag) ?  (drag_timeCounter + Time.deltaTime): 0f ;
        mainCharacter_Rigidbody2D.drag = (drag_set * (1f-(drag_timeCounter / drag_decline_set))<=0f) ? 0f : drag_set * (1f - (drag_timeCounter / drag_decline_set));


        //----UI----
        buttoners[(int)EnumStatus.Interact].gameObject.GetComponent<Button>().interactable = nowInteract_possibly ? true : false;
        buttoners[(int)EnumStatus.Up].gameObject.GetComponent<Button>().interactable =buttoners[(int)EnumStatus.Down].gameObject.GetComponent<Button>().interactable = isLaddering ? true : false;
    }
}
