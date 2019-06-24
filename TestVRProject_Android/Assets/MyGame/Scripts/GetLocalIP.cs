using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using TMPro;
using System;
using UnityEngine.UI;


public class GetLocalIP : MonoBehaviour
{
    public TextMeshProUGUI targetInputField;
    public InputField inputField_Text;    // Start is called before the first frame update

    void Start()
    {
        CreateBerkeleySocket();
    
    }



        void CreateBerkeleySocket()
    {     
        //https://stackoverflow.com/questions/6803073/get-local-ip-address
        string localIP;
        using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
        {
            socket.Connect("8.8.8.8", 65530);
            IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
            localIP = endPoint.Address.ToString();
        }
        if (String.IsNullOrEmpty(localIP))
            localIP = "???";

        targetInputField.text =  localIP;
        inputField_Text.text = localIP; //<- because of Unity bug
      //  Debug.Log("Local IP: " + localIP);

    }

    // Update is called once per frame
    void Update()
    {
        if (String.IsNullOrEmpty(targetInputField.text))
        {
            CreateBerkeleySocket();
        }
    }
}
