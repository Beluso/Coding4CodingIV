using UnityEngine;
using System.Collections;

public class Connector : MonoBehaviour {

	public string connectionIP = "127.0.0.1";
	public int connectionPort = 25001;
	private GameObject playerCube;
	private float seed;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
//			PrintMsg("fart" + Time.time.ToString());
			networkView.RPC ("PrintMsg", RPCMode.All, "fart" + Time.time.ToString());
		}
	}

	[RPC]
	void PrintMsg(string msg)
	{
		print (msg);
	}

	void OnPlayerConnected(NetworkPlayer player)
	{
		// called on server
		networkView.RPC ("SetSeed", RPCMode.OthersBuffered, seed);
	}

	[RPC]
	void SetSeed(float input)
	{
		Random.seed = (int)seed;
	}

	//Network.OnPlayerConnected
	void OnPlayerDisconnected(NetworkPlayer player)
	{
		// called on server
		Network.DestroyPlayerObjects (player);
	}
	void OnDisconnectedFromServer(NetworkDisconnection info)
	{
		Application.Quit ();
	}
	void OnServerInitialized()
	{
		seed = 100 * Random.value;
		Random.seed = (int)seed;
		playerCube = (GameObject)Network.Instantiate (Resources.Load ("Player"), Vector3.zero, Quaternion.identity, 0);
	}

	void OnConnectedToServer()
	{
		// called on client
		//send MoTD, spawn characters, etc.
		//Instantiate(cube)
		playerCube = (GameObject)Network.Instantiate (Resources.Load ("Player"), Vector3.zero, Quaternion.identity, 0);
	}
	
	void OnGUI()
	{
		if (Network.peerType == NetworkPeerType.Disconnected)
		{
			GUI.Label(new Rect(10, 10, 300, 20), "Status: Disconnected");
			if (GUI.Button(new Rect(10, 30, 120, 20), "Client Connect"))
			{
				Network.Connect(connectionIP, connectionPort);
			}
			if (GUI.Button(new Rect(10, 50, 120, 20), "Initialize Server"))
			{
				Network.InitializeServer(32, connectionPort, false);
			}
		}
		else if (Network.peerType == NetworkPeerType.Client)
		{
			GUI.Label(new Rect(10, 10, 300, 20), "Status: Connected as Client");
			if (GUI.Button(new Rect(10, 30, 120, 20), "Disconnect"))
			{
				Network.Disconnect(200);
			}
		}
		else if (Network.peerType == NetworkPeerType.Server)
		{
			GUI.Label(new Rect(10, 10, 300, 20), "Status: Connected as Server");
			if (GUI.Button(new Rect(10, 30, 120, 20), "Disconnect"))
			{
				Network.Disconnect(200);
			}
		}
	}
}
