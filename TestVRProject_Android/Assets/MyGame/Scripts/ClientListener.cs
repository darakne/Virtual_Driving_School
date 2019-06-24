using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ClientListener : MonoBehaviour
{
    MyNetworkManager networkManager;
    // Start is called before the first frame update


    void Start()
    {
        networkManager = GameObject.Find("Network").GetComponent<MyNetworkManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (networkManager.IsClientDisconnected())
        {
            Debug.Log("Client disconnected?: '" + networkManager.GetClientNetworkAdress() + "'");
            // Debug.Log("Address2 is: '" + networkManager.initAdressTextField + "'");

            networkManager.InitClient(networkManager.GetClientNetworkAdress());
            if (networkManager.IsClientDisconnected())
                Debug.Log("Client init 2 did not work");



            //   ClientDisconnected();
            return;
        }


        MyMessage msg = new MyMessage();
        // GameObject.Find("Cartoon_SportCar_B01").GetComponent<CarController>().setInput(Input.acceleration.x, Input.acceleration.z, false);
        msg.steeringInput = Input.acceleration.x; // Input.GetAxis("Horizontal");
        msg.motorInput = Input.acceleration.z*-1.0f; // Input.GetAxis("Vertical");
        msg.text = "client message";

        if (msg.steeringInput > 0 || msg.motorInput > 0) { 
            // Debug.Log("Sending message now from client");
            networkManager.SendMessageToServer(msg);
         }
    
        MyMessage msg1 = new MyMessage();
        // GameObject.Find("Cartoon_SportCar_B01").GetComponent<CarController>().setInput(Input.acceleration.x, Input.acceleration.z, false);
        msg1.steeringInput = Input.GetAxis("Horizontal");
        msg1.motorInput = Input.GetAxis("Vertical");
        msg1.text = "client message 2";

        if (msg1.steeringInput > 0 || msg1.motorInput > 0) { 
            // Debug.Log("Sending message now from client2");
            networkManager.SendMessageToServer(msg1);
            }
            



}

void ClientDisconnected()
    {

        //close client
        networkManager.ShutdownClient();
        //destroy network object since a new one is created in the startscene
        Destroy(GameObject.Find("Network"));
        SceneManager.LoadScene("StartScene");
    }
}
