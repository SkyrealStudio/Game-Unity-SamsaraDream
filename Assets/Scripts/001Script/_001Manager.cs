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
    public _001Character characterManager;

    //public _001BlackGroundText hdbzManager;
    public MsgBoxManager mbmNormal;
    public MsgBoxManager mbmHDBZ;

    public enum ScriptState
    {
        OnWaiting,
        OnDoing
    }
    public ScriptState scriptState;
    public int performingPointer = 0;
    
    public Light2D l2d_white; 
    //public LightDimer ld;

    private float _l2d_outerRadiusSet;
    private float _l2d_IntensitySet;
    public float l2d_TimeSet;
    private float _l2d_timer;

    public CameraManager cm;
    public GameObject camHold;
    public GameObject mainCharacter;
    public Rigidbody2D mC_rig2D;
    public Collider2D mC_col2D;

    public Buttoner[] buttoners;

    public GameObject groundGroup;
    public float groundFloatAdd;

    public Vector3 V3_groundGroupPre = new Vector3 (0f,0f,-10f);
    

    public float speedX;
    //public float settledValue_x;

    private Vector3 positionOrigion;

    // Start is called before the first frame update
    void Start()
    {
        _setControlmode(false);

        _l2d_outerRadiusSet = l2d_white.pointLightOuterRadius; 
        _l2d_IntensitySet = l2d_white.intensity;

        l2d_white.pointLightOuterRadius = 0f;
        l2d_white.intensity = 0f;
        

        performingPointer = 0;
        scriptState = ScriptState.OnDoing;

        cm.SetFollowingGameObject(camHold);
        cm.StartFollowing();
        positionOrigion = mainCharacter.transform.position;
        _stopMoving();
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

    private void _setControlmode(bool mode)
    {
        foreach(Buttoner buttoner in buttoners)
        {
            buttoner.gameObject.SetActive(mode);
            buttoner.gameObject.GetComponent<Button>().interactable = mode;
        }
    }

    // Update is called once per frame

    public 
    
    void Update()
    {
        //----

        //----

        //----movement----
        if(buttoners[(int)inputManager.InputManager.EnumStatus.Left].pressed && !buttoners[(int)inputManager.InputManager.EnumStatus.Right].pressed)
        {
            _keeptMoving(false);
        }
        else if (!buttoners[(int)inputManager.InputManager.EnumStatus.Left].pressed && buttoners[(int)inputManager.InputManager.EnumStatus.Right].pressed)
        {
            _keeptMoving(true);
        }
        else
        {
            _stopMoving();
        }
        //----End of movement----

        switch (scriptState)
        {
            case ScriptState.OnWaiting:
                //让孩子们去做事
                break;
            case ScriptState.OnDoing:
                switch(performingPointer)
                {
                    case 0:
                        mbmNormal.RunWith(MsgBoxManager.MsgBoxStatus.Running, "身边一片漆黑^~");// + "像是知道目的地一样，我不断地向路的尽头走去^~" + "…目标是路的尽头吗，我不知道^~" + "路的尽头是光明吗…我不知道^~" + "我停下了脚步^~");
                        performingPointer++;
                        break;
                    case 1:
                        //等待mbmNormal加载完毕后
                        if (mbmNormal.Status == MsgBoxManager.MsgBoxStatus.Hidding)
                        {
                            performingPointer++;
                        }
                        break;
                    case 2:
                        mbmHDBZ.RunWith(MsgBoxManager.MsgBoxStatus.Running, "身边一片漆黑*~");// + "像是知道目的地一样，我不断地向路的尽头走去^~" + "…目标是路的尽头吗，我不知道^~" + "路的尽头是光明吗…我不知道^~" + "我停下了脚步^~");
                        performingPointer++;
                        break;
                    case 3:
                        //等待mbmNormal加载完毕后
                        if (mbmHDBZ.Status == MsgBoxManager.MsgBoxStatus.Hidding)
                        {
                            performingPointer++;
                        }
                        break;
                }
                break;
            default:
                break;
        }
    }
}
