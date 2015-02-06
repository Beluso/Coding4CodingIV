using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainManager : MonoBehaviour 
{
	public float displayThreshold = 0.5f;
	public float shiftThreshold = 2.0f;

	public Tile[] tile;
	private float tileW;
	public Transform player;
	
	public Prop[] propPrefabs;
	private Dictionary<string, List<Prop>> propPool;
	public int maxObjsPerTile = 30;
	private int temp;
	// Use this for initialization
	void Start () 
	{
		tileW = Vector3.Distance(tile[0].transform.position, tile[1].transform.position);
		propPool = new Dictionary<string, List<Prop>>();
		foreach (Prop prop in propPrefabs)
			propPool[prop.propName] = new List<Prop>();
		for (int i = 0; i < tile.Length; i++)
			tile[i].gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () 
	{
		TileShift();
		TileDisplay();
		/*
		 * for each terrain tile
		 * 	if x dist or z dist < .5 * w
		 * 		display tile
		 * 	else
		 * 		dont display
		 * 
		 * 	if x dist or z dist > 2 * w
		 * 		move in x or z (respectively)
		 * */
	}

	void TileShift()
	{
		foreach (Tile ti in tile)
		{
			if (Mathf.Abs (player.position.x - ti.transform.position.x) > shiftThreshold * tileW)
			{
				if (player.position.x > ti.transform.position.x) //player's to the right, move right
					ti.transform.position = new Vector3(ti.transform.position.x + 3 * tileW, ti.transform.position.y, ti.transform.position.z);
				else
					ti.transform.position = new Vector3(ti.transform.position.x - 3 * tileW, ti.transform.position.y, ti.transform.position.z);
			}
			if (Mathf.Abs (player.position.z - ti.transform.position.z) > shiftThreshold * tileW)
			{
				if (player.position.z > ti.transform.position.z) //player's to the above, move up
					ti.transform.position = new Vector3(ti.transform.position.x, ti.transform.position.y, ti.transform.position.z + 3 * tileW);
				else
					ti.transform.position = new Vector3(ti.transform.position.x, ti.transform.position.y, ti.transform.position.z - 3 * tileW);
			}
		}
	}

	void TileDisplay()
	{
		foreach (Tile ti in tile)
		{
			Vector2 playerPos = new Vector2 (player.position.x, player.position.z);
			Vector2 tilePos = new Vector2 (ti.transform.position.x, ti.transform.position.z);

			if (Vector2.Distance(playerPos, tilePos) < displayThreshold * tileW)
			{
				if (ti.gameObject.activeSelf == false)
					StartCoroutine(ActivateTile(ti));
			}
			else
			{
				if (ti.gameObject.activeSelf == true)
					DeactivateTile(ti);
			}

		}
	}
	
	IEnumerator ActivateTile(Tile ti)
	{
		/* weakness when multiple ActivateTile coroutines are running in parallel
		 * causes randomness to be non-determinate because random value is being used for different
		 * tiles at different times
		 * ex) case 1, tile A calls ActivateTile, calls random 1000 times. later, tile B calls ActivateTile, calls random 1000 times
		 * case 2, tile A calls ActivateTile, calls random 200 times. tile B calls ActivateTile before before tile A manages to make 1000,
		 * tile A and tile B alternate calling random
		 */

		ti.gameObject.SetActive(true);

		Random.seed = (int)(ti.transform.position.x * 1000 + ti.transform.position.z);
		int numObj = Random.Range (0, maxObjsPerTile);
		Prop prop;
		int type;
		for (int i = 0; i < numObj; i++)
		{
			type = Random.Range (0, propPool.Count);
			prop = GetObj(propPrefabs[type]);
			prop.transform.localScale = propPrefabs[type].transform.localScale;
			prop.transform.rotation = propPrefabs[type].transform.rotation;
			prop.transform.position = propPrefabs[type].transform.position;
			
			ti.props.Add (prop);
			
			prop.transform.localScale = new Vector3(prop.transform.localScale.x, prop.transform.localScale.y * Random.Range (0.9f, 1.1f), prop.transform.localScale.z);
			prop.transform.rotation.Set (prop.transform.rotation.x, prop.transform.rotation.y * Random.Range (0, 360), prop.transform.rotation.z, prop.transform.rotation.w);
			prop.transform.position = ti.transform.position;
			prop.transform.position = new Vector3(ti.transform.position.x + Random.Range (-tileW/2 , tileW/2), ti.transform.position.y, ti.transform.position.z + Random.Range(-tileW/2, tileW/2));

			if (i % 20 == 19)
				yield return null;
		}
	}
	
	void DeactivateTile(Tile ti)
	{
		ti.gameObject.SetActive (false);
		ClearObjs(ti.props);
	}
	
	Prop GetObj(Prop pr)
	{
		Prop newProp;
		if (propPool[pr.propName].Count > 0)
		{
			newProp = propPool[pr.propName][0];
			newProp.gameObject.SetActive(true);
			propPool[pr.propName].RemoveAt(0);
		}
		else
		{
			GameObject temp = (GameObject) Instantiate(pr.gameObject);
			newProp = temp.GetComponent<Prop>();
		}
		return newProp;
	}
	
	void ClearObjs(List<Prop> temp)
	{
		foreach (Prop pr in temp)
		{
			pr.gameObject.SetActive(false);
			propPool[pr.propName].Add (pr);
		}
		temp.Clear ();
	}
}
