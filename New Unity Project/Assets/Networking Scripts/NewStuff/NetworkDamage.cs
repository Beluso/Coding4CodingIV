using UnityEngine;
using System.Collections;

public class NetworkDamage : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	//Remote Procedure Call
	[RPC]
	public void TakeDamage()
	{
		Debug.Log ("GameOver");
		Vector3 pos = new Vector3(Random.Range (-100, 100), Random.Range (100, 400), Random.Range (-100f, 100));
		transform.FindChild("BBlimp").position = pos;
	}
}
