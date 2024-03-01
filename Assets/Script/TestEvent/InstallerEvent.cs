using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstallerEvent : MonoBehaviour
{
    public List<MonoBehaviour> scriptsToInitialize; // 保存需要初始化的脚本对象列表

    private void Start()
    {
        InitializeScriptsInOrder();
    }

    private void InitializeScriptsInOrder()
    {
        foreach (MonoBehaviour script in scriptsToInitialize)
        {
            script.transform.gameObject.SetActive(true);
            // script.SendMessage("CustomInitializationMethod", SendMessageOptions.DontRequireReceiver);
        }
    }
}
