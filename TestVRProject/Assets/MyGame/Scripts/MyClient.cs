﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Mirror;
public class MyClient : MonoBehaviour
{
    public bool isAtStartup = true;
    bool isNetworkActive=false;
    NetworkClient theClient => NetworkClient.singleton;
    // transport layer
    [SerializeField] protected Transport transport;
    [FormerlySerializedAs("m_NetworkAddress")] public string networkAddress = "localhost";

   /* // Start is called before the first frame update
    void Start()
    {
        InitClient();
    }
    */
    /*
    // Update is called once per frame
    void Update()
    {
        

    }*/
    
    public void InitClient()
    {   
       // Transport.activeTransport = transport;
        isNetworkActive = true;

        // RegisterClientMessages();
        NetworkClient.RegisterHandler<ConnectMessage>(OnClientConnectInternal);
        NetworkClient.RegisterHandler<DisconnectMessage>(OnClientDisconnectInternal);
        NetworkClient.RegisterHandler<NotReadyMessage>(OnClientNotReadyMessageInternal);
        NetworkClient.RegisterHandler<ErrorMessage>(OnClientErrorInternal);
        NetworkClient.RegisterHandler<SceneMessage>(OnClientSceneInternal);
        NetworkClient.RegisterHandler<MyMessage>(OnClientMyMessage);

        if (string.IsNullOrEmpty(networkAddress))
        {
            Debug.LogError("Must set the Network Address field in the manager");
            return;
        }
        if (LogFilter.Debug) Debug.Log("NetworkManager StartClient address:" + networkAddress);

        NetworkClient.Connect(networkAddress);

        isAtStartup = false;
    }


    #region Client Internal Message Handlers
    void OnClientConnectInternal(NetworkConnection conn, ConnectMessage message)
    {
        Debug.Log("NetworkManager.OnClientConnectInternal");

       
    }

    void OnClientDisconnectInternal(NetworkConnection conn, DisconnectMessage msg)
    {
        Debug.Log("NetworkManager.OnClientDisconnectInternal");
    }

    void OnClientNotReadyMessageInternal(NetworkConnection conn, NotReadyMessage msg)
    {
        Debug.Log("NetworkManager.OnClientNotReadyMessageInternal");
        // NOTE: clientReadyConnection is not set here! don't want OnClientConnect to be invoked again after scene changes.
    }

    void OnClientErrorInternal(NetworkConnection conn, ErrorMessage msg)
    {
        Debug.Log("NetworkManager:OnClientErrorInternal");
    }

    void OnClientSceneInternal(NetworkConnection conn, SceneMessage msg)
    {
        Debug.Log("NetworkManager.OnClientSceneInternal");

        if (NetworkClient.isConnected)
        {
           // ClientChangeScene(msg.sceneName, true, msg.sceneMode, msg.physicsMode);
        }
    }

    void OnClientMyMessage(NetworkConnection conn, MyMessage msg)
    {
        Debug.Log("Server gives me a mymessage!");

       
    }
    
    #endregion

    public void Send(MyMessage msg) {
        NetworkClient.Send(msg);
    }


    public void StopClient()
    {
        Debug.Log("NetworkManager StopClient");
        isNetworkActive = false;

        // shutdown client
        NetworkClient.Disconnect();
        NetworkClient.Shutdown();
    }


}