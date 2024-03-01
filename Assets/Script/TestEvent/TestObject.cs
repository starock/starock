using System.Collections;
using System.Collections.Generic;
using STAROCK.Event;
using UnityEngine;

public class TestObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventManager.Send(EventIDs.Test1, new ParamTest(){ID=88});
        EventManager.Send(EventIDs.Test2, 666);
    }

     
}
