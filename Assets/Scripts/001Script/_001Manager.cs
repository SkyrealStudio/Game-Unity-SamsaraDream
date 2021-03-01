using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Experimental.Rendering.Universal;

public class _001Manager : MonoBehaviour
{
    public Animator animator;
    public _001Character characterManager;

    //public _001BlackGroundText hdbzManager;
    public MsgBoxManager mbmNormal;//MsgBoxManager_Noramal(Top)
    public float mbmNormal_Time;
    public MsgBoxManager mbmBg;//MsgBoxManager_backGround
    public float mbmBg_Time;
    public enum ScriptState
    {
        OnWaiting,
        OnDoing
    }
    public enum OnDoingStatus
    {
        Normal_On,
        Normal_Hold,
        Bg_On,
        Bg_Hold
    }
    public enum LightStatus
    {
        Off,
        Off2On,
        On,
        On2Off
    }

    public LightStatus lightStatus;
    public ScriptState scriptState;
    public OnDoingStatus onDoingStatus = 0;
    public int doorPointer = 0;
    
    public Light2D l2dObj; 
    
    private float _l2d_outerRadiusSet;
    private float _l2d_IntensitySet;
    public float l2d_TimeSet;
    private float _l2d_timer = 0f;

    public CameraManager cm;
    public GameObject camHold;
    public GameObject mainCharacter;
    public Rigidbody2D mC_rig2D;
    public Collider2D mC_col2D;

    public Buttoner[] buttoners;

    public GameObject groundGroup;
    public float groundFloatAdd;

    public Vector3 V3_groundGroupPre = new Vector3 (0f,0f,-10f);
    public string[] thingsToSay = new string[]
    {

    };
    public int thingsToSayPointer = 0;

    public float speedX;
    private bool _isFirst = true;

    private bool _allowInteract = false;
    //public float settledValue_x;

    private Vector3 positionOrigion;
    private Collider2D _door_OnWallCollider2D;
    private Collider2D _doorOpenerCollider2D;
    private GameObject _preDoor;

    void Start()
    {
        SetmbmAlphaTimeSet(mbmBg,mbmBg_Time);
        SetmbmAlphaTimeSet(mbmNormal, mbmNormal_Time);

        _setControlmode(false);

        _l2d_outerRadiusSet = l2dObj.pointLightOuterRadius;
        _l2d_IntensitySet = l2dObj.intensity;

        l2dObj.pointLightOuterRadius = 0f;
        l2dObj.intensity = 0f;

        lightStatus = LightStatus.Off;

        onDoingStatus = 0;

        cm.SetFollowingGameObject(camHold);
        cm.StartFollowing();
        positionOrigion = mainCharacter.transform.position;
        _stopMoving();

        characterManager.GenerateGroundEvent.AddListener(_GenNewGround);

        characterManager.DoorEventIn_OnWall.AddListener(_MakeDoor_OnWall_In);
        characterManager.DoorEventExit_OnWall.AddListener(_MakeDoor_OnWall_Exit);
        buttoners[(int)inputManager.InputManager.EnumStatus.Interact].ClickEvent.AddListener(_DoorMethod_OnWall_OnClick);

        characterManager.DoorEventOpenerIn.AddListener(_MakeDoor_In);
        characterManager.DoorEventOpenerExit.AddListener(_MakeDoor_Exit);
        characterManager.DoorEventLockerIn.AddListener(_DoorMethod_OnLockTrigger);
        buttoners[(int)inputManager.InputManager.EnumStatus.Interact].ClickEvent.AddListener(_DoorMethodOpener_OnClick);

        //----The First Word
        _StartShowThing(thingsToSay[thingsToSayPointer], mbmBg);
    }

    private void SetmbmAlphaTimeSet( MsgBoxManager mbm , float mbm_Time)
    {
        mbm.textAlphaTime_SetFloat = mbm.boxAlphaTime_SetFloat = mbm_Time;
    }

    private void _GenNewGround()
    {
        GameObject tgo = Instantiate(groundGroup, (V3_groundGroupPre += new Vector3(groundFloatAdd, 0f, 0f)), groundGroup.transform.rotation, GameObject.Find("GameWorld").transform);
        tgo.transform.Find("ColliderGen").gameObject.SetActive(true);
    }

