using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//from tutorial: https://www.gamasutra.com/blogs/VivekTank/20180706/321374/Unity_Wheel_Collider_for_Motor_vehicle_Tutorial_2018.php
[System.Serializable]
public class WheelPair
{
    public WheelCollider colliderLeftWheel, colliderRightWheel; 
    public Transform transformLeftWheel, transformRightWheel;
    public bool hasMotor; //the wheel is connected with the motor
    public bool canSteer; //something frontwheels can
}

    //from here https://www.youtube.com/watch?v=j6_SMdWeGFI&feature=youtu.be&t=1384
    //https://github.com/coderDarren/Unity3D-Cars/tree/master/Cars/Assets/Scripts
    public class CarController : MonoBehaviour
{
    private float steeringInput;    
    private float motorInput;
    private float breakInput;

    public List<WheelPair> wheelPairs;
    public Transform steeringWheel;
    public float steeringWheelTurningSpeed = 100;
    public float maxSteeringWheelAngle = 270;


    public float maxSteerAngle = 30; //rotation of the front wheels to left/right
    public float motorForce = 500;
    public float brakeForce = 1e+5f;
    public float decelerationForce = 1e+5f;

    
    public void Start()
    {
    
    }
    
    public void GetInput()
    {
        steeringInput = Input.GetAxis("Horizontal");
        motorInput = Input.GetAxis("Vertical");
        
        if (Input.GetKey(KeyCode.Space)) breakInput = brakeForce;
        else breakInput = 0f;
    }


    private void Steer(WheelPair wheelPair)
    {
       float steeringAngle = maxSteerAngle * steeringInput;
       wheelPair.colliderLeftWheel.steerAngle = steeringAngle;
       wheelPair.colliderRightWheel.steerAngle = steeringAngle;
  
       
    }

    //let the motor force do its stuff
    private void Accelerate(WheelPair wheelPair)
    {
        float currentMotorForce = motorForce * motorInput;
        wheelPair.colliderLeftWheel.motorTorque = currentMotorForce;
        wheelPair.colliderRightWheel.motorTorque = currentMotorForce;
    }
    //at the moment a fixed value .... later with terrain value or something else?
    private void Decelerate(WheelPair wheelPair)
    {
        wheelPair.colliderLeftWheel.brakeTorque = decelerationForce;
        wheelPair.colliderRightWheel.brakeTorque = decelerationForce;
    }

        //use brake
        private void Brake(WheelPair wheelPair)
    {
        wheelPair.colliderLeftWheel.brakeTorque = breakInput;
        wheelPair.colliderRightWheel.brakeTorque = breakInput;
    }


    //rotate the wheel while the car is moving (animation)
    private void UpdateWheelPairPose(WheelPair wheelPair)
    {
        Vector3 lPos = wheelPair.transformLeftWheel.position; 
        Vector3 rPos = wheelPair.transformRightWheel.position;

        Quaternion lQuat = wheelPair.transformLeftWheel.rotation;
        Quaternion rQuat = wheelPair.transformRightWheel.rotation;

        wheelPair.colliderLeftWheel.GetWorldPose(out lPos, out lQuat);
        wheelPair.colliderRightWheel.GetWorldPose(out rPos, out rQuat);

        wheelPair.transformLeftWheel.position = lPos;
        wheelPair.transformRightWheel.position = rPos;

        wheelPair.transformLeftWheel.rotation = lQuat;
        wheelPair.transformRightWheel.rotation = rQuat;
    }

    //note steering wheel is still rotating around ...

    private void UpdateSteeringWheel()
    {
        //rotate the steering wheel
       // Debug.Log("steering input: " + steeringInput);

        float angle = steeringInput* steeringWheelTurningSpeed + steeringWheel.localRotation.y;
        //"cut" the rotation between 2 values (+-maxSteeringWheelAngle)
        float angle2 = Mathf.Clamp(angle, -maxSteeringWheelAngle, maxSteeringWheelAngle);
        //Debug.Log("angle2: " + angle2);
        //got this by playing around
         steeringWheel.localRotation = Quaternion.Euler(angle2 +90,0f,  -90f);
     
    }


    //https://docs.unity3d.com/ScriptReference/Gyroscope.html
    // The Gyroscope is right-handed.  Unity is left handed.
    // Make the necessary change to the object.


    /* 
     * Android: Gravity, Linear Acceleration, Rotation Vector. More information.
       iOS: Gyroscope, Device-Motion. More information.
     */
    void GyroSensorInput()
    {
        if (Input.gyro.enabled) {
            //best option? https://www.youtube.com/watch?v=0Dqazn653v8
            Vector3 rotationRate = Input.gyro.rotationRate;

            //other options
            Vector3 a = Input.gyro.userAcceleration; //acceleration to match the side-to-side acceleration of the smartphone
            Vector3 b = Input.gyro.gravity;
            Quaternion q = Input.gyro.attitude;

            //careful:
            Quaternion quaternionForUnity = new Quaternion(q.x, q.y, -q.z, -q.w);
            transform.rotation = quaternionForUnity;
        }
    }
   



    private void FixedUpdate()
    {
        GetInput();
        
        UpdateSteeringWheel();

        for (int i = 0, len = wheelPairs.Count; i < len; ++i) { 
            if(wheelPairs[i].canSteer) Steer(wheelPairs[i]);
            if (wheelPairs[i].hasMotor) Accelerate(wheelPairs[i]);
            Decelerate(wheelPairs[i]);
            Brake(wheelPairs[i]);

            UpdateWheelPairPose(wheelPairs[i]);
        }
    }
}
