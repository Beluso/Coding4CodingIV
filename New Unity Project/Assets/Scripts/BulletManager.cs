using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BulletManager : MonoBehaviour 
{
	public int MAXBULLETS = 256;
	public GameObject bulletPrefab;

	private int inUse = 0; // number in use and index to next available one
	public List<Bullet> pool = new List<Bullet>();
	// Use this for initialization
	void Start () 
	{
		for (int i = 0; i < MAXBULLETS; i++)
		{
			GameObject go = (GameObject) Instantiate(bulletPrefab);
			pool.Add (go.GetComponent<Bullet>());
			go.transform.parent = gameObject.transform;
			go.name = "bullet" + i.ToString ("000");
			go.SetActive(false);
		}
	}
	
	public Bullet GetBullet()
	{
		Debug.Log (inUse);
		if (inUse < MAXBULLETS)
		{
			pool[inUse].gameObject.SetActive(true);
			pool[inUse].Activate();
			return pool[inUse++];	// return a bullet, and increment inUse
		}
		else
			return null;
	}

	public void ClearBullet(Bullet temp)
	{
		temp.gameObject.SetActive(false);
		inUse--;

		int b1 = temp.id;
		int b2 = inUse;

		int temp2 = b1;
		b1 = b2;
		b2 = temp2;
		Bullet temp3 = pool[b1];
		pool[b1] = pool[b2];
		pool[b2] = temp3;
	}
}
