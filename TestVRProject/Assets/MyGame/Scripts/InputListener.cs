using UnityEngine;

public class InputListener : MonoBehaviour
{
    private float steeringInput;
    private float motorInput;
    private bool breakInput;

    public void Start()
    {

    }

    public void GetInput()
    {
        steeringInput = Input.GetAxis("Horizontal");
        motorInput = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.Space)) breakInput = true;
        else breakInput = false;

        print("Controller steering input: " + steeringInput);
        print("Controller motorInput input: " + motorInput);
        print("Controller breakInput input: " + breakInput);
    }

    private void FixedUpdate()
    {
        GetInput();
    }
}
