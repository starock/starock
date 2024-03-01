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
    private void Awake()
    {
        EventManager.Register(EventIDs.Test1, new Action<ParamTest>(Test));
        EventManager.Register(EventIDs.Test2, new Action<int>(Test2));
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
