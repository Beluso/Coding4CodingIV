using UnityEngine;
using System.Collections;

public class TestLerp : MonoBehaviour 
{
	public float test;

	// Use this for initialization
	void Start () 
	{
		test = 90;
	}
	
	// Update is called once per frame
	void Update () 
	{
		test = Mathf.Lerp (test, 0,  Time.deltaTime);
		Debug.Log (test);
	}
}
