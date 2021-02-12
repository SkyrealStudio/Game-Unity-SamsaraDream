using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class _001Manager : MonoBehaviour
{
    public _001BlackGroundText hdbzManager;
    public MsgBoxManager mbmNormal;

    public enum ScriptState
    {
        OnWaiting,
        OnDoing
    }
    public ScriptState scriptState;
    public int performingPointer = 0;

    public LightDimer ld;

    public CameraManager cm;
    public GameObject camHold;
    public GameObject mainCharacter;
    public Rigidbody2D mC_rig2D;
    public Collider2D mC_col2D;

    public GameObject groundGroup;
    public float groundFloatAdd;

    public Vector3 V3_groundGroupPre = new Vector3 (0f,0f,-10f);
    

    public float speedX;
    //public float settledValue_x;

    private Vector3 positionOrigion;

    // Start is called before the first frame update
    void Start()
    {
        performingPointer = 0;
        scriptState = ScriptState.OnDoing;

        cm.SetFollowingGameObject(camHold);
        cm.StartFollowing();
        positionOrigion = mainCharacter.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (performingPointer < 6)
        {
            mC_rig2D.velocity = new Vector3(speedX, mC_rig2D.velocity.y);
        }
        else
        {
            mC_rig2D.velocity = new Vector3(0f, mC_rig2D.velocity.y);
        }
        switch(scriptState)
        {
            case ScriptState.OnWaiting:
                //让孩子们去做事
                break;
            case ScriptState.OnDoing:
                switch(performingPointer)
                {
                    case 0:
                        switch(hdbzManager.Status)
                        {
                            case -1:
                                if(true)//可等待判定
                                    hdbzManager.Act("…这是哪…" + System.Environment.NewLine + "好黑…",Color.white);
                                break;
                            case 3:
                                hdbzManager.SetAble();
                                performingPointer++;//下一剧本...
                                break;
                            default:
                                break;
                        }
                        break;
                    case 1:
                        mbmNormal.RunWith(MsgBoxManager.MsgBoxStatus.Running, "身边一片漆黑^~" + System.Environment.NewLine + "像是知道目的地一样，我不断地向路的尽头走去^~" + System.Environment.NewLine + "…目标是路的尽头吗，我不知道^~" + System.Environment.NewLine + "路的尽头是光明吗…我不知道^~" + System.Environment.NewLine + "我停下了脚步^~");
                        performingPointer++;
                        break;
                    case 2:
                        //等待mbmNormal加载完毕后
                        if (mbmNormal.Status == MsgBoxManager.MsgBoxStatus.Hidding)
                            performingPointer++;
                        break;
                    case 3:
                        switch (hdbzManager.Status)
                        {
                            case -1:
                                if (true)//可等待判定
                                    hdbzManager.Act("你还是来了",Color.white);
                                break;
                            case 3:
                                hdbzManager.SetAble();
                                performingPointer++;//下一剧本...
                                break;
                            default:
                                break;
                        }
                        break;
                    case 4:
                        switch (hdbzManager.Status)
                        {
                            case -1:
                                if (true)//可等待判定
                                    hdbzManager.Act("熟悉的，少女的声音，出现在路的尽头" + System.Environment.NewLine +"我向着那个声音追去，最后干脆跑了起来" + System.Environment.NewLine +"再快点，或许就能追上，在无尽漆黑之路中出现的希望之音" + System.Environment.NewLine +"只要顺着这个方向前进，或许就能找到，无尽黑暗中的救赎",Color.white);
                                break;
                            case 3:
                                hdbzManager.SetAble();
                                performingPointer++;//下一剧本...
                                break;
                            default:
                                break;
                        }
                        break;
                    case 5:
                        switch (hdbzManager.Status)
                        {
                            case -1:
                                if (true)//可等待判定
                                    hdbzManager.Act("我在这里哦",Color.red);
                                break;
                            case 3:
                                hdbzManager.SetAble();
                                performingPointer++;//下一剧本...
                                break;
                            default:
                                break;
                        }
                        break;
                    case 6:
                        //rending door
                        //此时玩家不动
                        switch (hdbzManager.Status)
                        {
                            case -1:
                                if (true)//可等待判定
                                    hdbzManager.Act("不是来见我的吗", Color.red);
                                break;
                            case 3:
                                hdbzManager.SetAble();
                                performingPointer++;//下一剧本...
                                break;
                            default:
                                break;
                        }

                        break;
                    case 7:
                        mbmNormal.RunWith(MsgBoxManager.MsgBoxStatus.Running, "不要….不要…^~" + System.Environment.NewLine + "为什么脚在擅自的动，快停下！^~");
                        performingPointer++;
                        break;
                    case 8:
                        //等待mbmNormal加载完毕后
                        if (mbmNormal.Status == MsgBoxManager.MsgBoxStatus.Hidding)
                            performingPointer++;  
                        break;
                    case 9:
                        switch (hdbzManager.Status)
                        {
                            case -1:
                                if (true)//可等待判定
                                    hdbzManager.Act("这是你导致的后果", Color.red);
                                break;
                            case 3:
                                hdbzManager.SetAble();
                                performingPointer++;//下一剧本...
                                break;
                            default:
                                break;
                        }
                        break;
                    case 10:
                        mbmNormal.RunWith(MsgBoxManager.MsgBoxStatus.Running, "不要….不要…^~" + System.Environment.NewLine + "不要记起…不要靠近…^~");
                        performingPointer++;
                        break;
                    case 11:
                        //等待mbmNormal加载完毕后
                        if (mbmNormal.Status == MsgBoxManager.MsgBoxStatus.Hidding)
                            performingPointer++;
                        break;
                    case 12:
                        switch (hdbzManager.Status)
                        {
                            case -1:
                                if (true)//可等待判定
                                    hdbzManager.Act("不要！", Color.red);
                                break;
                            case 3:
                                hdbzManager.SetAble();
                                performingPointer++;//下一剧本...
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
    }
}
