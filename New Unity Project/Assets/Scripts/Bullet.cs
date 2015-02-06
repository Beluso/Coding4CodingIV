using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour 
{
	public float expireTime = 1.0f;
	private BulletManager bulletManager;
	static int numBullets = 0;
	public int id = 0;
	// Use this for initialization
	void Start () 
	{
		id = numBullets;
		numBullets++;
		bulletManager = GameObject.FindGameObjectWithTag("BulletManager").GetComponent<BulletManager>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag != "Bullet")
			bulletManager.ClearBullet(this);
	}

	IEnumerator Expire()
	{
		yield return new WaitForSeconds(expireTime);
		bulletManager.ClearBullet(this);
	}

	public void Activate()
	{
		StartCoroutine("Expire");
	}
}
