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
		Vector3 pos = new Vector3(Random.Range (-1800, 1800), Random.Range (400, 1200), Random.Range (-1800f, 1800));
		transform.FindChild("BBlimp").position = pos;
	}
}
