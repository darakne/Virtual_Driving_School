using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.SceneManagement;
using System;

public class ServerButtonScript : MonoBehaviour
{
    private Button serverButton;
    // Start is called before the first frame update
    void Start()
    {
        serverButton = GetComponent<Button>();
        serverButton.onClick.AddListener(ChangeSceneToServer);
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

    void ChangeSceneToServer()
    {
        Debug.Log("Hello serverbutton");
        Debug.Log("XR Settings are: " + XRSettings.enabled);

        GameObject.FindWithTag("ClientButton").SetActive(false);
        // ToggleVR();

    
        SceneManager.LoadScene("SampleScene");
    }

}
