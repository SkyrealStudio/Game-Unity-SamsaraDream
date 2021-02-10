using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MsgBoxManager : MonoBehaviour
{
    public GameManager gm;

    public Text Textt;
    private string _textout;

    public GameObject goBox;
    public GameObject curtain;
    public GameObject[] roles;
    private int _rolesCount = 0;
    private bool[] _arrIsProcessingGrayscaleRole;
    private float[] _arrtimeGrayscaleRole;


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

    public float printSpeed;
    private float _printTimerCurrent;
    
    private int _count;
    
    private float _boxAlpha_percentCurrent;
    private float _curtainAlpha_percentCurrent;
    private float _roleAlpha_percentCurrent;
    private float[] _roleGrayscales_percentCurrent;
    private float _textAlpha_percentCurrent;

    private bool _stableFlag;
    private bool _TabOpenedFlag;


    private void Start()
    {
        goBox.GetComponent<Image>().color = new Color(goBox.GetComponent<Image>().color.r, goBox.GetComponent<Image>().color.g, goBox.GetComponent<Image>().color.b, 0f);
        curtain.GetComponent<Image>().color = new Color(curtain.GetComponent<Image>().color.r, curtain.GetComponent<Image>().color.g, curtain.GetComponent<Image>().color.b, 0f);
        Textt.color = new Color(Textt.color.r, Textt.color.g, Textt.color.b, 0f);
        foreach (GameObject i  in roles)
        {
            i.GetComponent<Image>().color = new Color(i.GetComponent<Image>().color.r, i.GetComponent<Image>().color.g, i.GetComponent<Image>().color.b, 0f);
            _rolesCount++;
        }
        _timerBoxAlpha = _timerCurtainAlpha = _timerRoleAlpha = 0f;
        _arrIsProcessingGrayscaleRole = new bool[_rolesCount];
        _arrtimeGrayscaleRole = new float[_rolesCount];

        for (int i = 0; i < _rolesCount; i++)
        {
            _arrIsProcessingGrayscaleRole[i] = false;
            //_arrtimeGrayscaleRole[i] = 0f;
        }
        
        _stableFlag = true;
    }   

    public enum MsgBoxState {
        Initing,
        Running,
        Hidding
    };

    private MsgBoxState _state;
    public MsgBoxState State
    {
        get
        {
            return _state;
        }
    }

    private bool _SwitchingTab(bool mode)
    {
        if (_stableFlag == false)
        {
            if (
            _timerBoxAlpha > boxAlphaTime_SetFloat &&
            _timerCurtainAlpha > curtainTime_SetFloat &&
            _timerRoleAlpha > roleAlphaTime_SetFloat)
            {
                _stableFlag = true;

                if (!mode)
                {
                    gm.GameResume();
                    _TabOpenedFlag = false;
                }
                _timerBoxAlpha = _timerCurtainAlpha = _timerRoleAlpha = 0f;
                return _stableFlag;
            }

            else
            {

                _stableFlag = false;
                if (mode)
                {
                    gm.GamePause();
                    _TabOpenedFlag = true;
                    //[Tip][20210210]I know, this will be done much times, see "GamePause()" itself.
                    //[Tip][20210210]然后..yc你就感觉心情差的时候改改吧(治疗血压低2333) -- by wyc
                }

                _timerBoxAlpha += Time.deltaTime;
                _timerCurtainAlpha += Time.deltaTime;
                _timerRoleAlpha += Time.deltaTime;

                _boxAlpha_percentCurrent        += (mode ? 1f : -1f) * (Time.deltaTime / boxAlphaTime_SetFloat);
                _curtainAlpha_percentCurrent    += (mode ? 1f : -1f) * (Time.deltaTime / curtainTime_SetFloat);
                _roleAlpha_percentCurrent       += (mode ? 1f : -1f) * (Time.deltaTime / roleAlphaTime_SetFloat);
                _textAlpha_percentCurrent       += (mode ? 1f : -1f) * (Time.deltaTime / textAlphaTime_SetFloat);
                

                return _stableFlag;

            }
        }
        else
        {
            //return flag;
            return true;
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
            //do nothing
        }
    }
    private void _Running()
    {
        if(_SwitchingTab(true))
        {
            //codes
        }
        else
        {
            //do nothing
        }
    }
    
    private void _TypeSingle(char c)
    {
        //[Tip][20210210]我觉得这里可配上打字机等效果(如果必要)
        _textout += c.ToString();
    }

    void Update()
    {
        //_TypeSingle('t');

        //----set Active?----
        goBox.SetActive(_TabOpenedFlag);
        curtain.SetActive(_TabOpenedFlag);
        foreach (GameObject i in roles)
            i.SetActive(_TabOpenedFlag);


        //----set AlphaValue Dynamaicly----
        goBox.GetComponent<Image>().color = new Color(goBox.GetComponent<Image>().color.r, goBox.GetComponent<Image>().color.g, goBox.GetComponent<Image>().color.b, _boxAlpha_percentCurrent * boxAlpha_SetFloat);
        curtain.GetComponent<Image>().color = new Color(curtain.GetComponent<Image>().color.r, curtain.GetComponent<Image>().color.g, curtain.GetComponent<Image>().color.b, _curtainAlpha_percentCurrent * curtainAlpha_SetFloat);
        Textt.color = new Color(Textt.color.r, Textt.color.g, Textt.color.b, _textAlpha_percentCurrent * textAlpha_SetFloat);

        Textt.text = _textout;

        foreach (GameObject i in roles)
        {
            i.GetComponent<Image>().color = new Color(i.GetComponent<Image>().color.r, i.GetComponent<Image>().color.g, i.GetComponent<Image>().color.b, _roleAlpha_percentCurrent * roleAlpha_SetFloat);
        }
        
        if (Input.GetKeyDown(KeyCode.R) && _stableFlag != false)
        {
            _stableFlag = false;
            _state = MsgBoxState.Running;
        }
        if (Input.GetKeyDown(KeyCode.H) && _stableFlag != false)
        {
            _stableFlag = false;
            _state = MsgBoxState.Hidding;
        }

        switch (_state)
        {
            case MsgBoxState.Hidding:
                _Hidding();
                break;
            case MsgBoxState.Running:
                _Running();
                break;
            default:
                break;
        }
    }

}
