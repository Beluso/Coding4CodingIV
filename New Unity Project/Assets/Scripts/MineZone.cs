using UnityEngine;
using System.Collections;

public class MineZone : MonoBehaviour 
{
    public float warningTime = 5.0f;
	private float warningTimeReset = 0.0f;
    public bool spawnWarning = false;
    public bool mineExists = false;

    public GameObject warningCube;
    public GameObject mine;

	void Start()
	{
		warningCube.SetActive (false);
		warningTimeReset = warningTime;
	}
	
	void Update () 
    {
        if (spawnWarning)
        {
            if (warningTime > 0.0f)
            {
                warningTime -= Time.deltaTime;
            }

            if (warningTime < 0.0f)
            {
				warningTime = warningTimeReset;
                spawnWarning = false;
                SpawnMine();
            }
        }
	}

    public void SpawnCube()
    {
        mine.SetActive(false);
        mineExists = false;

        //warning period while mine is being created
        warningTime = warningTimeReset;

        //show transparent red box to show mine zone warning
        warningCube.SetActive(true);

        //trigger collider for red box
        warningCube.collider.isTrigger = true;
        
        spawnWarning = true;
    }

    public void SpawnMine()
    {
        warningCube.SetActive(false);

        mine.SetActive(true);
		Vector3 randomLocation = new Vector3 (0, Random.Range (-300f, 300f), 0);
		Debug.Log (randomLocation);
        mine.transform.localPosition = randomLocation;

		mineExists = true;
    }

    public void mineTrue()
    {
        mineExists = true;
    }
}
