using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.SceneManagement;
using System;

public class StartXR : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ToggleVR();
    }

    IEnumerator LoadDevice(string newDevice)
    {
        if (String.Compare(XRSettings.loadedDeviceName, newDevice, true) != 0)
        {
            XRSettings.LoadDeviceByName(newDevice);
            yield return null;
            XRSettings.enabled = true;
        }
    }


    //http://talesfromtherift.com/googlevr-cardboard-switch-between-normal-mode-and-vr-mode-at-run-time/
    // https://docs.unity3d.com/ScriptReference/XR.XRSettings.LoadDeviceByName.html
    void ToggleVR()
    {

        if (XRSettings.loadedDeviceName == "Cardboard")
        {
            //  XRSettings.enabled = false;
            StartCoroutine(LoadDevice("None"));
        }
        else
        {
            XRSettings.enabled = true;
            Screen.orientation = ScreenOrientation.AutoRotation;
            StartCoroutine(LoadDevice("Cardboard"));
        }

        Debug.Log("XR Settings are: " + XRSettings.enabled);
        Debug.Log("active: " + XRSettings.isDeviceActive);
        Debug.Log("supported: " + XRSettings.supportedDevices);
    }
}
