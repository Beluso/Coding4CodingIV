using UnityEngine;
using System.Collections;

public class Hats : MonoBehaviour 
{
	public GameObject cowboyHat;
	public GameObject captainHat;
	public GameObject witchHat;
	// Use this for initialization
	void Start ()
	{
		foreach (Renderer render in cowboyHat.GetComponentsInChildren<Renderer>())
			render.enabled = false;
		foreach (Renderer render in captainHat.GetComponentsInChildren<Renderer>())
			render.enabled = false;
		foreach (Renderer render in witchHat.GetComponentsInChildren<Renderer>())
			render.enabled = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	void OnGUI()
	{
		GUILayout.BeginHorizontal ();
		GameObject newHat = null;
		if (networkView.isMine)
		{
			if (GUILayout.Button("Cowboy Hat"))
			{
				networkView.RPC ("EnableCowboyHat", RPCMode.All);
			}
			else if (GUILayout.Button("Captain's Hat"))
			{
				networkView.RPC ("EnableCaptainHat", RPCMode.All);
			}
			else if (GUILayout.Button("Witch Hat"))
			{
				networkView.RPC ("EnableWitchHat", RPCMode.All);
			}
		}
		GUILayout.EndHorizontal ();
	}

	[RPC]
	void EnableCowboyHat()
	{
		foreach (Renderer render in cowboyHat.GetComponentsInChildren<Renderer>())
			render.enabled = true;
		foreach (Renderer render in captainHat.GetComponentsInChildren<Renderer>())
			render.enabled = false;
		foreach (Renderer render in witchHat.GetComponentsInChildren<Renderer>())
			render.enabled = false;
	}

	[RPC]
	void EnableCaptainHat()
	{
		foreach (Renderer render in cowboyHat.GetComponentsInChildren<Renderer>())
			render.enabled = false;
		foreach (Renderer render in captainHat.GetComponentsInChildren<Renderer>())
			render.enabled = true;
		foreach (Renderer render in witchHat.GetComponentsInChildren<Renderer>())
			render.enabled = false;
	}

	[RPC]
	void EnableWitchHat()
	{
		foreach (Renderer render in cowboyHat.GetComponentsInChildren<Renderer>())
			render.enabled = false;
		foreach (Renderer render in captainHat.GetComponentsInChildren<Renderer>())
			render.enabled = false;
		foreach (Renderer render in witchHat.GetComponentsInChildren<Renderer>())
			render.enabled = true;
	}
}
