using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MsgBoxManager : MonoBehaviour
{
    public GameObject goBox;
    public GameObject curtain;//background Curtain
    public Text text;

    void Start()
    {

    }
    
    public enum MsgBoxState {
        Running,
        Hidden /* , die*/
    };

    private MsgBoxState _state;
    public MsgBoxState State
    {
        get
        {
            return _state;
        }
    }

    private void _hide()
    {

    }
    private void _run()
    {
    }

    void Update()
    {
        switch(_state)
        {
            case MsgBoxState.Hidden:
                _hide();
                break;
            case MsgBoxState.Running:
                _run();
                break;
            default:
                break;
        }
    }
}
