using UnityEngine;
using System.Collections;

public class BlimpController : MonoBehaviour 
{
	BlimpMotor bm;
	// Use this for initialization
	void Start () 
	{
		bm = GetComponent<BlimpMotor> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
//		if (Input.GetAxis("YAxis") < -.1f || Input.GetAxis("YAxis") > .1f)
		bm.Accel(Input.GetAxis("YAxis"));
//		if (Input.GetAxis ("XAxis") < -.1f || Input.GetAxis ("XAxis") > .1f)
		bm.Turn(Input.GetAxis ("XAxis"));
		if (Input.GetAxis ("Trigger") < -.1f || Input.GetAxis ("Trigger") > .1f)
			bm.Rise(Input.GetAxis ("Trigger"));
		if (Input.GetAxis ("R_XAxis") < -.1f || Input.GetAxis ("R_XAxis") > .1f)
			bm.CamX(Input.GetAxis ("R_XAxis"));
		if (Input.GetAxis ("R_YAxis") < -.1f || Input.GetAxis ("R_YAxis") > .1f)
			bm.CamY (Input.GetAxis ("R_YAxis"));
//		if (Input.GetAxis ("LBumper") < -.1f || Input.GetAxis ("LBumper") > .1f)
		bm.RocketLeft (Input.GetAxis ("LBumper"));
//		if (Input.GetAxis ("RBumper") < -.1f || Input.GetAxis ("RBumper") > .1f)
		bm.RocketRight (Input.GetAxis ("RBumper"));
	}
}
