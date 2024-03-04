using System;
using System.Collections;
using System.Collections.Generic;
using STAROCK;
using UnityEngine;

public class TestObject : MonoBehaviour
{ 

    private void OnEnable()
    {
        Debug.Log("Send events");
        EventManager.Send(EventIDs.Test1, new ParamTest(){ID=88});
        EventManager.Send(EventIDs.Test2, 666);
    }
}
