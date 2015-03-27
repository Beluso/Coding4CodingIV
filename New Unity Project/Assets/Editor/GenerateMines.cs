using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GenerateMines
{
	[MenuItem("LevelGenerator/GenerateMineZones")]
	static void GenerateMineZones()
	{
		GameObject[] allPrefabs = Resources.FindObjectsOfTypeAll<GameObject>();
		List<GameObject> mines = new List<GameObject>();
		foreach (GameObject go in allPrefabs)
		{
			if (go.tag == "MineZone")
			{
				mines.Add(go);
				Debug.Log (go.name);
			}
		}
		
		// we know the prefabs are 292x98 meters, and we know we want to make a scene that is 4000x4000m
		// prefab transform is aligned to the center, so we need to offset by 146h and 49v
		//Vector3 spawnPoint = new Vector3 (-1854f, 0f, -1951f);
		GameObject parentObject = new GameObject ();
		parentObject.name = "Mines";
		for (float v = -1850f; v < 2000f; v += 150f)
		{
			for (float h = -1850f; h < 2000f; h += 150f)
			{
//				int roll = Random.Range (0, mines.Count);
				Debug.Log (mines.Count);
				GameObject newBuilding = (GameObject) PrefabUtility.InstantiatePrefab(mines[0]);
				newBuilding.transform.position = new Vector3(h, 500, v);
				newBuilding.transform.parent = parentObject.transform;
			}
		}
	}
	
}
