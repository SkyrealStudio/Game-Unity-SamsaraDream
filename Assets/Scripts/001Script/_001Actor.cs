using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using EnumActorOptions;
using static _001Manager;

namespace EnumActorOptions
{
    public enum ActOptions
    {
        First001OnLoad
    }

}


public class _001Actor : MonoBehaviour
{
    public _001Manager.OnDoingStatus_MSGManager onDoingStatus_MSGManager;
    public _001Manager manager;

    public UnityEvent completedEvent = new UnityEvent();

    private MsgBoxManager mbmNormal;
    private MsgBoxManager mbmBg;


    private enum ScriptStatus
    {
        Sleeping,
        Acting
    }
    private ScriptStatus scriptStatus;
    
    private ActOptions actOption;

    internal bool haveMission = false;
    

    private void Start()
    {
        mbmNormal = manager.mbmNormal;
        mbmBg = manager.mbmBg;
        scriptStatus = ScriptStatus.Sleeping;
    }

    public void AddMision(ActOptions inOption)
    {
        actOption = inOption;
        haveMission = true;
    }

    public void StartActing()
    {
        scriptStatus = ScriptStatus.Acting;
    }

    public void _StartShowThing_Actor(string thingStr, MsgBoxManager mbmM)
    {
        if (mbmM == mbmNormal)//[Tip][20210307]当心! 这里两个状态传了对象
        {
            onDoingStatus_MSGManager = OnDoingStatus_MSGManager.mbmNormal_On;
        }
        else if (mbmBg)
        {
            onDoingStatus_MSGManager = OnDoingStatus_MSGManager.mbmBg_On;
        }
        else if (false)
        {
            //add more box here
        }
        else
        {

        }

        mbmM.RunWith(MsgBoxManager.MsgBoxStatus.Running, thingStr);
    }

    private void ActFrame()
    {
        if (haveMission && actOption==ActOptions.First001OnLoad)
        {
            switch (onDoingStatus_MSGManager)
            {
                case OnDoingStatus_MSGManager.mbmBg_On://background
                    if (mbmBg.StableFlag == true)
                        onDoingStatus_MSGManager = OnDoingStatus_MSGManager.mbmBg_Hold;
                    break;
                case OnDoingStatus_MSGManager.mbmBg_Hold:
                    if (mbmBg.Status == MsgBoxManager.MsgBoxStatus.Hiding && mbmBg.StableFlag)
                    {
                        _StartShowThing_Actor("身边一片漆黑。^", mbmNormal);
                        break;
                    }
                    break;
                case OnDoingStatus_MSGManager.mbmNormal_On:
                    if (mbmNormal.StableFlag == true)
                        onDoingStatus_MSGManager = OnDoingStatus_MSGManager.mbmNormal_Hold;
                    break;
                case OnDoingStatus_MSGManager.mbmNormal_Hold:
                    if (mbmNormal.Status == MsgBoxManager.MsgBoxStatus.Hiding && mbmNormal.StableFlag)
                    {
                        manager.lightStatus = LightStatus.Off2On;
                        haveMission = false;
                        //actorReport
                        completedEvent.Invoke();
                    }
                    break;
            }
        }
    }


    
    

    void Update()
    {
        if (haveMission)
        {
            switch(scriptStatus)
            {
                case ScriptStatus.Acting:
                    {
                        ActFrame();
                    }
                    break;
                case ScriptStatus.Sleeping:
                    {

                    }
                    break;
            }
        }

        else
        {
            
        }
    }
}
