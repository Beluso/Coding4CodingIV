using UnityEngine;
using System.Collections;

public class JoystickController : MonoBehaviour {
	
	/// <summary>
	/// This is a joystick detect demo/project. Made by project Team Unity~ from the Entertainment Technology Center at Carnegie Mellon.
	/// The purpose for this demo/project is to understand what is the mapping for you joystick. 
	/// </summary>
	
	private string currentButton;
	private string currentAxis;

	private float axisInput1;
	private float axisInput2;
	private float axisInput3;
	private float axisInput4;
	private float axisInput5;
	private float axisInput6;
	private float axisInput7;
	private float axisInput8;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		getAxis();
		getButton();
		
	
	}
	
	/// <summary>
	/// Get Axis data of the joysick
	/// </summary>
	void getAxis()
	{
		if(Input.GetAxisRaw("X axis")> 0.3|| Input.GetAxisRaw("X axis") < -0.3)
		{
			currentAxis = "X axis";
			axisInput1 = Input.GetAxisRaw("X axis");
		}
		
		if(Input.GetAxisRaw("Y axis")> 0.3|| Input.GetAxisRaw("Y axis") < -0.3)
		{
			currentAxis = "Y axis";
			axisInput2 = Input.GetAxisRaw("Y axis");
		}
		
		if(Input.GetAxisRaw("3rd axis")> 0.3|| Input.GetAxisRaw("3rd axis") < -0.3)
		{
			currentAxis = "3rd axis";
			axisInput3 = Input.GetAxisRaw("3rd axis");
		}
		
		if(Input.GetAxisRaw("4th axis")> 0.3|| Input.GetAxisRaw("4th axis") < -0.3)
		{
			currentAxis = "4th axis";
			axisInput4 = Input.GetAxisRaw("4th axis");
		}
		
		if(Input.GetAxisRaw("5th axis")> 0.3|| Input.GetAxisRaw("5th axis") < -0.3)
		{
			currentAxis = "5th axis";
			axisInput5 = Input.GetAxisRaw("5th axis");
		}
		
		if(Input.GetAxisRaw("6th axis")> 0.3|| Input.GetAxisRaw("6th axis") < -0.3)
		{
			currentAxis = "6th axis";
			axisInput6 = Input.GetAxisRaw("6th axis");
		}
		
		if(Input.GetAxisRaw("7th axis")> 0.3|| Input.GetAxisRaw("7th axis") < -0.3)
		{
			currentAxis = "7th axis";
			axisInput7 = Input.GetAxisRaw("7th axis");
		}
		
		if(Input.GetAxisRaw("8th axis") > 0.3|| Input.GetAxisRaw("8th axis") < -0.3)
		{
			currentAxis = "8th axis";
			axisInput8 = Input.GetAxisRaw("8th axis");
		}
		Debug.Log (Input.GetAxisRaw ("X axis"));
	}
	
	/// <summary>
	/// get the button data of the joystick
	/// </summary>
	void getButton()
	{
		if(Input.GetButton("joystick button 0"))
			currentButton = "joystick button 0";
		   
		if(Input.GetButton("joystick button 1"))
			currentButton = "joystick button 1";
		   
		if(Input.GetButton("joystick button 2"))
			currentButton = "joystick button 2";
		   
		if(Input.GetButton("joystick button 3"))
			currentButton = "joystick button 3";
		   
		if(Input.GetButton("joystick button 4"))
			currentButton = "joystick button 4";
		   
		if(Input.GetButton("joystick button 5"))
			currentButton = "joystick button 5";
		   
		if(Input.GetButton("joystick button 6"))
			currentButton = "joystick button 6";
		   
		if(Input.GetButton("joystick button 7"))
			currentButton = "joystick button 7";
		   
		if(Input.GetButton("joystick button 8"))
			currentButton = "joystick button 8";
		   
		if(Input.GetButton("joystick button 9"))
			currentButton = "joystick button 9";
		   
		if(Input.GetButton("joystick button 10"))
			currentButton = "joystick button 10";
		   
		if(Input.GetButton("joystick button 11"))
			currentButton = "joystick button 11";
		   
		if(Input.GetButton("joystick button 12"))
			currentButton = "joystick button 12";
		   
		if(Input.GetButton("joystick button 13"))
			currentButton = "joystick button 13";
		   
		if(Input.GetButton("joystick button 14"))
			currentButton = "joystick button 14";
		
		if(Input.GetButton("joystick button 15"))
			currentButton = "joystick button 15";
		
		if(Input.GetButton("joystick button 16"))
			currentButton = "joystick button 16";
		
		if(Input.GetButton("joystick button 17"))
			currentButton = "joystick button 17";
		
		if(Input.GetButton("joystick button 18"))
			currentButton = "joystick button 18";
		
		if(Input.GetButton("joystick button 19"))
			currentButton = "joystick button 19";	   
	}
	
	/// <summary>
	/// show the data onGUI
	/// </summary>
	void OnGUI()
	{
		GUILayout.BeginArea (new Rect (0, 0, Screen.width, Screen.height));
		GUILayout.TextArea( "Current Button : " + currentButton);
		GUILayout.TextArea( "Current Axis : " + currentAxis);
		GUILayout.TextArea( "Axis Value : " +  axisInput1);
		GUILayout.TextArea( "Axis Value : " +  axisInput2);
		GUILayout.TextArea( "Axis Value : " +  axisInput3);
		GUILayout.TextArea( "Axis Value : " +  axisInput4);
		GUILayout.TextArea( "Axis Value : " +  axisInput5);
		GUILayout.TextArea( "Axis Value : " +  axisInput6);
		GUILayout.TextArea( "Axis Value : " +  axisInput7);
		GUILayout.TextArea( "Axis Value : " +  axisInput8);
		GUILayout.EndArea ();
	}
}
