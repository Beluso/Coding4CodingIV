using UnityEngine;
using System.Collections;

public class SpawnPrefab : MonoBehaviour 
{
	public Transform playerPrefab;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnNetworkLoadedLevel ()
	{
		Vector3 pos = new Vector3(Random.Range (-100, 100), Random.Range (100, 400), Random.Range (-100f, 100));
		Transform newBlimp = (Transform) Network.Instantiate(playerPrefab, pos, transform.rotation, 0);
		TerrainManager terrainManager = transform.parent.gameObject.GetComponentInChildren<TerrainManager> ();
		terrainManager.InitPlayer (newBlimp.FindChild("BBlimp").gameObject);
		LODManager lodManager = transform.parent.gameObject.GetComponentInChildren<LODManager> ();
		lodManager.InitPlayer (newBlimp.FindChild("BBlimp").gameObject);
	}

	void OnPlayerDisconnected (NetworkPlayer player)
	{
		Debug.Log("Server destroying player");
		Network.RemoveRPCs(player, 0);
		Network.DestroyPlayerObjects(player);
	}
}
