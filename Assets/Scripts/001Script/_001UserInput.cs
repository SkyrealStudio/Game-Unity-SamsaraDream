using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class _001UserInput : MonoBehaviour
{
    public UnityEvent<float,int> alterTimeSetValue = new UnityEvent<float,int>();

    public Buttoner buttonerCommit;
    public Buttoner buttonerSwitch;
    public InputField inputField;
    public Text placeholder_text;
    private string[] placeholder_strings = new string[8];
    public int nowStatus = 0;

    // Start is called before the first frame update
    private void Start()
    {
        placeholder_text.text = 
        placeholder_strings[0] = "在此输入数字微调--文本框字速度--(毫秒)按下按钮生效";
        placeholder_strings[1] = "在此输入数字微调--文本框淡入淡出时间--(毫秒)按下按钮生效";
        placeholder_strings[2] = "在此输入数字微调--黑幕字速度--(毫秒)按下按钮生效";
        placeholder_strings[3] = "在此输入数字微调--黑幕淡入淡出时间--(毫秒)按下按钮生效";

        buttonerCommit.ClickEvent.AddListener(PlayMe);
        buttonerSwitch.ClickEvent.AddListener(SwitchMe);
    }

    private void SwitchMe()
    {
        placeholder_text.text = placeholder_strings[(++nowStatus == 4) ? nowStatus=0 : nowStatus] + placeholder_strings[nowStatus+4];
    }

    public void PlayMe()
    {
        if(inputField.text.Length > 0)
        {
            float reVal = (float)int.Parse(inputField.text) / 1000f;
            inputField.text = "";
            placeholder_strings[nowStatus+4] = "\n已调整为 " + reVal+" second(s)";
            placeholder_text.text = placeholder_strings[nowStatus] + placeholder_strings[nowStatus + 4];
            alterTimeSetValue.Invoke(reVal, nowStatus);
        }
    }

    private void Update()
    {
        buttonerSwitch.gameObject.SetActive(inputField.text.Length == 0);
    }
}
