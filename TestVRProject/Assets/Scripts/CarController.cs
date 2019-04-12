using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//from here https://www.youtube.com/watch?v=j6_SMdWeGFI&feature=youtu.be&t=1384
//https://github.com/coderDarren/Unity3D-Cars/tree/master/Cars/Assets/Scripts
public class CarController : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;
    private float steeringAngle;

    public WheelCollider cfrontLeftWheel, cfrontRightWheel;
    public WheelCollider cbackLeftWheel, cbackRightWheel;

    public Transform tfrontLeftWheel, tfrontRightWheel;
    public Transform tbackLeftWheel, tbackRightWheel;

    public Transform steeringWheel;

    public float maxSteerAngle = 30; //rotation of the front wheels to left/right
    public float motorForce = 50;

    //WheelCollider.brakeTorque


    public void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }


    private void Steer()
    {
        steeringAngle = maxSteerAngle * horizontalInput;
        cfrontLeftWheel.steerAngle = steeringAngle;
        cfrontRightWheel.steerAngle = steeringAngle;

  
       steeringWheel.localRotation = Quaternion.Euler(0f, 270f * horizontalInput, 0f);
    }

    private void Accelerate()
    {
        cfrontLeftWheel.motorTorque = motorForce * verticalInput;
        cfrontRightWheel.motorTorque = motorForce * verticalInput;
        cbackLeftWheel.motorTorque = motorForce * verticalInput;
        cbackRightWheel.motorTorque = motorForce * verticalInput;
    }


    //rotate the wheel
    private void UpdateWheelePose( WheelCollider collider, Transform transform)
    {
        Vector3 pos = transform.position;
        Quaternion quat = transform.rotation;

        //set the positon and quaternion
        collider.GetWorldPose(out pos, out quat);

        transform.position = pos;
        transform.rotation = quat;
    }

    private void FixedUpdate()
    {
        GetInput();
        Steer();
        Accelerate();

        UpdateWheelePose(cfrontLeftWheel, tfrontLeftWheel);
        UpdateWheelePose(cfrontRightWheel, tfrontRightWheel);
        UpdateWheelePose(cbackLeftWheel, tbackLeftWheel);
        UpdateWheelePose(cbackRightWheel, tbackRightWheel);
    }
}
