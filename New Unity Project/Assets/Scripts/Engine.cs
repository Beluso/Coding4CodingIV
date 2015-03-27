using UnityEngine;
using System.Collections;


public class Engine : MonoBehaviour 
{
	public float force = 20.0f;
	private int pulse = 0;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (pulse == 1)
		{
			GetComponent<Rigidbody>().AddForce(transform.up * force);
		}
		else if (pulse == -1)
		{
			GetComponent<Rigidbody>().AddForce (-transform.up * force);
		}

		pulse = 0;
	}

	public void Thrust()
	{
		pulse = 1;
	}

	public void ReverseThrust()
	{
		pulse = -1;
	}
}