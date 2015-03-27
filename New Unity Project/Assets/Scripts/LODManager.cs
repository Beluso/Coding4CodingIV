using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LODManager : MonoBehaviour 
{
	private TerrainManager terrainManager;
	private Transform player;

	// Use this for initialization
	void Start () 
	{
	}

	public void InitPlayer(GameObject input)
	{
		player = input.transform;
		terrainManager = GetComponent<TerrainManager>();
	}

	// Update is called once per frame
	void Update () 
	{
		CheckTiles();
	}

	void CheckTiles()
	{
		if (player != null)
		{
			foreach (Tile tile in terrainManager.tile)
			{
				Vector2 playerPos = new Vector2 (player.position.x, player.position.z);
				Vector2 tilePos = new Vector2 (tile.transform.position.x, tile.transform.position.z);

				MeshDetail reqDetail;
				// determine how close this tile is to the player
				// the tile the player is standing on is always high detail, if the player wanders close to an outer tile, set to high detail
				if (Vector2.Distance(playerPos, tilePos) < terrainManager.displayThreshold * terrainManager.tileW)
					reqDetail = MeshDetail.HIGH;
				else if (Vector2.Distance(playerPos, tilePos) < terrainManager.displayThreshold * terrainManager.tileW * 2)
					reqDetail = MeshDetail.MED;
				else
					reqDetail = MeshDetail.LOW;

				if (tile.meshDetail != reqDetail)
				{
					tile.meshDetail = reqDetail;
					MeshManager.Instance.Regenerate(terrainManager.tileW, tile.GetComponent<MeshFilter>(), tile.meshDetail, tile.transform.localScale.x);
				}
			}
		}
	}
}
