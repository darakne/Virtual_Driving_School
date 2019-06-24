// vis2k: GUILayout instead of spacey += ...; removed Update hotkeys to avoid
// confusion if someone accidentally presses one.
using System.ComponentModel;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;
using Mirror;


/* The Frankenstein-Monster-Mix of NetworkManager and NetworkManagerHUD */
    public class MyNetworkManager : MonoBehaviour
    {
        MyServer theserver;
        MyClient theclient;
        public string initAdressTextField = "localhost";
        public bool showGUI = true;
        public int offsetX;
        public int offsetY;
        // transport layer
        [SerializeField] protected Transport transport;
        [FormerlySerializedAs("m_DontDestroyOnLoad")] public bool dontDestroyOnLoad = true;
        [Scene]
        [FormerlySerializedAs("ClientScene")] public string clientScene = "";
        static UnityEngine.AsyncOperation loadingSceneAsync;

        MyNetworkManager singleton;

        void Awake()
        {
            InitializeSingleton();
        }


        private void InitializeSingleton()
        {
            if (singleton != null && singleton == this)
            {
                return;
            }

            if (dontDestroyOnLoad)
            {
                if (singleton != null)
                {
                    Debug.LogWarning("Multiple NetworkManagers detected in the scene. Only one NetworkManager can exist at a time. The duplicate NetworkManager will be destroyed.");
                    Destroy(gameObject);
                    return;
                }
                if (LogFilter.Debug) Debug.Log("NetworkManager created singleton (DontDestroyOnLoad)");
                singleton = this;
                if (Application.isPlaying) DontDestroyOnLoad(gameObject);
            }
            else
            {
                if (LogFilter.Debug) Debug.Log("NetworkManager created singleton (ForScene)");
                singleton = this;
            }

            // set active transport AFTER setting singleton.
            // so only if we didn't destroy ourselves.
            Transport.activeTransport = transport;
        }

    public Transport GetActiveTransport()
    {
        return Transport.activeTransport;
    }

    public void InitServer(string networkAdress)
    {
        if (!NetworkServer.active)
        {
            initAdressTextField = networkAdress;
             theserver = new MyServer();
            theserver.InitServer(networkAdress);
             
        }
    }

    public int CountServerConnections()
    {
        return NetworkServer.connections.Count;
    }

    public string GetServerNetworkAdress()
    {
        return theserver.networkAddress;
    }

    public void ShutdownServer()
    {
        if (NetworkServer.active)
        {
                theserver.StopServer();
        }
    }

    public void InitClient(string networkAdress)
    {
        if (!NetworkClient.isConnected && !NetworkClient.active)
        {
            initAdressTextField = networkAdress;
            theclient = new MyClient();
            theclient.InitClient(networkAdress);

        }

        else Debug.Log("Network is already active. No init client.");
    }

    

    public bool IsClientDisconnected()
    {
        return !theclient.ClientConnectedOnce();
    }

    public void ShutdownClient()
    {
        theclient.StopClient();
    }

    public string GetClientNetworkAdress()
    {
       return theclient.networkAddress;
    }

    /*
        void OnGUI()
        {
            if (!showGUI)
                return;

            GUILayout.BeginArea(new Rect(10 + offsetX, 40 + offsetY, 215, 9999));
            if (!NetworkClient.isConnected && !NetworkServer.active)
            {
                if (!NetworkClient.active)
                {
                    /*
                    // LAN Host
                    if (Application.platform != RuntimePlatform.WebGLPlayer)
                    {
                        if (GUILayout.Button("LAN Host"))
                        {
                            manager.StartHost();
                        }
                    }
                    * /
                    // LAN Client + IP
                    GUILayout.BeginHorizontal();
                    initAdressTextField = GUILayout.TextField(initAdressTextField);
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("LAN Client"))
                    {
                        theclient = new MyClient();
                        theclient.InitClient();
                        theclient.networkAddress = initAdressTextField;
                    }                   
                    

                    // LAN Server Only
                    if (Application.platform == RuntimePlatform.WebGLPlayer)
                    {
                        // cant be a server in webgl build
                        GUILayout.Box("(  WebGL cannot be server  )");
                    }
                    else
                    {
                        if (GUILayout.Button("LAN Server Only"))
                        {
                            theserver = new MyServer();
                            theserver.InitServer();
                            theserver.networkAddress = initAdressTextField;
                        }
                    }
                    GUILayout.EndHorizontal();
                }
                else
                {
                    // Connecting
                    GUILayout.Label("Connecting to " + theclient.networkAddress + "..");
                    if (GUILayout.Button("Cancel Connection Attempt"))
                    {
                        theclient.StopClient();
                    }
                }
            }
            else
            {
                // server / client status message
                if (NetworkServer.active)
                {
                    GUILayout.Label("Server: active. Transport: " + Transport.activeTransport);                  
                }
                if (NetworkClient.isConnected)
                {
                    GUILayout.Label("Client: address=" + theclient.networkAddress);
                }
            }

            // client ready
            if (NetworkClient.isConnected && !ClientScene.ready)
            {
                if (GUILayout.Button("Client Ready"))
                {
                    ClientScene.Ready(NetworkClient.connection);

                    /*if (ClientScene.localPlayer == null)
                    {
                    //    ClientScene.AddPlayer();
                    }
                   * /

                    ClientChangeScene(LoadSceneMode.Single, LocalPhysicsMode.None);

                }
            }

            // stop
            if (NetworkServer.active) {
                GUILayout.Label("Connections: " + NetworkServer.connections.Count);
                if (GUILayout.Button("Stop"))
                {
                    theserver.StopServer();
                }
            }
            else if (NetworkClient.isConnected)  {
                if (GUILayout.Button("Stop"))
                {
                    theclient.StopClient();
                }
            }

            GUILayout.EndArea();
        }
    */

        public virtual void ClientChangeScene(LoadSceneMode sceneMode, LocalPhysicsMode physicsMode)
        {
            if (string.IsNullOrEmpty(clientScene))
            {
                Debug.LogError("ServerChangeScene empty scene name");
                return;
            }

           Debug.Log("Client changes scene " + clientScene);


            LoadSceneParameters loadSceneParameters = new LoadSceneParameters(sceneMode, physicsMode);

            loadingSceneAsync = SceneManager.LoadSceneAsync(clientScene, loadSceneParameters);

            SceneMessage msg = new SceneMessage()
            {
                sceneName = clientScene,
                sceneMode = loadSceneParameters.loadSceneMode,
                physicsMode = loadSceneParameters.localPhysicsMode
            };

        }


    public void SendMessageToServer(MyMessage msg)
    {
        Debug.Log("Client sends message to server ...");
        if (NetworkClient.isConnected && ClientScene.ready)
            Debug.Log("client sends now ...");
        theclient.Send(msg);
        
    }
     
    }

