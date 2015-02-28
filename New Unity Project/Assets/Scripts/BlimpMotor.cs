using UnityEngine;
using System.Collections;

public class BlimpMotor : MonoBehaviour 
{
	private Camera mainCam;
	public float driveForce = 10.0f;
	public float turnForce = 20.0f;
	public float liftForce = 10.0f;
	public float rocketForce = 20.0f;
	private float xCamMovement = 0.6f;
	public float minCamY = -1.0f;
	public float maxCamY = 1.0f;
	public float camTimer = 1.0f;
	public float lowDrag = 0.2f;
	public float highDrag = 0.8f;
	public float lowADrag = 3.0f;
	public float highADrag = 20.0f;
	private float camTimerReset;
	private Transform cameraOrigin;
	public HingeJoint[] propellers;
	public HingeJoint[] turnFins;
	public ParticleSystem LRocketPar;
	public ParticleSystem RRocketPar;
	private float fwdInput;
	private float turnInput;
	private float liftInput;
	/*
	 * List of "Motors":
	 * Rear/main fans for forward/back
	 * Rear vertical rudders for left/right rotation
	 * Rear horizontal rudders for lift and descend
	 * Side thruster for left/right movement
	 **/

	// Use this for initialization
	void Start () 
	{
		camTimerReset = camTimer;
		mainCam = Camera.main;
		cameraOrigin = mainCam.transform.parent.FindChild ("CameraOrigin").transform;
	}
	//top speed is approx 55m/s

	// Update is called once per frame
	void Update ()
	{
		camTimer -= Time.deltaTime;


//		CorrectDrift ();
		AdditionalForces();

		foreach (HingeJoint hj in propellers)
		{
			JointMotor jm = hj.motor;
			jm.targetVelocity = 1000 * fwdInput + 100;
			hj.motor = jm;
		}
		foreach (HingeJoint hj in turnFins)
		{
			JointSpring js = hj.spring;
			js.targetPosition = 40 * turnInput;
			hj.spring = js;
		}

		if (camTimer <= 0.0f)
		{
			camTimer = 0;
			mainCam.transform.position = Vector3.Lerp (mainCam.transform.position, cameraOrigin.position, Time.deltaTime);
			mainCam.transform.LookAt (transform.position);
		}

//		Debug.Log ("XZ magnitude" + Mathf.Sqrt (rigidbody.velocity.x * rigidbody.velocity.x + rigidbody.velocity.z * rigidbody.velocity.z));
	}

	void AdditionalForces()
	{
		Vector2 blVel = new Vector2 (rigidbody.velocity.x, rigidbody.velocity.z);
		Vector2 blDir = new Vector2 (transform.forward.x, transform.forward.z);
		/* angle between vectors is between 180 and 0
		 * the closer the angle is to 90, the lower the value
		 * the closer the angle is to 180 or 0, the higher the value
		 * piecewise the function to handle cases over 90 and below 90
		 * if it's below 90
		 * 		//angle is a percentage of 90
		 * 		drag = lerp(lowdrag, highdrag, angle/90
		 * if it;s above 90,
		 * 		subtract 90
		 * 		lerp(highdrag, lowdrag, angle/90
		 */
		float angle = Vector2.Angle (blVel, blDir);
//		Debug.Log ("blVel = " + blVel + ", blDir = " + blDir + ", angle = " + angle);
		if (angle < 90)
			rigidbody.drag = Mathf.Lerp (lowDrag, highDrag, angle / 90f);
		else
			rigidbody.drag = Mathf.Lerp(highDrag, lowDrag, angle / 90f);

		// if it's going less than 6 mph, reduce its ability to turn
		if (Vector2.SqrMagnitude(blVel) < 9.0f)
		{
			rigidbody.angularDrag = Mathf.Lerp(highADrag, lowADrag, Vector2.SqrMagnitude(blVel) / 16.0f);
		}
		else
			rigidbody.angularDrag = lowADrag;

	}
	
	void CorrectDrift()
	{
		// the x and z euler axes need to lerp to zero
		transform.eulerAngles = new Vector3 (Mathf.Clamp(transform.eulerAngles.x, 0, 30), transform.eulerAngles.y, Mathf.Clamp(transform.eulerAngles.z, 0, 30));
		transform.eulerAngles = new Vector3 (Mathf.Lerp (transform.eulerAngles.x, 0, Time.deltaTime * 2.0f), transform.eulerAngles.y, Mathf.Lerp (transform.eulerAngles.z, 0, Time.deltaTime * 2.0f));
	}

	public void Accel(float input)
	{
		fwdInput = input;
		if (input < 0f)
			input *= .5f;
		rigidbody.AddForce (input * transform.forward * driveForce);
	}

	public void RocketLeft(float input)
	{
		LRocketPar.enableEmission = true;
		if (input == 0)
			LRocketPar.enableEmission = false;
		rigidbody.AddForce (input * -transform.right * rocketForce);
	}

	public void RocketRight(float input)
	{
		RRocketPar.enableEmission = true;
		if (input == 0)
			RRocketPar.enableEmission = false;
		rigidbody.AddForce (input * transform.right * rocketForce);
	}

	public void Turn(float input)
	{
		turnInput = input;
		rigidbody.AddTorque (input * Vector3.up * turnForce);
	}

	public void Rise(float input)
	{
		liftInput = input;
		rigidbody.AddForce (input * Vector3.up * liftForce);
	}

	public void CamX(float input)
	{
		camTimer = camTimerReset;
		mainCam.transform.RotateAround (transform.position, Vector3.up,  2 * input * -xCamMovement);
	}

	public void CamY(float input)
	{
		camTimer = camTimerReset;
		mainCam.transform.RotateAround (transform.position, mainCam.transform.right, input * xCamMovement);
		
		if (mainCam.transform.localPosition.y > maxCamY)
			mainCam.transform.RotateAround (transform.position, mainCam.transform.right, -input * xCamMovement);
		else if (mainCam.transform.localPosition.y < minCamY)
			mainCam.transform.RotateAround (transform.position, mainCam.transform.right, -input * xCamMovement);
	}
}
