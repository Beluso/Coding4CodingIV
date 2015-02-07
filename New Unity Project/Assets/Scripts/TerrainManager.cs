using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainManager : MonoBehaviour 
{
	public float displayThreshold = 0.5f;
	public float shiftThreshold = 2.0f;

	public Tile[] tile;
	public float tileW;
	private Transform player;

	public Prop[] propPrefabs;
	private Dictionary<string, List<Prop>> propPool;
	public int maxObjsPerTile = 30;
	private int temp;
	// Use this for initialization
	void Start () 
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
		float dist = float.PositiveInfinity;
		GameObject[] temp = GameObject.FindGameObjectsWithTag("Terrain");
		tile = new Tile[temp.Length];
		for (int i = 0; i < temp.Length; i++)
			tile[i] = temp[i].GetComponent<Tile>();
		foreach (Tile ti in tile)
		{
			if (ti != tile[0])
			{
				if (Vector3.Distance(ti.transform.position, tile[0].transform.position) < dist)
				{
					dist = Vector3.Distance(ti.transform.position, tile[0].transform.position);
				}
			}
		}
		tileW = dist;
		propPool = new Dictionary<string, List<Prop>>();
		foreach (Prop prop in propPrefabs)
			propPool[prop.propName] = new List<Prop>();
//		for (int i = 0; i < tile.Length; i++)
//			tile[i].gameObject.SetActive(false);
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
				if (ti.active == false)
					StartCoroutine(ActivateTile(ti));
			}
			else
			{
				if (ti.active == true)
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

		ti.active = true;

		Random.seed = (int)(ti.transform.position.x * 1000 + ti.transform.position.z);
		int numObj = Random.Range (0, maxObjsPerTile);
		/*
		 * random table for:
		 * int[] type[numObj]
		 * float[] scale[numObj]
		 * float[] rotation[numObj]
		 * vector2[] pos[numObj]
		 */
		int[] type = new int[numObj];
		float[] scale = new float[numObj];
		float[] rotation = new float[numObj];
		Vector2[] pos = new Vector2[numObj];

		for (int i = 0; i < type.Length; i++)
		{
			type[i] = Random.Range(0, propPool.Count);
			scale[i] = Random.Range (0.9f, 1.1f);
			rotation[i] = Random.Range (0.0f, 360.0f);
			pos[i] = new Vector2(Random.Range(-tileW / 2, tileW / 2), Random.Range(-tileW / 2, tileW / 2));
		}

		Prop prop;
		for (int i = 0; i < numObj; i++)
		{
			prop = GetObj(propPrefabs[type[i]]);
			prop.transform.localScale = propPrefabs[type[i]].transform.localScale;
			prop.transform.rotation = propPrefabs[type[i]].transform.rotation;
			prop.transform.position = propPrefabs[type[i]].transform.position;
			
			ti.props.Add (prop);
			
			prop.transform.localScale = new Vector3(prop.transform.localScale.x, prop.transform.localScale.y * scale[i], prop.transform.localScale.z);
			prop.transform.rotation.Set (prop.transform.rotation.x, prop.transform.rotation.y * rotation[i], prop.transform.rotation.z, prop.transform.rotation.w);
			prop.transform.position = ti.transform.position;
			prop.transform.position = new Vector3(ti.transform.position.x + pos[i].x, ti.transform.position.y, ti.transform.position.z + pos[i].y);

			if (i % 20 == 19)
				yield return null;
		}
	}
	
	void DeactivateTile(Tile ti)
	{
		ti.active = false;
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
