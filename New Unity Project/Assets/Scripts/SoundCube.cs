using UnityEngine;
using System.Collections;

public class SoundCube : MonoBehaviour 
{
	public AudioClip explodeNoise;
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	void OnCollisionEnter(Collision other)
	{
		Debug.Log ("what" + Time.time);
		Instantiate (Resources.Load ("explode"), transform.position, transform.rotation);
		AudioManager.Instance.PlaySound (explodeNoise);
	}
}
