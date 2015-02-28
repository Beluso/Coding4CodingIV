using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour 
{
	public float speed = 10;
	public Vector3 velocity;
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		//NetworkInstantiate
		//NetworkView.owner
		if (Network.isServer)
		{
			velocity = Vector3.zero;
			if (Input.GetKey(KeyCode.W))
			{
				velocity = speed * transform.forward;
				transform.position += speed * transform.forward * Time.deltaTime;
			}
			else if (Input.GetKey(KeyCode.S))
			{
				velocity = -speed * transform.forward;
				transform.position -= speed * transform.forward * Time.deltaTime;
			}
			networkView.RPC ("SetTrajectory", RPCMode.Others, transform.position, velocity);
		}
		else
		{
			//client behavior
			transform.position += velocity * Time.deltaTime;
		}
	}

	[RPC]
	void SetTrajectory(Vector3 pos, Vector3 v)
	{
		transform.position = pos;
		velocity = v;
	}
}