    //----OnWall Door
    private void _MakeDoor_OnWall_In(Collider2D collider2D)
    {
        _door_OnWallCollider2D = collider2D;
        
        _door_OnWallCollider2D.gameObject.GetComponent<SpriteRenderer>().material = new Material(Shader.Find("Custom/shader001"));
        _door_OnWallCollider2D.gameObject.GetComponent<SpriteRenderer>().material.SetFloat("_AspectRatio", 2.2f);//[Tip][20210214]这里目前是手动的
        _door_OnWallCollider2D.gameObject.GetComponent<SpriteRenderer>().material.SetFloat("_PixelPreUnit", 100);//[Tip][20210214]这里目前是手动的
        _door_OnWallCollider2D.gameObject.GetComponent<SpriteRenderer>().material.SetFloat("_lineWidth", 1);
        _door_OnWallCollider2D.gameObject.GetComponent<SpriteRenderer>().material.SetVector("_lineColor", new Vector4(1, 1, 0, 1));//yellow
        _door_OnWallCollider2D = collider2D;

        _allowInteract = true;
    }

    private void _MakeDoor_OnWall_Exit(Collider2D collider2D)
    {
        _door_OnWallCollider2D.gameObject.GetComponent<SpriteRenderer>().material = new Material(Shader.Find("Universal Render Pipeline/2D/Sprite-Lit-Default"));
        _door_OnWallCollider2D = null;
        _allowInteract = false;
    }

    private void _DoorMethod_OnWall_OnClick()
    {
        if (_door_OnWallCollider2D == null) return;
        else
        {
            //if (buttoners[(int)inputManager.InputManager.EnumStatus.Interact].pressed)
            //{
            if (_door_OnWallCollider2D.gameObject.tag == "001door_OnWall")
            {
                _door_OnWallCollider2D.GetComponent<_001InWallDoorAnimator>().CloseMe();
                _StartShowThing(thingsToSay[++thingsToSayPointer], mbmBg);
            }
            else if (_door_OnWallCollider2D.gameObject.tag == "001door_OnWall_touched")
            {
                _StartShowThing("锁上了^~", mbmNormal);
            }
        }
    }

    //----Normal Door
    private void _MakeDoor_In(Collider2D collider2D, GameObject goDoor)
    {
        _doorOpenerCollider2D = collider2D;
        _preDoor = goDoor;

        goDoor.GetComponent<SpriteRenderer>().material = new Material(Shader.Find("Custom/shader001"));
        goDoor.GetComponent<SpriteRenderer>().material.SetFloat("_AspectRatio", 0.36f);//[Tip][20210214]这里目前是手动的
        goDoor.GetComponent<SpriteRenderer>().material.SetFloat("_PixelPreUnit", 32);//[Tip][20210214]这里目前是手动的
        goDoor.GetComponent<SpriteRenderer>().material.SetFloat("_lineWidth", 1);
        goDoor.GetComponent<SpriteRenderer>().material.SetVector("_lineColor", new Vector4(1, 1, 0, 1));//yellow
        _allowInteract = true;
    }

    private void _MakeDoor_Exit(Collider2D collider2D, GameObject goDoor)
    {
        _preDoor = null;
        goDoor.GetComponent<SpriteRenderer>().material = new Material(Shader.Find("Universal Render Pipeline/2D/Sprite-Lit-Default"));
        _allowInteract = false;
    }
    
    private void _DoorMethodOpener_OnClick()
    {
        if (_preDoor == null) return;
        if(_preDoor.transform.position.x - characterManager.gameObject.transform.position.x > 0)
        {
            //开门动画
            _preDoor.GetComponent<_001NormalDoorAnimator>().OpenMe();
            _StartShowThing(thingsToSay[++thingsToSayPointer], mbmBg);
            _allowInteract = false;
        }
        else
        {
            _StartShowThing("锁上了^~", mbmNormal);
        }
    }
    
    private void _DoorMethod_OnLockTrigger(Collider2D collider2D,GameObject goDoor)
    {
        //关门动画
        goDoor.GetComponent<_001NormalDoorAnimator>().CloseMe();
        //goDoor.GetComponent<Collider2D>().isTrigger = false;
        _preDoor = goDoor;
    }
    
    /// <summary>
    /// 用来设定丢出指定的 什么文本 ... 在 什么文本框 ... 中. 
    /// 提示:会更改onDoingStatus = xxx_On
    /// </summary>
    /// <param name="thingStr"></param>
    /// <param name="mbmM"></param>
    private void _StartShowThing(string thingStr,MsgBoxManager mbmM)
    {
        scriptState = ScriptState.OnDoing;

        if (mbmM == mbmNormal)
        {
            onDoingStatus = OnDoingStatus.Normal_On;
        }
        else if(mbmBg)
        {
            onDoingStatus = OnDoingStatus.Bg_On;
        }
        else
        {
            //add more box here
        }
        
        mbmM.RunWith(MsgBoxManager.MsgBoxStatus.Running, thingStr);
    }

    private void _stopMoving()
    {
        mC_rig2D.velocity = new Vector3(0f, mC_rig2D.velocity.y);
    }

    private void _keeptMoving(bool mode)
    {
        if (mode)
        {
            mC_rig2D.velocity = new Vector3(speedX, mC_rig2D.velocity.y);
            return;
        }
        mC_rig2D.velocity = new Vector3(-speedX, mC_rig2D.velocity.y);
    }

