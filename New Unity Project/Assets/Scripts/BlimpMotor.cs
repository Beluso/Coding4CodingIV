using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BlimpMotor : MonoBehaviour 
{
//	private Camera mainCam;
	public float driveForce = 10.0f;
	private float driveForceReset;
	public float turnForce = 20.0f;
	public float liftForce = 10.0f;
	public float rocketForce = 20.0f;
//	public float minCamY = -1.0f;
//	public float maxCamY = 1.0f;
//	public float camTimer = 1.0f;
	public float lowDrag = 0.2f;
	public float highDrag = 0.8f;
	public float lowADrag = 3.0f;
	public float highADrag = 20.0f;
//	private float camTimerReset;
//	private Transform cameraOrigin;
	public HingeJoint[] propellers;
	public HingeJoint[] turnFins;
	public HingeJoint[] liftFins;
//	public GameObject target;
//	public GameObject targetOrigin;
	public GameObject rocketPrefab;
	public ParticleSystem LRocketPar;
	public ParticleSystem RRocketPar;
//	public GameObject secondCam;
//	public Transform secondCameraOrigin;
	private float fwdInput;
	private float turnInput;
	private float liftInput;
	
	public CameraMotor cameraMotor;
	public MissileArrays missileArray;
	private Camera mainCamera;

	/*
	 * List of "Motors":
	 * Rear/main fans for forward/back
	 * Rear vertical rudders for left/right rotation
	 * Rear horizontal rudders for lift and descend
	 * Side thruster for left/right movement
	 **/

	public void SetActiveBlimp()
	{
		missileArray.SetActiveBlimp ();
		cameraMotor.SetActiveBlimp ();
	}

	// Use this for initialization
	void Start () 
	{
		driveForceReset = driveForce;

		Debug.Log (transform.parent.name);
		if (networkView.isMine)
		{
			SetActiveBlimp ();
		}
	}

	// Update is called once per frame
	void Update ()
	{

//		CorrectDrift ();
//		AdditionalForces();

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
		foreach (HingeJoint hj in liftFins)
		{
			JointSpring js = hj.spring;
			js.targetPosition = 20 * liftInput;
			hj.spring = js;
		}
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
			rigidbody.drag = Mathf.Lerp(lowDrag, highDrag, Mathf.Clamp(2 * angle / 90f, 0, 1));
		else
			rigidbody.drag = Mathf.Lerp(highDrag, lowDrag, (angle / 90f) / 2);

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

	public void Fire(float input)
	{
		missileArray.Fire (input);
	}

	public void Aim(float input)
	{
		missileArray.Aim (input);
		if (input > .1f)
			driveForce = driveForceReset/2;
		else
			driveForce = driveForceReset;

	}

	public void Dodge(float input)
	{

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
		rigidbody.AddTorque (input * transform.up * turnForce);
	}

	public void Rise(float input)
	{
		liftInput = input;
		rigidbody.AddForce (input * Vector3.up * liftForce);
	}

	public void CamX(float input)
	{
		cameraMotor.XMovement (input);
	}

	public void CamY(float input)
	{
		cameraMotor.YMovement (input);
	}
}
