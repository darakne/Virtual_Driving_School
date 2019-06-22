using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;
public class MenuManager : MonoBehaviour
{
    //  public TextMeshProUGUI input;
    //  public GameObject serverButton;
    //  public GameObject clientButton;
    private Button serverButton;
    // Start is called before the first frame update
    void Start()
    {
        serverButton = GameObject.FindWithTag("ServerButton").GetComponent<Button>();
     //  serverButton.GetMouseButtonDown(0); //.onClick.AddListener(ChangeSceneToServer);
//serverButton.onClick.AddListener(ChangeSceneToServer);
   
    }




    void ChangeSceneToClient()
    {
        Debug.Log("Hello clientbutton");
        GameObject.FindWithTag("ClientButton").SetActive(false);
        SceneManager.LoadScene("ClientScene");
    }

}
