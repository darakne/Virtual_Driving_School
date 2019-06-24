using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class ClientButtonScript : MonoBehaviour
{
    private Button clientButton;
    public TextMeshProUGUI notificationText;
    MyNetworkManager networkManager;
    // Start is called before the first frame update
    void Start()
    {
        networkManager = GameObject.Find("Network").GetComponent<MyNetworkManager>();

        clientButton = GetComponent<Button>();
        clientButton.onClick.AddListener(ChangeSceneToClient);
    }


    void ChangeSceneToClient()
    {
        Debug.Log("Hello clientbutton");

        GameObject.FindWithTag("ServerButton").SetActive(false);

        TextMeshProUGUI networkAdressText = GameObject.Find("AdressInput_Text").GetComponent<TextMeshProUGUI>();
        Debug.Log("address text: " + networkAdressText.text);

        networkManager.InitClient(networkAdressText.text);
        // Debug.Log("Address: " + networkManager.GetClientNetworkAdress());

        /* //  Debug.Log("Is disconnect: " + disconnected);
         if (!networkManager.IsClientDisconnected())
             SceneManager.LoadScene("ClientScene");
         else {
             notificationText.text = "Could not connect to: " + networkManager.GetClientNetworkAdress();

          /*   if (GameObject.FindWithTag("ServerButton") != null)
                 GameObject.FindWithTag("ServerButton").SetActive(true);
                 * /
         }*/

        SceneManager.LoadScene("ClientScene");
    }

}
