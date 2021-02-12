using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class _001BlackGroundText : MonoBehaviour
{
    public Text text;
    public Image background;

    public float processingTime;
    public string content;

    private float _timerProcessingTime;
    private int _status;

    public int Status { get => _status;}

    // Start is called before the first frame update
    void Start()
    {
        if (processingTime == 0) processingTime = 3f;
        if (content == "") content = "Sample";
        text.text = content;
        _status = -1; // first
    }
    public void Act()
    {
        _status = 0;
        text.gameObject.SetActive(true);
        background.gameObject.SetActive(true);

        text.color = new Color(text.color.r, text.color.g, text.color.b, 0f);
        background.color = new Color(background.color.r, background.color.g, background.color.b, 0f);
        _timerProcessingTime = 0;
    }


    public void Act(string s,Color textColor)
    {
        content = s;
        _status = 0;
        text.gameObject.SetActive(true);
        background.gameObject.SetActive(true);

        text.text = content;
        text.color = textColor;

        text.color = new Color(text.color.r, text.color.g, text.color.b, 0f);
        background.color = new Color(background.color.r, background.color.g, background.color.b, 0f);
        _timerProcessingTime = 0;
    }
    // Update is called once per frame
    void Update()
    {
        switch(Status)
        {
            case 0:
                _timerProcessingTime += Time.deltaTime;
                if(_timerProcessingTime < processingTime /3)
                {
                    text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (Time.deltaTime / (processingTime / 3)) );
                    background.color = new Color(background.color.r, background.color.g, background.color.b, background.color.a + (Time.deltaTime / (processingTime / 3)) );
                }
                else
                {
                    _status++;
                    _timerProcessingTime = 0;
                }
                break;
            case 1:
                _timerProcessingTime += Time.deltaTime;
                if (_timerProcessingTime > processingTime / 3)
                {
                    _status++;
                    _timerProcessingTime = 0;
                }
                break;
            case 2:
                _timerProcessingTime += Time.deltaTime;
                if (_timerProcessingTime < processingTime / 3)
                {
                    text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (Time.deltaTime / (processingTime / 3)));
                    background.color = new Color(background.color.r, background.color.g, background.color.b, background.color.a - (Time.deltaTime / (processingTime / 3)));
                }
                else
                {
                    _status++;
                    _timerProcessingTime = 0;
                }
                break;
            case 3:
                text.gameObject.SetActive(false);
                background.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }
    public void SetAble()
    {
        _status = -1;
    }

}
