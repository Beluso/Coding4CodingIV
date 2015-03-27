using UnityEngine;
using System.Collections;

public class CameraMotor : MonoBehaviour 
{
	public bool missileCamera = false;
	public HingeJoint xAxis;
	public HingeJoint yAxis;

	public void SetActiveBlimp()
	{
		GameObject mainCam = GameObject.FindGameObjectWithTag("MainCamera");
		mainCam.transform.SetParent(yAxis.transform.FindChild ("CameraSlot"));
		mainCam.transform.localPosition = Vector3.zero;
		mainCam.transform.localRotation = Quaternion.identity;
		mainCam.transform.localScale = Vector3.one;
		Debug.Log ("camera set");
	}
	
	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	public void XMovement(float input)
	{
		JointSpring js = xAxis.spring;
		float tPosition = js.targetPosition + input;
		if (tPosition > 300)
			tPosition -= 360;
		if (tPosition < -300)
			tPosition += 360;
		js.targetPosition = tPosition;
		xAxis.spring= js;

	}

	public void YMovement(float input)
	{
		JointSpring js = yAxis.spring;
		js.targetPosition = Mathf.Clamp (js.targetPosition + input, yAxis.limits.min, yAxis.limits.max);
		yAxis.spring= js;
	}
}
