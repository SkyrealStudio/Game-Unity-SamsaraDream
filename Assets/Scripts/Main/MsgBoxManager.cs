using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MsgBoxManager : MonoBehaviour
{
    private UnityEngine.Events.UnityEvent _innerEvent = new UnityEngine.Events.UnityEvent();

    public enum MsgBoxStatus
    {
        Running,
        Hidding
    };

    private enum _TypeForm
    {
        Plain,
        WaitEnding,
        Clear
    }

    private enum _RoleGrayScaleStatus
    {
        Normal,
        ProcessingN2C,
        ProcessingC2N,
        Completed,
    }

    public GameManager gm;

    public Text Textt;
    private string _textout;

    public GameObject goBox;
    public GameObject curtain;

    public GameObject[] roles;
    private int _rolesLength = 0;
    public bool[] _arrNeedProcessRoleGrayscale;
    private _RoleGrayScaleStatus[] _arrRoleGrayScaleState;
    public float[] _arrtimerGrayscaleRole;

    [Range(0, 1f)] public float boxAlpha_SetFloat;
    public float boxAlphaTime_SetFloat;

    [Range(0, 1f)] public float textAlpha_SetFloat;
    public float textAlphaTime_SetFloat;

    private float _timerBoxAlpha;

    [Range(0, 1f)] public float curtainAlpha_SetFloat;
    public float curtainTime_SetFloat;
    private float _timerCurtainAlpha;

    [Range(0, 1f)] public float roleGrayscale_SetFloat;
    public float roleGrayscaleTime_SetFloat;
    private float _timerRoleGrayscale;

    [Range(0, 1f)] public float roleAlpha_SetFloat;
    public float roleAlphaTime_SetFloat;
    private float _timerRoleAlpha;

    public float secondPerChar;
    private float _printTimerCurrent;

    public string nowString;
    public int _msgPointer;

    private float _boxAlpha_percentCurrent;
    private float _curtainAlpha_percentCurrent;
    private float _roleAlpha_percentCurrent;
    public float[] _roleGrayscales_percentCurrent;
    private float _textAlpha_percentCurrent;

    private bool _stableFlag;
    private bool _TabOpenedFlag;


    private _TypeForm _nowtypeForm;
    private string[] _waitingStr = new string[4];
    private int _waitShape = 1;
    private float _timerWaitStateCount = 0f;
    public float timeWaitStateCount_Set;


    private void Start()
    {
        //if (nowString == "") nowString = Textt.text;
        _msgPointer = 0;
        Textt.text = "";
        nowString = "^";

        _nowtypeForm = _TypeForm.Plain;

        goBox.GetComponent<Image>().color = new Color(goBox.GetComponent<Image>().color.r, goBox.GetComponent<Image>().color.g, goBox.GetComponent<Image>().color.b, 0f);
        curtain.GetComponent<Image>().color = new Color(curtain.GetComponent<Image>().color.r, curtain.GetComponent<Image>().color.g, curtain.GetComponent<Image>().color.b, 0f);
        Textt.color = new Color(Textt.color.r, Textt.color.g, Textt.color.b, 0f);
        foreach (GameObject i in roles)
        {
            i.GetComponent<Image>().color = new Color(i.GetComponent<Image>().color.r, i.GetComponent<Image>().color.g, i.GetComponent<Image>().color.b, 0f);
            _rolesLength++;
        }
        _timerBoxAlpha = _timerCurtainAlpha = _timerRoleAlpha = 0f;
        
        
        _arrtimerGrayscaleRole = new float[_rolesLength];
        _arrNeedProcessRoleGrayscale = new bool[_rolesLength];
        _arrRoleGrayScaleState = new _RoleGrayScaleStatus[_rolesLength];
        _roleGrayscales_percentCurrent = new float[_rolesLength];

        for (int i = 0; i < _rolesLength; i++)
        {
            _arrNeedProcessRoleGrayscale[i] = false;
            _arrRoleGrayScaleState[i] = _RoleGrayScaleStatus.Normal;
            _roleGrayscales_percentCurrent[i] = 1f;
            //_arrtimeGrayscaleRole[i] = 0f;
        }
        _stableFlag = true;
        //----AddListener at _innerEvent----


        //----End of AddListener at _innerEvent----
    }
    void Update()
    {
        #region playgroundArea
        if (Input.GetKeyDown(KeyCode.R) && _stableFlag != false)
        {
            _stableFlag = false;
            _status = MsgBoxStatus.Running;
        }
        if (Input.GetKeyDown(KeyCode.H) && _stableFlag != false)
        {
            _stableFlag = false;
            _status = MsgBoxStatus.Hidding;
        }
        #endregion

        switch (_status)
        {
            case MsgBoxStatus.Running:
                _Running();
                break;
            case MsgBoxStatus.Hidding:
                _Hidding();
                break;
            default:
                break;
        }
    }
    private void _Running()
    {
        if (_SwitchingTab(true))
        {
            //----Type&Analysis----
            #region Type&Analysis
            switch (_nowtypeForm)
            {
                case _TypeForm.Plain:
                    _printTimerCurrent += Time.deltaTime;
                    if (_printTimerCurrent >= (secondPerChar / 1f))
                    {
                        for (int i = 0; i <= (int)_printTimerCurrent / (secondPerChar / 1f); i++)
                            _TypeSingle();
                        _printTimerCurrent = 0f;
                    }
                    break;
                default:
                    _Action();
                    break;
            }
            #endregion
            //----RoleGrayScaling----
            for (int i = 0; i < _rolesLength; i++)
            {
                roles[i].GetComponent<Image>().color
                    = new Color((roleGrayscale_SetFloat) + (1 - roleGrayscale_SetFloat) * _roleGrayscales_percentCurrent[i],
                                (roleGrayscale_SetFloat) + (1 - roleGrayscale_SetFloat) * _roleGrayscales_percentCurrent[i],
                                (roleGrayscale_SetFloat) + (1 - roleGrayscale_SetFloat) * _roleGrayscales_percentCurrent[i],
                                roles[i].GetComponent<Image>().color.a);
                _setRoleGrayscale(i, _calcModeRoleGrayScaleState(i));
            }
        }
        else
        {
            //do nothing
        }
    }
    private void _Hidding()
    {
        if (_SwitchingTab(false))
        {
            //codes
        }
        else
        {
            _textout = "";
            nowString = "^";
        }
    }



    private bool _calcModeRoleGrayScaleState(int m_pointer)
    {
        switch(_arrRoleGrayScaleState[m_pointer])
        {
            case _RoleGrayScaleStatus.Completed:
            case _RoleGrayScaleStatus.ProcessingC2N:
                return false;//back to normal
            case _RoleGrayScaleStatus.Normal:
            case _RoleGrayScaleStatus.ProcessingN2C:
                return true;//to grayscale
            default:
                Debug.LogError("Error at _RoleGrayScaleState -- no such selection");
#if UNITY_EDITOR
                EditorApplication.isPaused = true;
#endif
                return false;
        }
    }

    private MsgBoxStatus _status;
    public MsgBoxStatus Status
    {
        get
        {
            return _status;
        }
        set
        {
            _status = value;
        }
    }

    private bool _SwitchingTab(bool mode)
    {
        //----set AlphaValue Dynamaicly----
        
        goBox.GetComponent<Image>().color = new Color(goBox.GetComponent<Image>().color.r, goBox.GetComponent<Image>().color.g, goBox.GetComponent<Image>().color.b, _boxAlpha_percentCurrent * boxAlpha_SetFloat);
        curtain.GetComponent<Image>().color = new Color(curtain.GetComponent<Image>().color.r, curtain.GetComponent<Image>().color.g, curtain.GetComponent<Image>().color.b, _curtainAlpha_percentCurrent * curtainAlpha_SetFloat);
        Textt.color = new Color(Textt.color.r, Textt.color.g, Textt.color.b, _textAlpha_percentCurrent * textAlpha_SetFloat);

        foreach (GameObject i in roles)
        {
            i.GetComponent<Image>().color = new Color(i.GetComponent<Image>().color.r, i.GetComponent<Image>().color.g, i.GetComponent<Image>().color.b, _roleAlpha_percentCurrent * roleAlpha_SetFloat);
        }

        if (_stableFlag == false)
        {
            if (
            _timerBoxAlpha > boxAlphaTime_SetFloat &&
            _timerCurtainAlpha > curtainTime_SetFloat &&
            _timerRoleAlpha > roleAlphaTime_SetFloat)
            {
                _stableFlag = true;

                if (!mode/* && _TabOpenedFlag == true*/)
                {
                    if(gm!=null)
                        gm.GameResume();
                    _TabOpenedFlag = false;

                    goBox.SetActive(false);
                    curtain.SetActive(false);
                    foreach (GameObject i in roles)
                        i.SetActive(false);
                }
                _timerBoxAlpha = _timerCurtainAlpha = _timerRoleAlpha = 0f;
                return _stableFlag;
            }

            else
            {

                _stableFlag = false;
                if (mode && _TabOpenedFlag == false)
                {
                    if(gm!=null)
                        gm.GamePause();
                    _TabOpenedFlag = true;

                    //----set Active?----
                    goBox.SetActive(true);
                    curtain.SetActive(true);
                    foreach (GameObject i in roles)
                        i.SetActive(true);
                }

                _timerBoxAlpha += Time.deltaTime;
                _timerCurtainAlpha += Time.deltaTime;
                _timerRoleAlpha += Time.deltaTime;

                _boxAlpha_percentCurrent += (mode ? 1f : -1f) * (Time.deltaTime / boxAlphaTime_SetFloat);
                _curtainAlpha_percentCurrent += (mode ? 1f : -1f) * (Time.deltaTime / curtainTime_SetFloat);
                _roleAlpha_percentCurrent += (mode ? 1f : -1f) * (Time.deltaTime / roleAlphaTime_SetFloat);
                _textAlpha_percentCurrent += (mode ? 1f : -1f) * (Time.deltaTime / textAlphaTime_SetFloat);


                return _stableFlag;

            }
        }
        else
        {
            //return flag;
            return true;
        }
    }


    private void _setRoleGrayscale(int m_rolePointer,bool mode)
    {
        if (_arrNeedProcessRoleGrayscale[m_rolePointer] == false)
        {
            return;
        }

        if ((m_rolePointer >= _rolesLength || m_rolePointer < 0))
        {
            Debug.LogWarning("out of range _rolesLength");
            return;
        }

        _arrRoleGrayScaleState[m_rolePointer] = (mode) ? _RoleGrayScaleStatus.ProcessingN2C : _RoleGrayScaleStatus.ProcessingC2N;

        _arrtimerGrayscaleRole[m_rolePointer] += Time.deltaTime;

        _roleGrayscales_percentCurrent[m_rolePointer] -= (mode ? 1f : -1f) * (Time.deltaTime / roleGrayscaleTime_SetFloat);

        if (mode ? (_roleGrayscales_percentCurrent[m_rolePointer] < 0f) : (_roleGrayscales_percentCurrent[m_rolePointer] > 1f)/*|| _arrtimerGrayscaleRole[m_rolePointer] > roleGrayscaleTime_SetFloat*/)
        {
            _arrRoleGrayScaleState[m_rolePointer] = (mode) ? _RoleGrayScaleStatus.Completed: _RoleGrayScaleStatus.Normal;
            _arrNeedProcessRoleGrayscale[m_rolePointer] = false;
        }
    }


    #region Type&Analysis_Functions

    private void _Action()
    {
        switch (_nowtypeForm)
        {
            case _TypeForm.WaitEnding:
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0) || _IsTouched_Experimental() )
                {
                    _SetNowTypeForm(_TypeForm.Plain);
                    break;
                }
                _Type();
                break;

            case _TypeForm.Clear:
                //this shouldn't happened
                Debug.LogWarning("MsgBox进入了一个 _TypeForm.Clear 的 _TypeForm");
                break;
            default:
                //this shouldn't happened
                Debug.LogWarning("MsgBox进入了一个 未知 的_TypeForm");
                break;
        }
    }

    private void _TypeSingle()
    {
        //[Tip][20210210]我觉得这里可配上打字机音效(如果必要)

        //[Tip][20210210]请在_TypeForm.Plain下使用
        if (_msgPointer < nowString.Length)
        {
            if (nowString[_msgPointer] == '^')
            {
                _SetNowTypeForm(_TypeForm.WaitEnding);
                _Type();
                _msgPointer++;
            }
            else if (nowString[_msgPointer] == '~')
            {
                _SetNowTypeForm(_TypeForm.Clear);
                _Type();
                _msgPointer++;
            }
            else
            {
                _textout += nowString[_msgPointer];
                _Type();
                _msgPointer++;
            }
        }
        else
        {
            if (nowString == "^") return;
            //close This Tab
            _stableFlag = false;
            _status = MsgBoxStatus.Hidding;
            //[Tip][20210210]其实由此可知, 在大多数情况下, 你的文本末尾都会添加用以加载_TypeForm.WaitEnding的标记, 否则玩家来不及阅读文本
        }
    }

    //this _Type means Put-out stream
    private void _Type()
    {
        switch (_nowtypeForm)
        {
            case _TypeForm.Plain:
                Textt.text = _textout;
                break;
            case _TypeForm.WaitEnding:
                _timerWaitStateCount += Time.deltaTime;
                if (_timerWaitStateCount >= timeWaitStateCount_Set)
                {
                    _timerWaitStateCount = 0f;
                    ++_waitShape;
                    if (_waitShape > 3)
                    {
                        _waitShape = 1;
                    }
                    Textt.text = _waitingStr[_waitShape];
                }
                break;
            case _TypeForm.Clear:
                _textout = "";
                _SetNowTypeForm(_TypeForm.Plain);
                break;
            default:
                break;
        }
    }

    private void _SetNowTypeForm(_TypeForm t)
    {
        if (t == _TypeForm.WaitEnding)
        {
            _waitingStr[0] = _textout;
            _waitingStr[1] = _textout + " .";
            _waitingStr[2] = _textout + " . .";
            _waitingStr[3] = _textout + " . . .";
        }
        else if (t == _TypeForm.Plain && _nowtypeForm == _TypeForm.WaitEnding)
        {
            if (_waitingStr[0] != null)
            {
                _textout = _waitingStr[0];
            }
            else { } //do nothing
        }
        _nowtypeForm = t;
    }

    #endregion

    #region playground
    private bool _IsTouched_Experimental()
    {
        return (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began);
    }

    public void RunWith(MsgBoxStatus state, string str)
    {
        _ResetNowString(str);
        SetStatus(state);
    }

    public void _ResetNowString(string str)
    {
        Textt.text = "";
        nowString = str;
        _msgPointer = 0;
    }
    

    public void SetStatus(MsgBoxStatus state)
    {
        _stableFlag = false;
        _status = state;
    }

    #endregion
}
