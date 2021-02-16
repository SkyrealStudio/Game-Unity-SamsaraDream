using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

//UnityUI事件:https://www.cnblogs.com/llstart-new0201/p/7102868.html
//官方文档:https://docs.unity3d.com/530/Documentation/ScriptReference/EventSystems.EventTriggerType.html

public class Buttoner : MonoBehaviour
{
    public UnityEvent ClickEvent = new UnityEvent();
    public bool pressed;

    private void AddTriggersListener(GameObject obj, EventTriggerType eventID, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = obj.AddComponent<EventTrigger>();
        }

        if (trigger.triggers.Count == 0)
        {
            trigger.triggers = new List<EventTrigger.Entry>();
        }

        UnityAction<BaseEventData> callback = new UnityAction<BaseEventData>(action);
        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = eventID
        };
        entry.callback.AddListener(callback);

        trigger.triggers.Add(entry);
    }

    private void Awake()
    {
        AddTriggersListener(gameObject, EventTriggerType.PointerDown, MPress);
        AddTriggersListener(gameObject, EventTriggerType.PointerUp, MUnPress);
    }

    public void MPress(BaseEventData bEData)
    {
        pressed = true;
        ClickEvent.Invoke();
        //Debug.Log("MyOnPressed Called.");
    }

    public void MUnPress(BaseEventData bEData)
    {
        pressed = false;
        //Debug.Log("MyLeavingButton Called.");
    }

}