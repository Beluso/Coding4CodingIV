using UnityEngine;
using System.Collections;

public class NetworkProjectile : MonoBehaviour 
{
	public GameObject explosion;
	public float damage;
	public float timeout = 3f;
	// Use this for initialization
	IEnumerator Start () 
	{
		yield return new WaitForSeconds(timeout);
		if (networkView.isMine)
			Network.Destroy (gameObject);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnCollisionEnter(Collision other)
	{
		if (networkView.isMine)
		{
			if (other.transform.root.networkView != null)
			{
				GameObject go = other.transform.transform.root.gameObject;
				NetworkDamage d = go.GetComponent<NetworkDamage>();
				if (d != null)
				{
					d.networkView.RPC ("TakeDamage", RPCMode.Others, damage);
				}
			}
			if (other.collider.tag != "Projectile")
			{
				networkView.RPC("Explode", RPCMode.Others);
				Explode ();
				Network.Destroy (gameObject);
			}
		}
	}

	[RPC]
	void Explode()
	{
		Instantiate (explosion, transform.position, transform.rotation);
	}
}
