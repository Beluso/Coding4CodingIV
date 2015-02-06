using UnityEngine;
using System.Collections;

public class TileLerp : MonoBehaviour 
{
	private GameObject parentTile;
	private Vector3 prevPos;
	public float tileSpeed = 20.0f;
	public float drift = 3.0f;
	public float bobHeight = 1.0f;
	public float bobSpeed = 1.0f;
	// Use this for initialization
	void Start () 
	{
		GameObject[] possibleTiles = GameObject.FindGameObjectsWithTag("Terrain");
		float dist = float.PositiveInfinity;
		foreach (GameObject tile in possibleTiles)
		{
			if (Vector3.Distance(gameObject.transform.position, tile.transform.position) < dist)
			{
				dist = Vector3.Distance(gameObject.transform.position, tile.transform.position);
				parentTile = tile;
			}
		}
		prevPos = parentTile.transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (parentTile.transform.position != prevPos)
		{
			prevPos = parentTile.transform.position;
			StartCoroutine("MoveTile");
		}
		
		Bob();
	}
	
	IEnumerator MoveTile()
	{
		float halfDist = Vector3.Distance(transform.position, prevPos) / 2;
		float perc;
		while (Vector3.Distance(transform.position, prevPos) > 0.05f)
		{
			transform.position = Vector3.Lerp(transform.position, prevPos, Time.deltaTime * tileSpeed);
			float toDest = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(prevPos.x, prevPos.z));
			if (toDest > halfDist)
				perc = (halfDist * 2 - toDest) / halfDist; // function where closer to halfway point, percentage becomes bigger
			else
				perc = toDest / halfDist; // closer to destination, percentage becomes smaller
				
			transform.position = new Vector3(transform.position.x, transform.position.y - drift * perc, transform.position.z);
			
			yield return null;
		}
	}
	
	void Bob()
	{
		// distance  = amplitude * sine(time / period)
		float distance = (bobHeight / 1000) * Mathf.Sin(Time.time / (1 / bobSpeed));
		transform.position = new Vector3(transform.position.x, transform.position.y - distance, transform.position.z);
	}
}
