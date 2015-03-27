using UnityEngine;
using System.Collections;

public class PlayerUnrenderer : MonoBehaviour 
{
	private Transform player;
	// Use this for initialization
	void Start () 
	{
		TerrainManager tm = GameObject.FindGameObjectWithTag ("TerrainManager").GetComponent<TerrainManager> ();;
		player = tm.player;
	}
	
	void FixedUpdate () 
	{
		if (Vector3.Distance(transform.position, player.position) > 200f)
		{
			Renderer[] renderers;
			if (gameObject.transform.parent != null)
			{
				renderers = gameObject.transform.parent.GetComponentsInChildren<Renderer>();
			}
			else
			{
				renderers = gameObject.GetComponentsInChildren<Renderer>();
			}

			foreach (Renderer renderer in renderers)
			{
				renderer.enabled = false;
			}
		}
		else
		{
			Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
			foreach (Renderer renderer in renderers)
			{
				renderer.enabled = true;
			}
		}
	}
}
