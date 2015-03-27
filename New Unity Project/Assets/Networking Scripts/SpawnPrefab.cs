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
		Vector3 pos = new Vector3(Random.Range (-1800, 1800), Random.Range (400, 1200), Random.Range (-1800f, 1800));
		 Network.Instantiate(playerPrefab, pos, transform.rotation, 0);
	}

	void OnPlayerDisconnected (NetworkPlayer player)
	{
		Debug.Log("Server destroying player");
		Network.RemoveRPCs(player, 0);
		Network.DestroyPlayerObjects(player);
	}
}
