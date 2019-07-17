using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
         
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GameObject.Find("Cartoon_SportCar_B01").GetComponent<CarController>().SetInput(Input.acceleration.x, Input.acceleration.z, false);
    }
}
