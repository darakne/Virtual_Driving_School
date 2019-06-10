using UnityEngine;
using Mirror;

//documentation from Unity: https://docs.unity3d.com/Manual/class-NetworkBehaviour.html
//https://vis2k.github.io/Mirror/Classes/NetworkBehavior.html
public class ClientControllerInput : NetworkBehaviour
{
    public GameObject car;

    public float steeringInput;
    public float motorInput;
    public bool breakInput;
    public uint networkInstanceID = 1;

    /* OnStartClient called when the GameObject spawns on the client, 
     * or when the client connects to a server for GameObjects in the Scene
     */
    public override void OnStartClient()
    {
        networkInstanceID = (uint)Random.Range(0, 10);
        Debug.Log("Client " + networkInstanceID + " calls OnStartClient()");
        if (this.isLocalPlayer) Debug.Log("It is NOT a local player.");
        // register client events, enable effects
    }

    /* 
     * OnStartLocalPlayer called on clients for player GameObjects on the local client (only)
     */
    public override void OnStartLocalPlayer()
    {
        Debug.Log("Client " + networkInstanceID + " calls OnStartLocalPlayer()");
        // register client events, enable effects
        if (this.isLocalPlayer) Debug.Log("It is a local player.");
    }
    


    [ClientCallback] // This code executes on the client, gathering input
    void FixedUpdate()
    {
        Debug.Log("Getting Input on Client Side: ");
        Debug.Log("gyrosensor supported: " +SystemInfo.supportsGyroscope);
        Debug.Log("Input acceleration: " + Input.acceleration);

        steeringInput = Input.GetAxis("Horizontal");
        motorInput = Input.GetAxis("Vertical");

        if (Input.GetAxis("Jump") > 0.0f) breakInput = true;
        else breakInput = false;

        Debug.Log("steeringInput: " + steeringInput);
        Debug.Log("motorInput: " + motorInput);
        Debug.Log("breakInput: " + breakInput);

        // This line triggers the code to run on the server
        CmdThrust(steeringInput, motorInput, breakInput);
    }


    [Command] // This code executes on the server after Update() is called from below.
    public void CmdThrust(float steeringInput, float motorInput, bool breakInput)
    {
        Debug.Log("Getting Input on Server Side: ");
        // This code executes on the server after FixedUpdate() is

        // called from below.
        //this.steeringInput = steeringInput;
        //  this.motorInput = motorInput;
        //  this.breakInput = breakInput;
        // CarController c = GameObject.Find("Cartoon_SportCar_B01").GetComponent<CarController>();
        car.GetComponent<CarController>().setInput(steeringInput, motorInput, breakInput);
    }



}
 
 