    /// <summary>
    /// 用这个来设定玩家的按键显示或否
    /// true 显示, false不显示
    /// </summary>
    /// <param name="mode"></param>
    private void _setControlmode(bool mode)
    {
        foreach (Buttoner buttoner in buttoners)
        {
            if (buttoner != null)
            {
                if (buttoner.gameObject.name == "ButtonI" && mode)
                {
                    buttoner.gameObject.SetActive(_allowInteract);
                    buttoner.gameObject.GetComponent<Button>().interactable = (_allowInteract);
                    continue;
                }
                buttoner.gameObject.SetActive(mode);
                buttoner.gameObject.GetComponent<Button>().interactable = mode;
            }
        }
    }

    // Update is called once per frame

    public void Update()
    {
        //----
        if (!buttoners[(int)inputManager.InputManager.EnumStatus.Left].pressed && buttoners[(int)inputManager.InputManager.EnumStatus.Right].pressed)
        {
            _keeptMoving(true);
        }
        else
        {
            _stopMoving();
        }
        //----

        //----movement----
        if (buttoners[(int)inputManager.InputManager.EnumStatus.Left].pressed && !buttoners[(int)inputManager.InputManager.EnumStatus.Right].pressed)
        {
            animator.SetInteger("State", 0);
            _keeptMoving(false);//left
        }
        else if (!buttoners[(int)inputManager.InputManager.EnumStatus.Left].pressed && buttoners[(int)inputManager.InputManager.EnumStatus.Right].pressed)
        {
            animator.SetInteger("State", 1);
            _keeptMoving(true);//right
        }
        else
        {
            animator.StopPlayback();
            animator.SetInteger("State", -1);
            _stopMoving();
        }
        //----End of movement----

        switch (lightStatus)
        {
            case LightStatus.Off:
                break;
            case LightStatus.Off2On:
                _l2d_timer += Time.deltaTime;
                if (_l2d_timer > l2d_TimeSet)
                {
                    _l2d_timer = 0f;
                    lightStatus = LightStatus.On;
                }
                else
                {
                    l2dObj.intensity = (_l2d_timer / l2d_TimeSet) * _l2d_IntensitySet;
                    l2dObj.pointLightOuterRadius = (_l2d_timer / l2d_TimeSet) * _l2d_outerRadiusSet;
                }
                break;
            case LightStatus.On:
                break;
            case LightStatus.On2Off:
                _l2d_timer += Time.deltaTime;
                if (_l2d_timer > l2d_TimeSet)
                {
                    _l2d_timer = 0f;
                    lightStatus = LightStatus.Off;
                }
                else
                {
                    l2dObj.intensity = ((l2d_TimeSet - _l2d_timer) / l2d_TimeSet) * _l2d_IntensitySet;
                    l2dObj.pointLightOuterRadius = ((l2d_TimeSet - _l2d_timer) / l2d_TimeSet) * _l2d_outerRadiusSet;
                }
                break;
            default:
                break;
        }

        switch (scriptState)
        {
            case ScriptState.OnWaiting:
                if(!_isFirst)
                    _setControlmode(true);
                break;
            case ScriptState.OnDoing:
                _setControlmode(false);
                switch (onDoingStatus)
                {
                    case OnDoingStatus.Normal_On://normal
                        if(mbmNormal.StableFlag == true)
                            onDoingStatus = OnDoingStatus.Normal_Hold;
                        break;
                    case OnDoingStatus.Normal_Hold:
                        //等待mbmNormal加载完毕后
                        if (mbmNormal.Status == MsgBoxManager.MsgBoxStatus.Hidding && mbmNormal.StableFlag)
                        {
                            if (_isFirst)
                            {
                                lightStatus = LightStatus.Off2On;
                                _setControlmode(true);
                                _isFirst = false;
                                scriptState = ScriptState.OnWaiting;
                                break;
                            }
                            scriptState = ScriptState.OnWaiting;
                        }
                        break;

                    case OnDoingStatus.Bg_On://background

                        if (mbmBg.StableFlag == true)
                            onDoingStatus = OnDoingStatus.Bg_Hold;
                        break;
                    case OnDoingStatus.Bg_Hold:
                        //等待mbmNormal加载完毕后
                        if (mbmBg.Status == MsgBoxManager.MsgBoxStatus.Hidding && mbmBg.StableFlag)
                        {
                            if (_isFirst)
                            {
                                thingsToSayPointer++;
                                _StartShowThing("身边一片漆黑。^", mbmNormal);
                                break;
                            }
                            scriptState = ScriptState.OnWaiting;
                        }
                        break;
                }
                break;
            default:
                break;
        }
    }
}
