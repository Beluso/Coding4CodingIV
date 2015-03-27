using UnityEngine;
using System.Collections;

public class NetworkGun : MonoBehaviour 
{
	public GameObject prefab;
	public Transform firePoint;
	public float fireSpeed = 100f;
	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (networkView.isMine)
		{
			if (Input.GetAxis ("Fire1") > .1f)
			{
				Quaternion shootRotation = firePoint.rotation;
				shootRotation.eulerAngles = new Vector3 (shootRotation.eulerAngles.x, shootRotation.eulerAngles.y + Random.Range (0.0f, 10.0f), shootRotation.eulerAngles.z);
				GameObject go = (GameObject) Network.Instantiate(prefab, firePoint.position, shootRotation, 0);
				go.rigidbody.AddForce(firePoint.forward * fireSpeed);
			}
		}
	}
}