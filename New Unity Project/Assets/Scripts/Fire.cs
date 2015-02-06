using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour 
{
	private BulletManager bulletManager;
	public GameObject bulletPrefab;
	public GameObject nozzle;
	public float speed = 15.0f;

	// Use this for initialization
	void Start () 
	{
		bulletManager = GameObject.FindGameObjectWithTag ("BulletManager").GetComponent<BulletManager> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKey (KeyCode.Space))
		{
			Bullet newBullet = bulletManager.GetBullet();
			if (newBullet)
			{
				newBullet.transform.position = nozzle.transform.position;
				newBullet.transform.rotation = nozzle.transform.rotation;
				newBullet.rigidbody.velocity = nozzle.transform.up * speed;
			}
		}
	}
}
