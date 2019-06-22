using UnityEngine;

//source: https://www.youtube.com/watch?v=P5JxTfCAOXo
public class GyroControl : MonoBehaviour
{
    //Singleton definition
    #region Instance
	public static GyroControl instance;
    public static GyroControl Instance
    {
        get
        {
            if(null == instance)
            {
                instance = FindObjectOfType<GyroControl>();
                if(null == instance)
                {
                    instance = new GameObject("Spawned GyroControl", typeof(GyroControl)).GetComponent<GyroControl>();
                }
            }
            return instance;
        }
        set
        {
            instance = value;
        }
    }
    #endregion
    private Gyroscope gyro;
    private bool gyroActive;
    private Quaternion rotation;
    public Camera cam;


    private void Start()
	{
		//cameraContainer = new GameObject("Camera Container");
		//cameraContainer.transform.position = transform.position;
		//transform.SetParent(cameraContainer.transform);
        SetGyro();
	}

	private bool SetGyro() 
	{
		// Already activated
		if (gyroActive) { return true; }

		// check if phone supports gyroscope
		if (SystemInfo.supportsGyroscope)
		{
			gyro = Input.gyro;
			gyro.enabled = true;
            //gyroActive = gyro.enabled;

            cam.transform.rotation = Quaternion.Euler(90f, 90f, 0f);
			rotation = new Quaternion(0,0,1,0);
			gyroActive=true;
        }
		else gyroActive=false;

        Debug.Log("Gyrosenor is: " + gyroActive);

		return gyroActive;
	}

	private void Update()
	{
		if (gyroActive)
		{
			transform.localRotation = gyro.attitude  * rotation;
		}
	}

	public Quaternion GetGyroRotation() {
		return rotation;
	}
}
