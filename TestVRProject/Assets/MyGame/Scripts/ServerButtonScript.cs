using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

using System;
using TMPro;
using HD;

public class ServerButtonScript : MonoBehaviour
{
    MyNetworkManager networkManager;
    private Button serverButton;
    public TextMeshProUGUI notificationText;
    private bool sceneNotChanged = true;
    int lastKnownPhase = 0;

    // Start is called before the first frame update
    void Start()
    {
        networkManager = GameObject.Find("Network").GetComponent<MyNetworkManager>();
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
       // Debug.Log("XR Settings are: " + XRSettings.enabled);

        if (GameObject.FindWithTag("ClientButton") != null)
            GameObject.FindWithTag("ClientButton").SetActive(false);
       
        if (networkManager == null) notificationText.text = "Networkmanager null";

            notificationText.text = "Initialising Server";

            TextMeshProUGUI networkAdressText = GameObject.Find("AdressInput_Text").GetComponent<TextMeshProUGUI>();
            networkManager.InitServer();

           // bool waiting = networkManager.GetUDPChat().ConnectAsServer(notificationText.text);
          //  SceneManager.LoadScene("SampleScene");
            
             Debug.Log("Waiting for Client ...");
             notificationText.text = "Waiting for Client ...";
             networkManager.GetUDPChat().StartListening();

        
        //    Debug.Log("Address: " + networkManager.GetServerNetworkAdress());

    }

    
    private void Update()
    {
        if (null != networkManager && networkManager.initializationComplete == true)
        {
            if (null != networkManager.GetUDPChat())
            {
                Debug.Log("server phase: " + lastKnownPhase + "-" + networkManager.GetUDPChat().GetCurrentPhase());
                if (lastKnownPhase == 0 && networkManager.GetUDPChat().GetCurrentPhase() == 1)
                {
                    Debug.Log("phase 0 - 1 - Client connected.");
                    notificationText.text = "Client connected. Sending answer ...";
                  //  networkManager.GetUDPChat().StartListening();
                    lastKnownPhase = 1;
                }
               
                else if (lastKnownPhase == 1 && networkManager.GetUDPChat().GetCurrentPhase() == 1)
                {
                    Debug.Log("server sending 1");
                    MyMessage msg = new MyMessage(1);
                    networkManager.GetUDPChat().Send(msg);
                    Debug.Log("Sleep2 for 1/2 second.");
                    System.Threading.Thread.Sleep(500);
                }
                else if (lastKnownPhase == 1 && networkManager.GetUDPChat().GetCurrentPhase() == 2)
                {
                    Debug.Log("phase 1 - 2");
                    lastKnownPhase = 2;

                    notificationText.text = "Client got answer. Changing scene ...";
                    networkManager.ChangeScene();
                    
                }
                else if (lastKnownPhase == 2)
                {

                    Destroy(this);
                }
            /*    else
                {
                    Debug.Log("Something went wrong here in: server phase: " + lastKnownPhase + "-" + networkManager.GetUDPChat().GetCurrentPhase());
                }*/
            }
        }
    }
}
