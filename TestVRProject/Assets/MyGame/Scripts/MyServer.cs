using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Mirror;


public class MyServer : MonoBehaviour
{
    public bool isAtStartup = true;
    bool isNetworkActive=false;
    [FormerlySerializedAs("m_MaxConnections")] public int maxConnections = 2;


    [FormerlySerializedAs("m_NetworkAddress")] public string networkAddress = "localhost";

    public static MyServer singleton;
    /*
        // Start is called before the first frame update
        void Update()
        {
            if(Input.GetKey(KeyCode.Space))
                StopServer();
        }
    */
    public virtual void ConfigureServerFrameRate()
    {
        // set a fixed tick rate instead of updating as often as possible
        // * if not in Editor (it doesn't work in the Editor)
        // * if not in Host mode
#if !UNITY_EDITOR
            if (!NetworkClient.active && isHeadless)
            {
                Application.targetFrameRate = serverTickRate;
                Debug.Log("Server Tick Rate set to: " + Application.targetFrameRate + " Hz.");
            }
#endif
    }
    /*
      // Update is called once per frame
       void Start()
       {     
               InitServer();

       }
    */


    public void InitServer()
    {
        Debug.Log("Server starts to listen ...");
        NetworkServer.Listen(maxConnections);
        isAtStartup = false;

        Debug.Log("Registering Server Handlers ...");
        // this must be after Listen(), since that registers the default message handlers
        NetworkServer.RegisterHandler<ConnectMessage>(OnServerConnectInternal);
        NetworkServer.RegisterHandler<DisconnectMessage>(OnServerDisconnectInternal);
        NetworkServer.RegisterHandler<ReadyMessage>(OnServerReadyMessageInternal);
        NetworkServer.RegisterHandler<AddPlayerMessage>(OnServerAddPlayer);
        NetworkServer.RegisterHandler<RemovePlayerMessage>(OnServerRemovePlayerMessageInternal);
        NetworkServer.RegisterHandler<ErrorMessage>(OnServerErrorInternal);
        NetworkServer.RegisterHandler<MyMessage>(OnServerMyMessage);

        isNetworkActive = true;

        ConfigureServerFrameRate();
        if (!NetworkServer.Listen(maxConnections))
            {
                Debug.LogError("StartServer listen failed.");
                return;
            }
        Debug.Log("Server is active now ...");
    }

    #region Server Internal Message Handlers
    void OnServerConnectInternal(NetworkConnection conn, ConnectMessage connectMsg)
    {
        Debug.Log("NetworkManager.OnServerConnectInternal");
       /*
        if (networkSceneName != "" && networkSceneName != offlineScene)
        {
            SceneMessage msg = new SceneMessage() { sceneName = networkSceneName };
            conn.Send(msg);
        }
        */
    }

    void OnServerDisconnectInternal(NetworkConnection conn, DisconnectMessage msg)
    {
        Debug.Log("NetworkManager.OnServerDisconnectInternal");
    }

    void OnServerReadyMessageInternal(NetworkConnection conn, ReadyMessage msg)
    {
        Debug.Log("NetworkManager.OnServerReadyMessageInternal");
    }

    public virtual void OnServerAddPlayer(NetworkConnection conn, AddPlayerMessage extraMessage)
    {
        Debug.Log("NetworkManager.OnServerAddPlayer");

        /*
        if (playerPrefab == null)
        {
            Debug.LogError("The PlayerPrefab is empty on the NetworkManager. Please setup a PlayerPrefab object.");
            return;
        }
        

        if (playerPrefab.GetComponent<NetworkIdentity>() == null)
        {
            Debug.LogError("The PlayerPrefab does not have a NetworkIdentity. Please add a NetworkIdentity to the player prefab.");
            return;
        }
        */
        if (conn.playerController != null)
        {
            Debug.LogError("There is already a player for this connections.");
            return;
        }
        /*
        Transform startPos = GetStartPosition();
        GameObject player = startPos != null
            ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
            : Instantiate(playerPrefab);

        NetworkServer.AddPlayerForConnection(conn, player);
        */
        NetworkServer.AddConnection(conn);
    }

    void OnServerRemovePlayerMessageInternal(NetworkConnection conn, RemovePlayerMessage msg)
    {
        Debug.Log("NetworkManager.OnServerRemovePlayerMessageInternal");
    }

    void OnServerErrorInternal(NetworkConnection conn, ErrorMessage msg)
    {
        Debug.Log("NetworkManager.OnServerErrorInternal");
    }

    void OnServerMyMessage(NetworkConnection conn, MyMessage msg)
    {
        Debug.Log("Server got a my message: " + msg.text);
        GameObject.Find("Cartoon_SportCar_B01").GetComponent<CarController>().setInput(msg.steeringInput, msg.motorInput, msg.breakInput);
    }
    
    #endregion


    public void StopServer()
    {
        if (!NetworkServer.active)
            return;

        Debug.Log("NetworkManager StopServer");
        isNetworkActive = false;
        NetworkServer.Shutdown();
      
    }

}
