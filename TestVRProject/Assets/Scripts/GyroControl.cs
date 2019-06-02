using UnityEngine;

public class GyroControl : MonoBehaviour
{
	private bool gyroActive;
	private Gyroscope gyro;

	private Quaternion rotation;
	private GameObject cameraContainer;

	private void Start()
	{
		cameraContainer = new GameObject("Camera Container");
		cameraContainer.transform.position = transform.position;
		transform.SetParent(cameraContainer.transform); 

		gyroActive = EnableGyro();
	}

	private bool EnableGyro() 
	{
		// Already activated
		if (gyroActive) { return true; }

		// check if phone supports gyroscope
		if (SystemInfo.supportsGyroscope)
		{
			gyro = Input.gyro;
			gyro.enabled = true;
			//gyroActive = gyro.enabled;
			return true;
		}

		return false;
	}

	private void Update()
	{
		if (gyroActive)
		{
			rotation = gyro.attitude;
		}
	}

	public Quaternion GetGyroRotation() {
		return rotation;
	}
}
