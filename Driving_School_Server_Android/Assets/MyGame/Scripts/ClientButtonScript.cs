using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.SceneManagement;
using System;
using TMPro;
using HD;

public class ClientButtonScript : MonoBehaviour
{
    private bool sceneNotChanged = true;
    private Button clientButton;
    public TextMeshProUGUI notificationText;
    private bool phaseAlreadyCalled = false;
    private MyNetworkManager networkManager;
    int lastKnownPhase = 0;
    // Start is called before the first frame update
    void Start()
    {
        //networkManager = GameObject.Find("/Network").GetComponent<MyNetworkManager>();
        networkManager = MyNetworkManager.getInstance();

      //  clientButton = GetComponent<Button>();
      //  clientButton.onClick.AddListener(ChangeSceneToClient);
    }


    public void ChangeSceneToClient()
    {
        Debug.Log("Hello clientbutton");
        notificationText.text = "Init Client ...";

        if (GameObject.FindWithTag("ServerButton") != null)
        {
            Debug.Log("Server Button found and set false.");
            GameObject.FindWithTag("ServerButton").SetActive(false);
        }
        if (networkManager == null)
        {
            Debug.Log("Networkmanager is null.");
            notificationText.text = "Networkmanager null";
        }
        TextMeshProUGUI networkAdressText = GameObject.Find("AdressInput_Text").GetComponent<TextMeshProUGUI>();
        Debug.Log("address text: " + networkAdressText.text);
       
        networkManager.InitClient(networkAdressText.text);


        // Debug.Log("Address: " + networkManager.GetClientNetworkAdress());

        //  Debug.Log("Is disconnect: " + disconnected);
     

    //    bool waiting = networkManager.GetUDPChat().ConnectAsClient(notificationText.text);
        //SceneManager.LoadScene("ClientScene");
        
        notificationText.text = "Client connects to server ...";
        networkManager.GetUDPChat().StartListening();
         


        /*   if (GameObject.FindWithTag("ServerButton") != null)
               GameObject.FindWithTag("ServerButton").SetActive(true);
               */


    }
    
    private void Update()
    {
        if(null != networkManager && networkManager.initializationComplete == true)
        {
            if(null != networkManager.GetUDPChat())
            {
                Debug.Log("client phase: " + lastKnownPhase + "-" + networkManager.GetUDPChat().GetCurrentPhase());
                if (networkManager.GetUDPChat().GetCurrentPhase() == 0)
                {
                    Debug.Log("client sending 1");
                    MyMessage msg = new MyMessage(1);
                    networkManager.GetUDPChat().Send(msg);
                    Debug.Log("Sleep for 1/2 second.");
                    System.Threading.Thread.Sleep(500);
                }
                else if (lastKnownPhase == 0 && networkManager.GetUDPChat().GetCurrentPhase() == 1)
                {
                    Debug.Log("phase 0 - 1");
                    notificationText.text = "Could connect to server ...";
                    //  networkManager.GetUDPChat().StartListening();
                    lastKnownPhase = 1;
                    notificationText.text = "Notificate server ...";
                }
                else if (lastKnownPhase == 1 && networkManager.GetUDPChat().GetCurrentPhase() == 1)
                {
                    lastKnownPhase = 2;
                    //MyMessage msg = new MyMessage(2);
                    //networkManager.GetUDPChat().Send(msg);
                    networkManager.ChangeScene();
                }
                else if (lastKnownPhase == 2)
                {
                    Destroy(this);
                }
            /*    else
                {
                    Debug.Log("Something went wrong here ...");
                }*/
            }
        }
    }


}
