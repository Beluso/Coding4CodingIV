using UnityEngine;
using System.Collections;

public class TestMineZone : MonoBehaviour 
{
    public bool runMZ = false;

    public MineZone mz;

	// Use this for initialization
	void Start () 
    {
	    
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if (runMZ)
        {
            mz.SpawnCube();
            runMZ = false;
        }
	}
}
