using UnityEngine;
using System.Collections;

public class BlimpSpawner : MonoBehaviour 
{
	private bool respawning = false;
	private float respawnTime;
	Transform[] children;
	// Use this for initialization
	void Start () 
	{
		children = gameObject.transform.GetComponentsInChildren<Transform> ();
		if (networkView.isMine)
		{
			networkView.RPC ("Respawn", RPCMode.All, 2.0f);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (respawning)
		{
			respawnTime -= Time.deltaTime;

			if (respawnTime <= 0)
			{
				respawning = false;
				foreach(Transform child in children)
				{
					if (child != gameObject.transform)
						child.gameObject.SetActive(true);
				}
			}
		}
	}

	[RPC]
	void Respawn(float input)
	{
		transform.position = new Vector3(0f, 1000f, 0f);
		respawning = true;
		respawnTime = input;
		foreach (Transform child in children)
		{
			if (child != gameObject.transform)
				child.gameObject.SetActive(false);
		}
	}
}
