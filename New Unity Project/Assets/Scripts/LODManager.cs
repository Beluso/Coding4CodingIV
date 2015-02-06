using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LODManager : MonoBehaviour 
{
	private float tileW;
	private TerrainManager terrainManager;
	private Transform player;

	// Use this for initialization
	void Start () 
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
		terrainManager = GetComponent<TerrainManager>();
		tileW = Vector3.Distance(terrainManager.tile[0].transform.position, terrainManager.tile[1].transform.position);
	}
	
	// Update is called once per frame
	void Update () 
	{
		CheckTiles();
	}

	void CheckTiles()
	{
		foreach (Tile tile in terrainManager.tile)
		{
			Vector2 playerPos = new Vector2 (player.position.x, player.position.z);
			Vector2 tilePos = new Vector2 (tile.transform.position.x, tile.transform.position.z);

			MeshDetail reqDetail;
			// determine how close this tile is to the player
			// the tile the player is standing on is always high detail, if the player wanders close to an outer tile, set to high detail
			if (Vector2.Distance(playerPos, tilePos) < terrainManager.displayThreshold * tileW)
				reqDetail = MeshDetail.HIGH;
			else if (Vector2.Distance(playerPos, tilePos) < terrainManager.displayThreshold * tileW * 2)
				reqDetail = MeshDetail.MED;
			else
				reqDetail = MeshDetail.LOW;

			if (tile.meshDetail != reqDetail)
			{
				tile.meshDetail = reqDetail;
				MeshManager.Instance.GenerateMesh(tileW, tile.GetComponent<MeshFilter>(), tile.meshDetail, tile.transform.localScale.x);
			}
		}
	}
}
