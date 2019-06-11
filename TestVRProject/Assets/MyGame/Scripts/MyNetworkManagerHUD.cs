// vis2k: GUILayout instead of spacey += ...; removed Update hotkeys to avoid
// confusion if someone accidentally presses one.
using System.ComponentModel;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace Mirror
{
    [AddComponentMenu("Network/NetworkManagerHUD")]
    [RequireComponent(typeof(NetworkManager))]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [HelpURL("https://vis2k.github.io/Mirror/Components/NetworkManagerHUD")]
    public class MyNetworkManagerHUD : MonoBehaviour
    {
        NetworkManager manager;
        public bool showGUI = true;
        public int offsetX;
        public int offsetY;

        CrossPlatformInputManager.VirtualAxis m_HVAxis;
        string horizontalAxisName = "Horizontal";
        CrossPlatformInputManager.VirtualAxis m_VVAxis;
        string verticalAxisName = "Vertical";

        void Awake()
        {
            manager = GetComponent<NetworkManager>();
        }

        void OnGUI()
        {
            if (!showGUI)
                return;

            GUILayout.BeginArea(new Rect(10 + offsetX, 40 + offsetY, 215, 9999));
            if (!NetworkClient.isConnected && !NetworkServer.active)
            {
                if (!NetworkClient.active)
                {
                    // LAN Host
                    if (Application.platform != RuntimePlatform.WebGLPlayer)
                    {
                        if (GUILayout.Button("LAN Host"))
                        {
                            manager.StartHost();
                        }
                    }

                    // LAN Client + IP
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("LAN Client"))
                    {
                        manager.StartClient();
                    }
                    manager.networkAddress = GUILayout.TextField(manager.networkAddress);
                    GUILayout.EndHorizontal();

                    // LAN Server Only
                    if (Application.platform == RuntimePlatform.WebGLPlayer)
                    {
                        // cant be a server in webgl build
                        GUILayout.Box("(  WebGL cannot be server  )");
                    }
                    else
                    {
                        if (GUILayout.Button("LAN Server Only")) manager.StartServer();
                    }
                }
                else
                {
                    // Connecting
                    GUILayout.Label("Connecting to " + manager.networkAddress + "..");
                    if (GUILayout.Button("Cancel Connection Attempt"))
                    {
                        manager.StopClient();
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
                    GUILayout.Label("Client: address=" + manager.networkAddress);
                }
            }

            // client ready
            if (NetworkClient.isConnected && !ClientScene.ready)
            {
                if (GUILayout.Button("Client Ready"))
                {
                    ClientScene.Ready(NetworkClient.connection);

                    if (ClientScene.localPlayer == null)
                    {
                        ClientScene.AddPlayer();
                    }
                }
            }

            // stop
            if (NetworkServer.active || NetworkClient.isConnected)
            {
                GUILayout.Label("Connections: " + NetworkServer.connections.Count);
                if (GUILayout.Button("Stop"))
                {
                    manager.StopHost();
                }
            }

            GUILayout.EndArea();
        }

        /*
        // Start is called before the first frame update
        void Start()
        {
            if (NetworkServer.active)
            {
                m_HVAxis = new CrossPlatformInputManager.VirtualAxis(horizontalAxisName);
                CrossPlatformInputManager.RegisterVirtualAxis(m_HVAxis);
                m_VVAxis = new CrossPlatformInputManager.VirtualAxis(verticalAxisName);
                CrossPlatformInputManager.RegisterVirtualAxis(m_VVAxis);
                NetworkServer.Listen(25000);

            NetworkServer.RegisterHandler<MyMessage>(ServerReceiveMessage);
            }
        }

        private void ServerReceiveMessage(NetworkConnection arg1, MyMessage message)
        {
            Debug.Log("Server is receiving message.");
            m_HVAxis.Update(message.steeringInput);
            m_VVAxis.Update(message.motorInput);
        }

        void FixedUpdate()
        {
            if (NetworkClient.isConnected && ClientScene.ready)
            {
                MyMessage msg = new MyMessage();
                msg.steeringInput = Input.GetAxis("Horizontal");
                msg.motorInput = Input.GetAxis("Vertical");

                Debug.Log("Sending message now from client");
                int channelId = 888;
                NetworkClient.Send<MyMessage>(msg, channelId);
            }
        }*/
    }
}
