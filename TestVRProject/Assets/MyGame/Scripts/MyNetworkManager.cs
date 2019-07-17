// vis2k: GUILayout instead of spacey += ...; removed Update hotkeys to avoid
// confusion if someone accidentally presses one.
using System.ComponentModel;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections;
using HD;
using UnityEngine.SceneManagement;

/* The Frankenstein-Monster-Mix of NetworkManager and NetworkManagerHUD */
public class MyNetworkManager : MonoBehaviour
    {
        UDPChat udpchat;
        public bool initializationComplete = false;
        public string initAdressTextField;
        public int port = 7777;

  // transport layer
  //[FormerlySerializedAs("m_DontDestroyOnLoad")] public bool dontDestroyOnLoad = true;
    //    static UnityEngine.AsyncOperation loadingSceneAsync;

        static MyNetworkManager singleton;

        public CarController carController; //the CarController sets itself when loading the server specific scene

    public UDPChat GetUDPChat()
    {
        return udpchat;
    }


    void Awake()
        {
            InitializeSingleton();
        }

    public static MyNetworkManager getInstance()
    {
        return singleton;
    }
        
     

    private void InitializeSingleton()
    {
            if (singleton != null && singleton == this)  return;
            Debug.Log("NetworkManager created singleton (ForScene)");
            singleton = this;  
    }

    private void Start()
    {
        Scene loadedLevel = SceneManager.GetSceneByName("StartScene");
        if (loadedLevel.isLoaded)
        {
            SceneManager.SetActiveScene(loadedLevel);
            return;
        }
        Debug.Log("Loading Additive Scene");
        StartCoroutine(LoadSceneAsync("StartScene", true));
    }


    //https://catlikecoding.com/unity/tutorials/object-management/multiple-scenes/
    IEnumerator LoadScene(string sceneName, bool isSceneAdditive)
    {
            if (isSceneAdditive)
                 SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            else
                SceneManager.LoadScene(sceneName);
            yield return null;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
    }

    IEnumerator LoadSceneAsync(string sceneName, bool isSceneAdditive)
    {
        if (isSceneAdditive)
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        else
            yield return SceneManager.LoadSceneAsync(sceneName);

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
    }

    IEnumerator UnloadScene(string sceneName)
    {
        yield return SceneManager.UnloadSceneAsync(sceneName);
    }


public void InitServer()
    {
        if (null != udpchat) return ;

        //initAdressTextField = networkAdress;
        // udpchat.targetAddress = networkAdress;
        udpchat = new UDPChat(true);
        udpchat.port = port;
        udpchat.Init();
        initializationComplete = true;
    }

   
    public void InitClient(string networkAdress)
    {
        if (null != udpchat) return;

        initAdressTextField = networkAdress;
        udpchat = new UDPChat(false);
        udpchat.targetAddress = networkAdress;
        udpchat.port = port;
        udpchat.Init();
        //  udpchat.StartListening();
        initializationComplete = true;
    }


    public void Shutdown()
    {
        if( null != udpchat)
            udpchat.OnApplicationQuit();
    }


    private void OnApplicationQuit()
    {
        Shutdown();
    }


    //tell the server, that it can find the CarController now
    public void ChangeScene()
    {
        Debug.Log("Changing scene ...");
        StartCoroutine(UnloadScene("StartScene"));
        if (udpchat.IsServer()) {
            StartCoroutine(LoadScene("SampleScene", true));

            Debug.Log("scene changed");
  
         /*   Scene s = SceneManager.GetActiveScene();
            GameObject[] gameObjects = s.GetRootGameObjects();
            foreach (GameObject g in gameObjects)
            {
                g.get
            }
            GameObject car = GameObject.FindWithTag("Player");*/
         //GameObject car1 = Instantiate(car) as GameObject;
         // car1.transform.position = new Vector3(30, 2, 105);

        }
        else
        {
            StartCoroutine(LoadScene("ClientScene", true));
        }
        
    }


    private void Update()
    {
        if (null != udpchat && udpchat.GetCurrentPhase() > 0) {
            if (udpchat.IsServer())
            {
                Debug.Log("I am server");
                if (null != carController) {
                    Debug.Log("car controller");
                    MyMessage lastMessage = udpchat.GetLastMessage();
                    if(null != lastMessage)
                    {
                        Debug.Log("lastMessage.SteeringInput: " + lastMessage.SteeringInput);
                        Debug.Log("lastMessage.MotorInput " + lastMessage.MotorInput);
                        carController.SetInput(lastMessage.SteeringInput, lastMessage.MotorInput, lastMessage.BreakInput);
                    }
                }
            }
            else
            {
                Debug.Log("lastMessage.SteeringInput: " + Input.GetAxis("Horizontal"));
                Debug.Log("lastMessage.MotorInput " + Input.GetAxis("Vertical"));
             //   MyMessage msg = new MyMessage(2, Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), false);
                MyMessage msg = new MyMessage(2, Input.acceleration.x, -Input.acceleration.z, false);
                udpchat.Send(msg);
                Debug.Log("Sending client input message.");
            }
        }
    }

}

