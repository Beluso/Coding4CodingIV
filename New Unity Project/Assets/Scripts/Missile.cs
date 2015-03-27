using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour 
{
	public GameObject explosionPrefab;
	public float force = 10;
	private GameObject model;
	private float timer = 0f;
	// Use this for initialization
	void Start () 
	{
		rigidbody.AddForce (transform.forward * force);
	}

	void FixedUpdate () 
	{
		if (networkView.isMine)
		{
			timer += Time.deltaTime;
			if (timer > 5.0f)
			{
				networkView.RPC("Explode", RPCMode.Others);
				Explode ();
				Network.Destroy (gameObject);
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Target")
		{
			Debug.Log ("woohoO!");
			Destroy (gameObject);
		}
	}

	void OnCollisionEnter(Collision other)
	{
		if (networkView.isMine)
		{
			if (other.transform.root.networkView != null)
			{
				GameObject go = other.transform.transform.root.gameObject;
				NetworkDamage d = go.GetComponentInChildren<NetworkDamage>();
				if (d != null)
				{
					d.networkView.RPC ("TakeDamage", RPCMode.All);
				}
			}

			networkView.RPC("Explode", RPCMode.Others);
			Explode ();
			Network.Destroy (gameObject);
		}
	}

	[RPC]
	void Explode()
	{
		GameObject explosion = (GameObject) Instantiate (explosionPrefab, transform.position, transform.rotation);
		Destroy (explosion, 2.0f);
	}
}
