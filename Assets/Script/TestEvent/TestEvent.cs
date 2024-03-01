using System;
using System.Collections;
using System.Collections.Generic;
using STAROCK.Event;
using UnityEngine;



public struct ParamTest
{
    public int ID;
}


public class TestEvent : MonoBehaviour
{ 
    private void OnEnable()
    {
        Debug.Log("Register events");
        EventManager.Register(EventIDs.Test1, new Action<ParamTest>(Test));
        EventManager.Register(EventIDs.Test2, new Action<int>(Test2));
    }

    private void OnDisable()
    {
        Debug.Log("Unregister events");
        EventManager.Unregister(EventIDs.Test1, new Action<ParamTest>(Test));
        EventManager.Unregister(EventIDs.Test2, new Action<int>(Test2));
    }

    private void Test(ParamTest param)
    {
        Debug.Log("test event "+param.ID);
    }
    
    private void Test2(int param)
    {
        Debug.Log("test event "+param);
    }
}
