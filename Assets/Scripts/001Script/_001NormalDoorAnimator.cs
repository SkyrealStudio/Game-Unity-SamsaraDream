using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Experimental.Rendering.Universal;

public class _001NormalDoorAnimator : MonoBehaviour
{
    private float _timerAnime = 0f;
    public float timeAnime_SetFloat;
    public bool _stableFlag;

    private Queue<int> _orderQueue = new Queue<int>(0);
    public Sprite[] sprites;

    public enum EnumState
    {
        Open,
        Open2Close,
        Close,
        Close2Open
    }
    private EnumState _state;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = sprites[0];
        _setCaster(0, true);
        _setCaster(1, false);
        _setCaster(2, false);

        _stableFlag = true;
        _state = EnumState.Close;
    }

    public void OpenMe()
    {
        _orderQueue.Enqueue(1);
    }

    public void CloseMe()
    {
        _orderQueue.Enqueue(2);
    }

    private void _setCaster(int n,bool mode)
    {
        gameObject.transform.Find("CasterUpper" + n.ToString()).gameObject.SetActive(mode);
        gameObject.transform.Find("CasterLower" + n.ToString()).gameObject.SetActive(mode);
    }

    // Update is called once per frame
    void Update()
    {
        if(_stableFlag)
        {
            if (_orderQueue.Count > 0)
            {
                switch (_orderQueue.Dequeue())
                {
                    case 1:
                        if (_state != EnumState.Close) break;
                        _stableFlag = false;
                        _state = EnumState.Close2Open;
                        break;
                    case 2:
                        if (_state != EnumState.Open) return;
                        _stableFlag = false;
                        _state = EnumState.Open2Close;
                        break;
                    default:
                        break;
                }
            }
        }
        else
        {
            switch(_state)
            {
                case EnumState.Open:
                    _setCaster(1, false);
                    _setCaster(2, true);
                    gameObject.GetComponent<SpriteRenderer>().sprite = sprites[2];
                    _stableFlag = true;
                    break;
                case EnumState.Open2Close:
                    _setCaster(2, false);
                    _setCaster(1, true);
                    gameObject.GetComponent<SpriteRenderer>().sprite = sprites[1];
                    //
                    _timerAnime += Time.deltaTime;
                    if (_timerAnime>timeAnime_SetFloat)
                    {
                        _timerAnime = 0f;
                        _state = EnumState.Close;
                    }
                    break;
                case EnumState.Close:
                    _setCaster(1, false);
                    _setCaster(0, true);
                    gameObject.GetComponent<SpriteRenderer>().sprite = sprites[0];
                    _stableFlag = true;
                    break;
                case EnumState.Close2Open:
                    _setCaster(0, false);
                    _setCaster(1, true);
                    gameObject.GetComponent<SpriteRenderer>().sprite = sprites[1];
                    //
                    _timerAnime += Time.deltaTime;
                    if(_timerAnime>timeAnime_SetFloat)
                    {
                        _timerAnime = 0f;
                        _state = EnumState.Open;
                    }
                    break;
            }
        }
    }
}
