using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NotificationScript : MonoBehaviour
{
    MyNetworkManager networkManager;
    public TextMeshProUGUI notificationText;
    // Start is called before the first frame update
    void Start()
    {
        networkManager = GameObject.Find("Network").GetComponent<MyNetworkManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
