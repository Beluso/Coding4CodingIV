using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour 
{
	private static AudioManager _audioManager;
	private int numChannels = 16;
	private AudioSource audioSource;
	private Vector3 test;
	
	
	public static AudioManager Instance
	{
		get
		{
			if (_audioManager == null)
				_audioManager = new AudioManager();
			return _audioManager;
		}
	}
	
	private AudioManager()
	{
		audioSource = GetComponent<AudioSource> ();
		test = new Vector3 (0.5f, 1.0f, 0.5f);
		Debug.Log (test);
	}
	
	public void PlaySound(AudioClip audioClip)
	{
		audioSource.clip = audioClip;
		audioSource.Play();
	}
}
