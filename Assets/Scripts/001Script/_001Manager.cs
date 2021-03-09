using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class _001Manager : MonoBehaviour
{
    public Animator mcAnimator;
    public _001Character characterManager;

    public _001UserInput _001UserInputManager;
    //public _001BlackGroundText hdbzManager;
    public MsgBoxManager mbmNormal;//MsgBoxManager_Noramal(Top)
    public float mbmNormal_Time;
    public MsgBoxManager mbmBg;//MsgBoxManager_backGround
    public float mbmBg_Time;
    public enum ScriptState
    {
        AtFirstTime,
        AllowPlaying,
        OnDoing
    }
    public enum OnDoingStatus_MSGManager
    {
        mbmNormal_On,
        mbmNormal_Hold,
        mbmBg_On,
        mbmBg_Hold
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
    public OnDoingStatus_MSGManager onDoingStatus = 0;
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
    private string _NextThingToSay()
    {
        if (_thingsToSayPointer == -1)
        {
            _thingsToSayPointer++;
            return thingsToSay[0];
        }
        else
        {
            //loop
            return thingsToSay[(++_thingsToSayPointer == thingsToSay.Length) ? (_thingsToSayPointer=0) : _thingsToSayPointer];
        }
    }

    private int _thingsToSayPointer = -1;

    public float speedX;
    private bool _isFirst = true;

    private bool _allowInteract = false;
    //public float settledValue_x;

    private Vector3 positionOrigion;
    private Collider2D _door_OnWallCollider2D;
    private Collider2D _doorOpenerCollider2D;
    private GameObject _preDoor;

    private bool _isMsgBoxing = false;

    private void _setMsgBoxing(bool b)
    {
        _isMsgBoxing = b;
    }

    void Start()
    {
        _001UserInputManager.alterTimeSetValue.AddListener(_001userInput_SATS);
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

        //when Interact, stop movement
        buttoners[(int)inputManager.InputManager.EnumStatus.Interact].ClickEvent.AddListener(_stopMoving);
        
        mbmNormal.exitEvent.AddListener(_setMsgBoxing);
        mbmNormal.exitEvent.AddListener(_setControlmode);

        mbmBg.exitEvent.AddListener(_setMsgBoxing);
        mbmBg.exitEvent.AddListener(_setControlmode);

        //----The First Word
        _stopMoving();
        _StartShowThing(_NextThingToSay(), mbmBg);
    }

    private void SetmbmAlphaTimeSet( MsgBoxManager mbm, float mbm_Time)
    {
        mbm.curtainTime_SetFloat = mbm.textAlphaTime_SetFloat = mbm.boxAlphaTime_SetFloat = mbm_Time;
    }

    private void _GenNewGround()
    {
        GameObject tgo = Instantiate(groundGroup, (V3_groundGroupPre += new Vector3(groundFloatAdd, 0f, 0f)), groundGroup.transform.rotation, GameObject.Find("GameWorld").transform);

        tgo.transform.Find("ColliderGen").gameObject.SetActive(true);
        tgo.transform.Find("DoorGroup").gameObject.transform.Find("ColliderOpener1").gameObject.SetActive(true);
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
            _setMsgBoxing(true);
            //if (buttoners[(int)inputManager.InputManager.EnumStatus.Interact].pressed)
            //{
            if (_door_OnWallCollider2D.gameObject.tag == "001door_OnWall")
            {
                _door_OnWallCollider2D.GetComponent<_001InWallDoorAnimator>().CloseMe();
                _StartShowThing(_NextThingToSay(), mbmBg);
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
        _setMsgBoxing(true);
        if (_preDoor == null) return;

        if(_preDoor.transform.position.x - characterManager.gameObject.transform.position.x > 0)
        {

            //开门动画
            _preDoor.GetComponent<_001NormalDoorAnimator>().OpenMe();

            _StartShowThing(_NextThingToSay(), mbmBg);
            _allowInteract = false;

            //set UnActive to the BoxCollider2D
            _preDoor.transform.parent.Find("ColliderOpener1").gameObject.SetActive(false);
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

        if (mbmM == mbmNormal)//[Tip][20210307]当心! 这里两个状态传了对象
        {
            onDoingStatus = OnDoingStatus_MSGManager.mbmNormal_On;
        }
        else if(mbmBg)
        {
            onDoingStatus = OnDoingStatus_MSGManager.mbmBg_On;
        }
        else if(false)
        {
            //add more box here
        }
        else
        {

        }
        
        mbmM.RunWith(MsgBoxManager.MsgBoxStatus.Running, thingStr);
    }

    private void _stopMoving()
    {
        mC_rig2D.velocity = new Vector3(0f, mC_rig2D.velocity.y);
    }

    private void _keeptMoving(int mode)
    {
        if (mode>0 )
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

    private void _setControlmode(bool mode, int specialSetting)
    {
        switch (specialSetting)
        {
            case 1:
                foreach (Buttoner buttoner in buttoners)
                    if(buttoner!=null)buttoner.pressed = false;
                break;
        }
        _setControlmode(mode);
    }

    // Update is called once per frame

    public void Update()
    {
        //----movement----
        if (_isMsgBoxing == false)
        {
            if (Input.GetKey(KeyCode.A) || buttoners[(int)inputManager.InputManager.EnumStatus.Left].pressed && !buttoners[(int)inputManager.InputManager.EnumStatus.Right].pressed)
            {
                mcAnimator.SetInteger("State", 0);
                _keeptMoving(-1);//left
            }
            else if (Input.GetKey(KeyCode.D) || !buttoners[(int)inputManager.InputManager.EnumStatus.Left].pressed && buttoners[(int)inputManager.InputManager.EnumStatus.Right].pressed)
            {
                mcAnimator.SetInteger("State", 1);
                _keeptMoving(1);//right
            }
            else
            {
                mcAnimator.StopPlayback();
                mcAnimator.SetInteger("State", -1);
                _stopMoving();
            }
        }
        else //_isInteracting == true
        {
            _stopMoving();
            mcAnimator.SetInteger("State", -1);
            mcAnimator.StopPlayback();
            _setControlmode(false,1);
            //print("abc");
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
            case ScriptState.AtFirstTime:
                break;
            case ScriptState.AllowPlaying:
                if(!_isFirst)
                    _setControlmode(true);
                break;
            case ScriptState.OnDoing:
                _setControlmode(false);
                switch (onDoingStatus)
                {
                    case OnDoingStatus_MSGManager.mbmNormal_On:
                        if(mbmNormal.StableFlag == true)
                            onDoingStatus = OnDoingStatus_MSGManager.mbmNormal_Hold;
                        break;
                    case OnDoingStatus_MSGManager.mbmNormal_Hold:
                        //等待mbmNormal加载完毕后
                        if (mbmNormal.Status == MsgBoxManager.MsgBoxStatus.Hiding && mbmNormal.StableFlag)
                        {
                            if (_isFirst)
                            {
                                lightStatus = LightStatus.Off2On;
                                _setControlmode(true);
                                _isFirst = false;
                                scriptState = ScriptState.AllowPlaying;
                                break;
                            }
                            scriptState = ScriptState.AllowPlaying;
                        }
                        break;
                    case OnDoingStatus_MSGManager.mbmBg_On://background

                        if (mbmBg.StableFlag == true)
                            onDoingStatus = OnDoingStatus_MSGManager.mbmBg_Hold;
                        break;
                    case OnDoingStatus_MSGManager.mbmBg_Hold:
                        //等待mbmNormal加载完毕后
                        if (mbmBg.Status == MsgBoxManager.MsgBoxStatus.Hiding && mbmBg.StableFlag)
                        {
                            if (_isFirst)
                            {
                                _StartShowThing("身边一片漆黑。^", mbmNormal);
                                break;
                            }
                            scriptState = ScriptState.AllowPlaying;
                        }
                        break;
                }
                break;
            default:
                break;
        }
    }

    #region playground
    private void _001userInput_SATS(float mbm_Time, int mode)
    {
        switch (mode)
        {
            case 0:
                mbmNormal.timeWaitStateCount_Set = mbm_Time;
                break;
            case 1:
                mbmNormal._AdaptAlphaValueProperties_Set(mbm_Time);
                break;
            case 2:
                mbmBg.timeWaitStateCount_Set = mbm_Time;
                break;
            case 3:
                mbmBg._AdaptAlphaValueProperties_Set(mbm_Time);
                break;
        }
    }
    #endregion
}
