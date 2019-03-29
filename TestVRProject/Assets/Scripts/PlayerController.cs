using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this whole project was made with this tutorial: https://developer.oculus.com/documentation/unity/latest/concepts/unity-tutorial/
//https://docs.unity3d.com/Manual/MobileInput.html
//https://docs.unity3d.com/ScriptReference/Input.GetTouch.html
//https://docs.unity3d.com/ScriptReference/Gyroscope.html
public class PlayerController : MonoBehaviour
{
    Rigidbody rigidBody;
    public float speed = 0;
    public float accelerationRate = 10f;
    public float decelerationRate = 5f;

    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

    }
    // Update is called once per frame
    void Update()
    { 

        // get input data from keyboard or controller
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // update player position based on input
        Vector3 prevPos = this.transform.position;

        
        
        float speedkm = rigidBody.velocity.magnitude;
        //add to the current velocity according while accelerating
        float allVelocity = speedkm + (accelerationRate * Time.deltaTime) - (decelerationRate * Time.deltaTime);
        //subtract from the current velocity while decelerating
        rigidBody.velocity = Vector3.forward * allVelocity * moveVertical;

        // rigidBody.AddForce(new Vector3(0, -jumpHeight * 1.5f, 0), ForceMode.VelocityChange);
        // var vel = rigidBody.velocity;      //to get a Vector3 representation of the velocity
        // to get magnitude

        if (speedkm > 1f) {
            rigidBody.velocity.Normalize();
        }
            Debug.Log("speed is 1: " + speedkm);
    }
    IEnumerator CalcVelocity()
    {
        while (Application.isPlaying)
        {
            // Position at frame start
            Vector3 prevPos = this.transform.position;
            // Wait till the end of the frame
            yield return new WaitForEndOfFrame();
            rigidBody.velocity = (prevPos - this.transform.position) / 3.6f *Time.deltaTime;

            //https://answers.unity.com/questions/873859/how-to-move-an-object-a-specific-speed-in-kmh.html
            // transform.Translate(Vector3.forward * 100 / 3.6 * Time.deltaTime);

            Debug.Log("speed is 2: " + rigidBody.velocity.magnitude);
            

        }
    }

}
