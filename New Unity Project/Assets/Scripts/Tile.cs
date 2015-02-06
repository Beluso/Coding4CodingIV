using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour 
{
	public List<Prop> props;
	// Use this for initialization
	void Awake () 
	{
		props = new List<Prop> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
