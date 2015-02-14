using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum MeshDetail
{
	HIGH,
	MED,
	LOW
}

public class Tile : MonoBehaviour 
{
	public List<Prop> props;
	public MeshDetail meshDetail = MeshDetail.LOW;
	public bool active = false;
	public GameObject[] skirts;
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
