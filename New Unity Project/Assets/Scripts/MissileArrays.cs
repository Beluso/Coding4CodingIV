using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MissileArrays : MonoBehaviour 
{
	public ConfigurableJoint[] missileArrays;
	public HingeJoint camXHinge;
	public HingeJoint leftXHinge;
	public HingeJoint leftYHinge;
	public Transform leftMissileOrigin;
	public Transform leftMissileAim;
	public HingeJoint rightXHinge;
	public HingeJoint rightYHinge;
	public Transform rightMissileOrigin;
	public Transform rightMissileAim;
	public RectTransform crosshair;
	private Camera mainCamera;
	private bool leftArrayActive = true;
	public GameObject missilePrefab;

	private float chargeTime = 0f;
	public Text charge;

	public GameObject blimp;

	private bool dumbBlimp = true;

	public void SetActiveBlimp()
	{
		crosshair = GameObject.FindGameObjectWithTag("Crosshair").GetComponent<RectTransform>();
		charge = GameObject.FindGameObjectWithTag("ChargeTimer").GetComponent<Text>();
		dumbBlimp = false;
		Debug.Log ("missiles set");
	}

	// Use this for initialization
	void Start ()
	{
		mainCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!dumbBlimp)
			charge.text = chargeTime.ToString();
	}

	public void Aim(float input)
	{
		if (!dumbBlimp)
		{
			if (input > .1f)
			{
				// user is aiming
				ArrayActive(true);
				SetCrosshair(true);
				float direction = camXHinge.spring.targetPosition;
				if (direction > 360f)
					direction -= 360f;
				else if (direction < 0f)
					direction += 360f;

				if (direction > 0f && direction < 180f)
				{
					Debug.Log ("yes");
					CalcHinges(rightXHinge, rightYHinge, rightMissileOrigin, rightMissileAim);
					leftArrayActive = false;
					crosshair.GetComponent<Image>().color = Color.blue;
				}
				else
				{
					CalcHinges(leftXHinge, leftYHinge, leftMissileOrigin, leftMissileAim);
					leftArrayActive = true;
					crosshair.GetComponent<Image>().color = Color.yellow;
				}
			}
			else
			{
				// user is not aiming
				SetCrosshair(false);
				ArrayActive (false);
			}
		}
		else
		{
			if (input > .1f)
			{
				ArrayActive(true);
			}
			else
			{
				ArrayActive(false);
			}
		}
	}

	public void Fire(float input)
	{
		if (!dumbBlimp)
		{
			if (input > .1f)
			{
				if (crosshair.parent.GetComponent<Canvas>().enabled)
				{
					//start charging
					chargeTime = Mathf.Clamp(chargeTime + Time.deltaTime, 0f, 1f);
				}
			}
			else if (!(input > .1f) && chargeTime > 0.1f)
			{
				if (leftArrayActive)
				{
					GameObject newMissile = (GameObject) Network.Instantiate(missilePrefab, leftMissileOrigin.position, leftMissileOrigin.rotation, 0);
//					GameObject newMissile = (GameObject)Instantiate(missilePrefab, leftMissileOrigin.position, leftMissileOrigin.rotation);
					newMissile.GetComponent<Missile>().force = newMissile.GetComponent<Missile>().force * chargeTime;
				}
				else
				{
					GameObject newMissile = (GameObject) Network.Instantiate(missilePrefab, rightMissileOrigin.position, rightMissileOrigin.rotation, 0);
//					GameObject newMissile = (GameObject)Instantiate(missilePrefab, rightMissileOrigin.position, rightMissileOrigin.rotation);
					newMissile.GetComponent<Missile>().force = newMissile.GetComponent<Missile>().force * chargeTime;
				}
				chargeTime = 0f;
			}
			else
			{
				chargeTime = 0f;
			}
		}
	}

	void SetCrosshair(bool input)
	{
		if (input)
		{
			crosshair.parent.GetComponent<Canvas>().enabled = true;
		}
		else
		{
			crosshair.parent.GetComponent<Canvas>().enabled = false;
		}
	}

	void CalcHinges(HingeJoint xHinge, HingeJoint yHinge, Transform missileOrigin, Transform missileAim)
	{
//		 the pointer is 80 pixels from the top, centered in the x direction
		Vector3 rayOrigin = new Vector3(0.5f, (mainCamera.pixelHeight - 80.0f)/(float)mainCamera.pixelHeight, 0.0f);
		Ray ray = mainCamera.ViewportPointToRay (rayOrigin);
		Vector3 point = ray.GetPoint (600f);
//		point represents the point the turret wants to aim at
//		find angle that the turret needs to rotate in the x direction
//		find angle that turret needs to rotate in the y direction
//		clamp values if greater than limits

		Vector3 targetVector = (point - missileAim.position).normalized;
		Debug.DrawLine (missileAim.position, point);
		//float angle;
		float xAngle;
		float yAngle;
		float rAngle;
		float dist = targetVector.magnitude;
		float elev = Vector3.Dot (targetVector, transform.up);
		targetVector -= elev * transform.up;
		targetVector.Normalize ();

		xAngle = Vector3.Angle (missileAim.transform.forward, targetVector); //front, back
		rAngle = Vector3.Angle (missileAim.transform.right, targetVector); // left, right

		if (rAngle > 90)
			xAngle = -xAngle;
		Debug.Log (xAngle);

		yAngle = Mathf.Asin (elev / dist) * Mathf.Rad2Deg;

		JointSpring joint = xHinge.spring;
		joint.targetPosition = Mathf.Clamp (xAngle, xHinge.limits.min, xHinge.limits.max);
		xHinge.spring= joint;

		joint = yHinge.spring;
		joint.targetPosition = Mathf.Clamp (yAngle, yHinge.limits.min, yHinge.limits.max);
		yHinge.spring = joint;

		// now that the joints are aimed, place the crosshair's x and coordinates
		ray = new Ray (missileOrigin.position, missileOrigin.forward);
		Vector3 point2 = ray.GetPoint (Vector3.Distance(point, missileOrigin.position));
		Vector3 crosshairPos = mainCamera.WorldToScreenPoint (point2);
		crosshair.position = new Vector3 (crosshairPos.x, crosshairPos.y, 0f);
		Debug.DrawLine (missileOrigin.position, point2);
	}

	void ArrayActive(bool input)
	{
		if (input)
		{
			foreach (ConfigurableJoint cj in missileArrays)
				cj.targetPosition = new Vector3(0f, -8f, 0f);
		}
		else
		{
			
			foreach (ConfigurableJoint cj in missileArrays)
				cj.targetPosition = new Vector3(0f, 0f, 0f);
		}
	}
}