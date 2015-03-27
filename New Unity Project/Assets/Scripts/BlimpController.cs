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
		if (networkView.isMine)
		{
			bm.Accel(Input.GetAxis("Vertical"));
			bm.Turn(Input.GetAxis ("Horizontal"));
			bm.Rise (Input.GetAxis ("Rise/Fall"));
			bm.Aim (Input.GetAxis ("Aim"));
			bm.Fire (Input.GetAxis ("Fire"));
			bm.CamX(Input.GetAxis ("Cam Horizontal"));
			bm.CamY (Input.GetAxis ("Cam Vertical"));
			networkView.RPC ("SendInputs", RPCMode.Others, Input.GetAxis("Vertical"), Input.GetAxis ("Horizontal"), Input.GetAxis ("Rise/Fall"), Input.GetAxis ("Aim"));
		}
	}

	[RPC]
	void SendInputs(float vertical, float horizontal, float riseFall, float aim)
	{
		bm.Accel(vertical);
		bm.Turn(horizontal);
		bm.Rise (riseFall);
		bm.Aim (aim);
	}
}
