using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GenerateCity
{
	[MenuItem("LevelGenerator/GenerateBuildings")]
	static void GenerateBuildings()
	{
		GameObject[] allPrefabs = Resources.FindObjectsOfTypeAll<GameObject>();
		List<GameObject> buildings = new List<GameObject>();
		foreach (GameObject go in allPrefabs)
		{
			if (go.tag == "Building")
			{
				buildings.Add(go);
				Debug.Log (go.name);
			}
		}

		// we know the prefabs are 292x98 meters, and we know we want to make a scene that is 4000x4000m
		// prefab transform is aligned to the center, so we need to offset by 146h and 49v
		//Vector3 spawnPoint = new Vector3 (-1854f, 0f, -1951f);
		GameObject parentObject = new GameObject ();
		parentObject.name = "Buildings";
		for (float v = -1951f; v < 2000f; v += 98f)
		{
			for (float h = -1854f; h < 2000f; h += 292f)
			{
				int roll = Random.Range (0, buildings.Count);
				GameObject newBuilding = (GameObject) PrefabUtility.InstantiatePrefab(buildings[roll]);
//				newBuilding.tag = "Untagged";
				newBuilding.transform.position = new Vector3(h, 0, v);
				newBuilding.transform.parent = parentObject.transform;
			}
		}
	}

}
